using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.DTOs
{
    public class VerifyTokenDTO
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
        public string? NewValue { get; set; }
        public string? ConfirmNewValue { get; set; }
    }
}
