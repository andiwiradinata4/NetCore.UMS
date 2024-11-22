using AW.Core.DTOs;
using AW.Infrastructure.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Contexts;
using UMS.Core.DTOs;
using UMS.Core.Entities;

namespace UMS.Infrastructure.Interfaces.Services
{
    public interface ITokenService : IBaseService<UMSDbContext, AppToken>
    {
        Task<MessageObject<VerifyTokenDTO>> EmailConfirmationToken(string userId, AppToken.TokenTypeValue tokenType);
        MessageObject<TokenDTO> LoginToken(string username, string password);
        MessageObject<TokenDTO> RefreshToken(string refreshToken);
        Task<MessageObject<VerifyTokenDTO>> ResetPasswordToken(string email);
        Task<MessageObject<bool>> VerifyToken(string userId, VerifyTokenDTO dto, AppToken.TokenTypeValue tokenType);
    }
}
