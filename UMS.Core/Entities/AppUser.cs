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
    [Table("UMS_mstUser")]
    public class AppUser : BaseEntity
    {
        [StringLength(256)]
        public string Username { get; set; } = string.Empty;
        [StringLength(256)]
        public string Name { get; set; } = string.Empty;
        [StringLength(256)]
        public string NormalizedUserName { get; set; } = string.Empty;
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;
        [StringLength(256)]
        public string NormalizedEmail { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        [StringLength(256)]
        public string PhoneNumber { get; set; } = string.Empty;
        public bool PhoneNumberConfirmed { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        [NotMapped]
        public List<AppUserRole> Roles { get; set; } = [];
    }
}
