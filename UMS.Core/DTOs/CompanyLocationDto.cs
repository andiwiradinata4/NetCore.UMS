using AW.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.DTOs
{
	public class CompanyLocationDto : BaseEntity
	{
		public string Initial { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string CompanyId { get; set; } = string.Empty;
		public CompanyDto? Company { get; set; }
	}
}
