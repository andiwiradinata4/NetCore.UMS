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
	[Table("UMS_mstRole")]
	public class AppRole : BaseEntity
	{
		[StringLength(500)]
		public string Name { get; set; } = string.Empty;
		[StringLength(500)]
		public string Description { get; set; } = string.Empty;
	}
}
