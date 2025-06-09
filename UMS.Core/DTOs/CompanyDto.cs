using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AW.Core.Entities;

namespace UMS.Core.DTOs
{
	public class CompanyDto : BaseEntity
	{
		public string AppProjectId { get; set; } = string.Empty;
		public AppProjectDto? AppProject { get; set; }
		public string Initial { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public string CountryId { get; set; } = string.Empty;
		public string CountryCode { get; set; } = string.Empty;
		public string CountryName { get; set; } = string.Empty;
		public string ProvinceId { get; set; } = string.Empty;
		public string ProvinceName { get; set; } = string.Empty;
		public string CityId { get; set; } = string.Empty;
		public string CityName { get; set; } = string.Empty;
		public string SubDistrictId { get; set; } = string.Empty;
		public string SubDistrictName { get; set; } = string.Empty;
		public string AreaId { get; set; } = string.Empty;
		public string AreaName { get; set; } = string.Empty;
		public string PostalCode { get; set; } = string.Empty;
	}
}
