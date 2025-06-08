using AW.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.Entities
{
	[Table("UMS_sysToken")]
	public class AppToken : BaseEntity
	{
		public string UserID { get; set; } = string.Empty;
		public string TokenType { get; set; } = string.Empty;
		public string TokenValue { get; set; } = string.Empty;
		[StringLength(256)]
		public string Code { get; set; } = string.Empty;
		public DateTime ValidFrom { get; set; }
		public DateTime ValidTo { get; set; }
		public enum TokenTypeValue
		{
			EMAIL_CONFIRMATION,
			CHANGE_EMAIL,
			LOGIN_TOKEN,
			REFRESH_TOKEN,
			RESET_PASSWORD_TOKEN,
		}
	}
}
