﻿using AW.Core.DTOs;
using AW.Infrastructure.Interfaces.Services;
using AW.Web.Controllers.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UMS.Core.Contexts;
using UMS.Core.DTOs;
using UMS.Core.Entities;
using UMS.Infrastructure.Interfaces.Services;

namespace UMS.Web.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TokenController : BaseController<UMSDbContext, AppToken>
    {
        private readonly ITokenService _svc;
        private readonly IClientCredentialService _clientCredentialSvc;
        public TokenController(IBaseService<UMSDbContext, AppToken> baseSvc, ITokenService svc, IClientCredentialService clientCredentialSvc) : base(baseSvc)
        {
            _svc = svc; _clientCredentialSvc = clientCredentialSvc;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO dto)
        {
            MessageObject<TokenDTO> messageObject = _svc.LoginToken(dto.Username, dto.Password);
            if (messageObject.ProcessingStatus)
            {
                return Ok(messageObject);
            }
            return BadRequest(messageObject);
        }

        [HttpPost("login/secure")]
        public IActionResult LoginSecure([FromBody] LoginDTO dto)
        {
            MessageObject<TokenDTO> messageObject = _svc.LoginToken(dto.Username, dto.Password);
            if (messageObject.ProcessingStatus)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = messageObject.Data.ValidTo,
                };

                Response.Cookies.Append("AuthToken", messageObject.Data.AccessToken, cookieOptions);
                Response.Cookies.Append("RefreshToken", messageObject.Data.RefreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = messageObject.Data.ValidFrom.AddDays(7)
                    });
                return Ok(new { message = "Login successful" });
            }
            return BadRequest(messageObject);
        }

        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            MessageObject<TokenDTO> messageObject = new MessageObject<TokenDTO>();
            string? refreshToken = HttpContext.Request.Headers["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) messageObject.AddMessage(new Message(MessageType.Error, "401", "Unauthorized", "RefreshToken"));
            messageObject = _svc.RefreshToken(refreshToken!);
            if (messageObject.ProcessingStatus) return Ok(messageObject);
            return Unauthorized(messageObject);
        }

        [HttpPost("refresh/secure")]
        public IActionResult RefreshSecure()
        {
            MessageObject<TokenDTO> messageObject = new MessageObject<TokenDTO>();
            string? refreshToken = Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) messageObject.AddMessage(new Message(MessageType.Error, "401", "Unauthorized", "RefreshToken"));
            messageObject = _svc.RefreshToken(refreshToken!);

            if (messageObject.ProcessingStatus)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = messageObject.Data.ValidTo,
                };

                Response.Cookies.Append("AuthToken", messageObject.Data.AccessToken, cookieOptions);
                Response.Cookies.Append("RefreshToken", messageObject.Data.RefreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = messageObject.Data.ValidFrom.AddDays(7)
                    });
                return Ok(new { message = "Token refreshed successfully" });
            }
            return Unauthorized(messageObject);
        }

        [HttpPost("email-confirmation")]
        [Authorize]
        public async Task<IActionResult> EmailConfirmation()
        {
            MessageObject<VerifyTokenDTO> messageObject = new MessageObject<VerifyTokenDTO>();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                messageObject.AddMessage(new Message(MessageType.Error, "401", "Unauthorized", "UserId"));
                return Unauthorized(messageObject);
            }
            messageObject = await _svc.EmailConfirmationToken(userId!, AppToken.TokenTypeValue.EMAIL_CONFIRMATION);
            if (messageObject.ProcessingStatus) return Ok(messageObject);
            return BadRequest(messageObject);
        }

        [HttpPost("verify-email-confirmation")]
        [Authorize]
        public async Task<IActionResult> VerifyEmailConfirmation([FromBody] VerifyTokenDTO dto)
        {
            MessageObject<bool> messageObject = new MessageObject<bool>();
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userId))
            //{
            //    messageObject.AddMessage(new Message(MessageType.Error, "401", "Unauthorized", "UserId"));
            //    return Unauthorized(messageObject);
            //}
            messageObject = await _svc.VerifyToken(dto, AppToken.TokenTypeValue.EMAIL_CONFIRMATION);
            if (messageObject.ProcessingStatus) return Ok(messageObject);
            return BadRequest(messageObject);
        }

        [HttpPost("change-email")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail()
        {
            MessageObject<VerifyTokenDTO> messageObject = new MessageObject<VerifyTokenDTO>();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                messageObject.AddMessage(new Message(MessageType.Error, "401", "Unauthorized", "UserId"));
                return Unauthorized(messageObject);
            }
            messageObject = await _svc.EmailConfirmationToken(userId!, AppToken.TokenTypeValue.CHANGE_EMAIL);
            if (messageObject.ProcessingStatus) return Ok(messageObject);
            return BadRequest(messageObject);
        }

        [HttpPost("verify-change-email")]
        public async Task<IActionResult> VerifyChangeEmail([FromBody] VerifyTokenDTO dto)
        {
            MessageObject<bool> messageObject = new MessageObject<bool>();
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userId))
            //{
            //    messageObject.AddMessage(new Message(MessageType.Error, "401", "Unauthorized", "UserId"));
            //    return Unauthorized(messageObject);
            //}
            messageObject = await _svc.VerifyToken(dto, AppToken.TokenTypeValue.CHANGE_EMAIL);
            if (messageObject.ProcessingStatus) return Ok(messageObject);
            return BadRequest(messageObject);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            MessageObject<VerifyTokenDTO> messageObject = await _svc.ResetPasswordToken(dto.Email);
            if (messageObject.ProcessingStatus) return Ok(messageObject);
            return BadRequest(messageObject);
        }

        [HttpPost("verify-reset-password")]
        public async Task<IActionResult> VerifyResetPassword([FromBody] VerifyTokenDTO dto)
        {
            MessageObject<bool> messageObject = new MessageObject<bool>();
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userId))
            //{
            //    messageObject.AddMessage(new Message(MessageType.Error, "401", "Unauthorized", "UserId"));
            //    return Unauthorized(messageObject);
            //}
            messageObject = await _svc.VerifyToken(dto, AppToken.TokenTypeValue.RESET_PASSWORD_TOKEN);
            if (messageObject.ProcessingStatus) return Ok(messageObject);
            return BadRequest(messageObject);
        }

    }
}
