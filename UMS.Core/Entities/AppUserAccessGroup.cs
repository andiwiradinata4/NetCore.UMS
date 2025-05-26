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
        public string AppSystemId { get; set; } = string.Empty;
        public string AppSystemCode { get; set; } = string.Empty;
        public string AppSystemName { get; set; } = string.Empty;
        public string AppProjectId { get; set; } = string.Empty;
        public string AppProjectCode { get; set; } = string.Empty;
        public string AppProjectName { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public string CompanyInitial { get; set; } = string.Empty;
        public string CompanyDescription { get; set; } = string.Empty;
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
