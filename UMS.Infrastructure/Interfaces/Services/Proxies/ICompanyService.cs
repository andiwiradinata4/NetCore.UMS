using AW.Core.DTOs;
using AW.Infrastructure.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Contexts;
using UMS.Core.DTOs;

namespace UMS.Infrastructure.Interfaces.Services.Proxies
{
	public interface ICompanyService : IBaseService<UMSDbContext, CompanyDto>
	{
		Task<MessageGetList<CompanyDto>> GetAllDataProxy(QueryObject query, string additionalName = "");
		Task<CompanyDto?> GetByIdProxy(string Id, QueryObject query);
	}
}
