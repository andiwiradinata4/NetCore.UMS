using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.DTOs
{
    public class TokenDTO
    {
        public string UserName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string AccessToken { get; set; } = String.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string RefreshToken { get; set; } = String.Empty;
        public string Issuer { get; set; } = String.Empty;
        public string Audience { get; set; } = String.Empty;
    }
}
