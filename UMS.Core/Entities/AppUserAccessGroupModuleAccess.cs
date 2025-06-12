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
    [Table("UMS_mstUserAccessGroupModuleAccess")]
    public class AppUserAccessGroupModuleAccess: BaseEntity
    {
        [Required]
        public string AppAccessId { get; set; } = string.Empty;
        [Required]
        [ForeignKey("AppUserAccessGroupModule")]
        public string AppUserAccessGroupModuleId { get; set; } = string.Empty;
        public AppUserAccessGroupModule? AppUserAccessGroupModule { get; set; }
    }
}