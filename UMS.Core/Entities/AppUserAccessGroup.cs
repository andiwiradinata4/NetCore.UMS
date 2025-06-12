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
        [Required]
        public string AppSystemId { get; set; } = string.Empty;
        [Required]
        public string AppProjectId { get; set; } = string.Empty;
        [Required]
        public string CompanyId { get; set; } = string.Empty;
        [Required]
        public string CompanyLocationId { get; set; } = string.Empty;
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
