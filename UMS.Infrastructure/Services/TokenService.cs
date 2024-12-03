using AW.Core.DTOs;
using AW.Infrastructure.Interfaces.Repositories;
using AW.Infrastructure.Services;
using AW.Infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Contexts;
using UMS.Core.DTOs;
using UMS.Core.Entities;
using UMS.Infrastructure.Interfaces.Repositories;
using UMS.Infrastructure.Interfaces.Services;

namespace UMS.Infrastructure.Services
{
    public class TokenService : BaseService<UMSDbContext, AppToken>, ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepo;

        public TokenService(IBaseRepository<UMSDbContext, AppToken> repo, IConfiguration configuration, IUserRepository userRepo) : base(repo)
        {
            _configuration = configuration;
            _userRepo = userRepo;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"] ?? ""));
        }

        public MessageObject<TokenDTO> LoginToken(string username, string password)
        {
            UMSDbContext context = base.repo.GetDbContext();
            MessageObject<TokenDTO> message = new MessageObject<TokenDTO>();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var data = _userRepo.GetByConditionAsQueryable(e => e.Username == username);
                    if (data.Count() == 0)
                    {
                        message.AddMessage(new Message(MessageType.Error, "400", "Invalid username"));
                        return message;
                    }

                    AppUser user = data.First();
                    bool isValid = PasswordHasher.VerifyPassword(password, user.PasswordHash);
                    if (!isValid)
                    {
                        message.AddMessage(new Message(MessageType.Error, "400", "Invalid username and password"));
                        return message;
                    }

                    if (string.IsNullOrEmpty(user.Id))
                    {
                        message.AddMessage(new Message(MessageType.Error, "400", "User ID not Valid!"));
                        return message;
                    }

                    TokenDTO token = GenerateJWTToken(user, AppToken.TokenTypeValue.LOGIN_TOKEN);
                    var refreshToken = GenerateRefreshToken();
                    var tokenExists = base.repo.GetByConditionAsQueryableWithDisabledRecord(e => e.UserID == user.Id && (e.TokenType == AppToken.TokenTypeValue.LOGIN_TOKEN.ToString() || e.TokenType == AppToken.TokenTypeValue.REFRESH_TOKEN.ToString())).ToList();
                    if (tokenExists.Count > 0) context.RemoveRange(tokenExists);

                    message.Data = new()
                    {
                        UserName = user.Username ?? "",
                        Email = user.Email ?? "",
                        AccessToken = token.AccessToken,
                        ValidFrom = token.ValidFrom,
                        ValidTo = token.ValidTo,
                        RefreshToken = refreshToken,
                        Issuer = _configuration["JWT:Issuer"] ?? "",
                        Audience = _configuration["JWT:Audience"] ?? ""
                    };

                    base.repo.CreateAsync(new AppToken()
                    {
                        UserID = user.Id,
                        TokenType = AppToken.TokenTypeValue.LOGIN_TOKEN.ToString(),
                        TokenValue = message.Data.AccessToken,
                        ValidFrom = message.Data.ValidFrom,
                        ValidTo = message.Data.ValidTo,
                    }, true);

                    base.repo.CreateAsync(new AppToken
                    {
                        UserID = user.Id,
                        TokenType = AppToken.TokenTypeValue.REFRESH_TOKEN.ToString(),
                        TokenValue = refreshToken,
                        ValidFrom = DateTime.Today,
                        ValidTo = DateTime.Today.AddDays(7),
                    }, true);

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    /// Log Error
                    Console.WriteLine(ex.Message);
                    message.AddException(ex);
                    transaction.Rollback();
                }
            }
            return message;
        }

        public MessageObject<TokenDTO> RefreshToken(string refreshToken)
        {
            UMSDbContext context = base.repo.GetDbContext();
            MessageObject<TokenDTO> message = new MessageObject<TokenDTO>();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var tokenExists = base.repo.GetByConditionAsQueryableWithDisabledRecord(e => e.TokenType == AppToken.TokenTypeValue.REFRESH_TOKEN.ToString() && e.TokenValue == refreshToken);
                    if (tokenExists.Count() == 0)
                    {
                        message.AddMessage(new Message(MessageType.Error, "400", "Invalid refresh token"));
                        return message;
                    }
                    string nowString = DateTime.Now.ToString("yyyyMMddHHmmss");
                    Int64 nowResult = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    Int64 expiredResult = Convert.ToInt64(tokenExists.First().ValidTo.ToString("yyyyMMddHHmmss"));

                    if (nowResult > expiredResult)
                    {
                        message.AddMessage(new Message(MessageType.Error, "400", "Refresh token has been expired"));
                        return message;
                    }

                    var data = _userRepo.GetByConditionAsQueryable(e => e.Id == tokenExists.First().UserID);
                    AppUser user = data.First();
                    if (data.Count() == 0)
                    {
                        message.AddMessage(new Message(MessageType.Error, "400", "User not valid"));
                        return message;
                    }

                    if (string.IsNullOrEmpty(user.Id)) 
                    {
                        message.AddMessage(new Message(MessageType.Error, "400", "User ID not Valid!"));
                        return message;
                    }

                    TokenDTO newToken = GenerateJWTToken(user, AppToken.TokenTypeValue.LOGIN_TOKEN);
                    var newRefreshToken = GenerateRefreshToken();
                    var oldTokenExists = base.repo.GetByConditionAsQueryableWithDisabledRecord(e => e.UserID == user.Id && (e.TokenType == AppToken.TokenTypeValue.LOGIN_TOKEN.ToString() || e.TokenType == AppToken.TokenTypeValue.REFRESH_TOKEN.ToString())).ToList();
                    if (oldTokenExists.Count > 0) context.RemoveRange(oldTokenExists);

                    message.Data = new()
                    {
                        UserName = user.Username ?? "",
                        Email = user.Email ?? "",
                        AccessToken = newToken.AccessToken,
                        ValidFrom = newToken.ValidFrom,
                        ValidTo = newToken.ValidTo,
                        RefreshToken = refreshToken,
                        Issuer = _configuration["JWT:Issuer"] ?? "",
                        Audience = _configuration["JWT:Audience"] ?? ""
                    };

                    base.repo.CreateAsync(new AppToken()
                    {
                        UserID = user.Id,
                        TokenType = AppToken.TokenTypeValue.LOGIN_TOKEN.ToString(),
                        TokenValue = newToken.AccessToken,
                        ValidFrom = message.Data.ValidFrom,
                        ValidTo = message.Data.ValidTo,
                    }, true);

                    base.repo.Create(new AppToken
                    {
                        UserID = user.Id,
                        TokenType = AppToken.TokenTypeValue.REFRESH_TOKEN.ToString(),
                        TokenValue = refreshToken,
                        ValidFrom = DateTime.Now,
                        ValidTo = DateTime.Now.AddDays(7),
                    }, true);

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    /// Log Error
                    Console.WriteLine(ex.Message);
                    message.AddException(ex);
                    transaction.Rollback();
                }
            }
            return message;
        }

        public async Task<MessageObject<VerifyTokenDTO>> EmailConfirmationToken(string userId, AppToken.TokenTypeValue tokenType)
        {
            UMSDbContext context = base.repo.GetDbContext();
            MessageObject<VerifyTokenDTO> messageObject = new MessageObject<VerifyTokenDTO>();
            var user = await _userRepo.GetByIDAsync(userId);
            if (user == null) messageObject.AddMessage(new Message(MessageType.Error, "401", "User not found", "UserId"));

            if (messageObject.ProcessingStatus)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        TokenDTO newToken = GenerateJWTToken(user!, tokenType);
                        var oldTokenExists = base.repo.GetByConditionAsQueryableWithDisabledRecord(e => e.UserID == userId && e.TokenType == tokenType.ToString()).ToList();
                        if (oldTokenExists.Count > 0) context.RemoveRange(oldTokenExists);

                        messageObject.Data = new()
                        {
                            Token = newToken.AccessToken,
                            Code = GenerateCode()
                        };

                        await base.repo.CreateAsync(new AppToken()
                        {
                            UserID = userId,
                            TokenType = tokenType.ToString(),
                            TokenValue = messageObject.Data.Token,
                            ValidFrom = newToken.ValidFrom,
                            ValidTo = newToken.ValidFrom.AddMinutes(5),
                            Code = messageObject.Data.Code
                        }, true);

                        context.SaveChanges();
                        transaction.Commit();

                        /// Send Email Notification
                    }
                    catch (Exception ex)
                    {
                        /// Log Error
                        Console.WriteLine(ex.Message);
                        messageObject.AddException(ex);
                        transaction.Rollback();
                    }
                }
            }
            return messageObject;
        }

        public async Task<MessageObject<VerifyTokenDTO>> ResetPasswordToken(string email)
        {
            UMSDbContext context = base.repo.GetDbContext();
            MessageObject<VerifyTokenDTO> messageObject = new MessageObject<VerifyTokenDTO>();
            var allUser = _userRepo.GetByConditionAsQueryableWithDisabledRecord(e => e.Email == email);
            if (allUser.Count() == 0)
            {
                messageObject.AddMessage(new Message(MessageType.Error, "400", "Email not found", "Email"));
                return messageObject;
            }

            var user = allUser.First();

            if (messageObject.ProcessingStatus)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        TokenDTO newToken = GenerateJWTToken(user!, AppToken.TokenTypeValue.RESET_PASSWORD_TOKEN);
                        var oldTokenExists = base.repo.GetByConditionAsQueryableWithDisabledRecord(e => e.UserID == user.Id && e.TokenType == AppToken.TokenTypeValue.RESET_PASSWORD_TOKEN.ToString()).ToList();
                        if (oldTokenExists.Count > 0) context.RemoveRange(oldTokenExists);

                        messageObject.Data = new()
                        {
                            Token = newToken.AccessToken,
                            Code = GenerateCode()
                        };

                        await base.repo.CreateAsync(new AppToken()
                        {
                            UserID = user.Id,
                            TokenType = AppToken.TokenTypeValue.RESET_PASSWORD_TOKEN.ToString(),
                            TokenValue = messageObject.Data.Token,
                            ValidFrom = newToken.ValidFrom,
                            ValidTo = newToken.ValidFrom.AddMinutes(5),
                            Code = messageObject.Data.Code
                        }, true);

                        context.SaveChanges();
                        transaction.Commit();

                        /// Send Email Notification
                    }
                    catch (Exception ex)
                    {
                        /// Log Error
                        Console.WriteLine(ex.Message);
                        messageObject.AddException(ex);
                        transaction.Rollback();
                    }
                }
            }
            return messageObject;
        }

        public async Task<MessageObject<bool>> VerifyToken(string userId, VerifyTokenDTO dto, AppToken.TokenTypeValue tokenType)
        {
            UMSDbContext context = base.repo.GetDbContext();
            MessageObject<bool> messageObject = new MessageObject<bool>();

            AppUser? user;

            if (tokenType == AppToken.TokenTypeValue.RESET_PASSWORD_TOKEN)
            {
                if (string.IsNullOrEmpty(dto.NewValue) || string.IsNullOrEmpty(dto.ConfirmNewValue))
                {
                    messageObject.AddMessage(new Message(MessageType.Error, "400", "Password and Confirm Password not allow blank", "Password"));
                    return messageObject;
                }
                else if (dto.NewValue != dto.ConfirmNewValue)
                {
                    messageObject.AddMessage(new Message(MessageType.Error, "400", "Password and Confirm Password not match", "Password"));
                    return messageObject;
                }

                var oldToken = base.repo.GetByConditionAsQueryableWithDisabledRecord(e => e.TokenType == tokenType.ToString() && e.TokenValue == dto.Token && e.Code == dto.Code).ToList();
                if (oldToken.Count() == 0)
                {
                    messageObject.AddMessage(new Message(MessageType.Error, "400", "Unauthorized", "Token"));
                    return messageObject;
                }
                user = await _userRepo.GetByIDAsync(oldToken.First().UserID);
            }
            else
            {
                user = await _userRepo.GetByIDAsync(userId);
            }

            if (user == null) messageObject.AddMessage(new Message(MessageType.Error, "400", "User not found", "UserId"));

            var oldTokenExists = base.repo.GetByConditionAsQueryableWithDisabledRecord(e => e.UserID == userId && e.TokenType == tokenType.ToString() && e.TokenValue != "SUCCESS").ToList();
            if (oldTokenExists.Count() == 0)
            {
                messageObject.AddMessage(new Message(MessageType.Error, "400", "Unauthorized", "Token"));
                return messageObject;
            };

            if (tokenType == AppToken.TokenTypeValue.CHANGE_EMAIL)
            {
                if (string.IsNullOrEmpty(dto.NewValue))
                {
                    messageObject.AddMessage(new Message(MessageType.Error, "400", "New Email not allow blank", "NewValue"));
                    return messageObject;
                }
                else if (_userRepo.ExistsInDbWithDisabledRecord(e => e.Email == dto.NewValue && e.Id != user!.Id))
                {
                    messageObject.AddMessage(new Message(MessageType.Error, "400", "New Email already exist", "NewValue"));
                    return messageObject;
                }
            }

            AppToken token = oldTokenExists.First();
            if (token.TokenValue != dto.Token)
            {
                messageObject.AddMessage(new Message(MessageType.Error, "400", "Unauthorized", "Token"));
                return messageObject;
            }

            if (token.Code != dto.Code)
            {
                messageObject.AddMessage(new Message(MessageType.Error, "400", "Invalid code", "Code"));
                return messageObject;
            }

            if (messageObject.ProcessingStatus)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        token.TokenValue = "SUCCESS";
                        user!.EmailConfirmed = true;

                        if (tokenType == AppToken.TokenTypeValue.CHANGE_EMAIL)
                        {
                            user!.Email = dto.NewValue!;
                            user!.NormalizedEmail = dto.NewValue!;
                        }
                        else if (tokenType == AppToken.TokenTypeValue.RESET_PASSWORD_TOKEN)
                        {
                            user!.PasswordHash = PasswordHasher.HashPassword(dto.NewValue!);

                            var allTokenLogin = base.repo.GetByConditionAsQueryable(e => e.UserID == user!.Id && (e.TokenType == AppToken.TokenTypeValue.LOGIN_TOKEN.ToString() || e.TokenType == AppToken.TokenTypeValue.REFRESH_TOKEN.ToString()));
                            if (allTokenLogin.Count() > 0) context.RemoveRange(allTokenLogin);
                        }

                        context.SaveChanges();
                        transaction.Commit();
                        messageObject.Data = true;
                    }
                    catch (Exception ex)
                    {
                        /// Log Error
                        Console.WriteLine(ex.Message);
                        messageObject.AddException(ex);
                        transaction.Rollback();
                    }
                }
            }
            return messageObject;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private TokenDTO GenerateJWTToken(AppUser user, AppToken.TokenTypeValue tokenType)
        {
            var claims = new List<Claim> {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id ?? ""),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Name, user.Name ?? ""),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                        new Claim(JwtRegisteredClaimNames.GivenName, user.Username ?? "")
                    };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                IssuedAt = DateTime.Now,
                TokenType = tokenType.ToString()
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var resultToken = tokenHandler.WriteToken(token);

            TokenDTO tokenDTO = new()
            {
                UserName = user.Username ?? "",
                Email = user.Email ?? "",
                AccessToken = resultToken,
                ValidFrom = token.ValidFrom,
                ValidTo = token.ValidTo,
                RefreshToken = "",
                Issuer = _configuration["JWT:Issuer"] ?? "",
                Audience = _configuration["JWT:Audience"] ?? ""
            };
            return tokenDTO;
        }

        private string GenerateCode()
        {
            string code = "";
            Random rdm = new Random();
            for (int i = 0; i < 6; i++)
            {
                code += rdm.Next(0, 9).ToString();
            }
            return code;
        }
    }
}
