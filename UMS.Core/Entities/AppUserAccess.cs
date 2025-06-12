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
    [Table("UMS_mstUserAccess")]
    public class AppUserAccess : BaseEntity
    {
        [Required]
        [ForeignKey("AppUser")]
        public string UserId { get; set; } = string.Empty;
        public AppUser? AppUser { get; set; }
        [Required]
        [ForeignKey("AppUserAccessGroup")]
        public string UserGroupId { get; set; } = string.Empty;
        public AppUserAccessGroup? AppUserAccessGroup { get; set; }
    }
}