using AW.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.DTOs
{
	public class AppProjectDto : BaseEntity
	{
		public string Code { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string AppSystemId { get; set; } = string.Empty;
		public AppSystemDto? AppSystem { get; set; }
	}
}
