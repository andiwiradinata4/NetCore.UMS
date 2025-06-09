using AW.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.Entities
{
    [Table("UMS_mstUserAccessGroupModule")]
    public class AppUserAccessGroupModule : BaseEntity
    {
        public string AppModuleId { get; set; } = string.Empty;
        [NotMapped]
        public object? AppModule { get; set; }
        [ForeignKey("AppUserAccessGroup")]
        public string AppUserAccessGroupId { get; set; } = string.Empty;
        public AppUserAccessGroup? AppUserAccessGroup { get; set; }
        [NotMapped]
        public List<AppUserAccessGroupModuleAccess> AppModuleAccesses { get; set; } = [];
    }
}
