using AW.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.DTOs;

namespace UMS.Core.Entities
{
    [Table("UMS_mstUserAccessGroupModule")]
    public class AppUserAccessGroupModule : BaseEntity
    {
        [Required]
        public string AppModuleId { get; set; } = string.Empty;
        [NotMapped]
        public AppModuleDto? AppModule { get; set; }
        [Required]
        [ForeignKey("AppUserAccessGroup")]
        public string AppUserAccessGroupId { get; set; } = string.Empty;
        public AppUserAccessGroup? AppUserAccessGroup { get; set; }
        [NotMapped]
        public List<AppUserAccessGroupModuleAccess> AppModuleAccesses { get; set; } = [];
    }
}
