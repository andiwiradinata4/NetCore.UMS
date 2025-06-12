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
	[Table("UMS_mstUserRole")]
	public class AppUserRole : BaseEntity
	{
        [Required]
        [ForeignKey("AppUser")]
		public string UserId { get; set; } = string.Empty;
		public AppUser? AppUser { get; set; }
        [Required]
        [ForeignKey("AppRole")]
		public string AppRoleId { get; set; } = string.Empty;
		public AppRole? AppRole { get; set; }
		[NotMapped]
		public string AppRoleName => AppRole?.Name ?? string.Empty;
	}
}
