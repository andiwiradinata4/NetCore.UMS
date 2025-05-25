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
    [Table("UMS_mstUserAccessGroup")]
    public class AppUserAccessGroup : BaseEntity
    {
        [ForeignKey("AppUserAccess")]
        public string AppUserAccessId { get; set; } = string.Empty;
        public AppUserAccess? AppUserAccess { get; set; }
        public AppUser? AppUser { get; set; }
        [StringLength(256)]
        public string Code { get; set; } = string.Empty;
        [StringLength(256)]
        public string Name { get; set; } = string.Empty;
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        [NotMapped]
        public List<AppUserAccessGroupModule> AppModules { get; set; } = [];
    }
}
