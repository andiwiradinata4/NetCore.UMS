using AW.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.Entities
{
	[Table("UMS_mstUserRole")]
	public class AppUserRole : BaseEntity
	{
		[ForeignKey("AppUser")]
		public string UserId { get; set; } = string.Empty;
		public AppUser? AppUser { get; set; }
		[ForeignKey("AppRole")]
		public string AppRoleId { get; set; } = string.Empty;
		public AppRole? AppRole { get; set; }
		[NotMapped]
		public string AppRoleName => AppRole?.Name ?? string.Empty;
	}
}
