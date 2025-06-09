using AW.Core.Contexts;
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
	public interface IAppProjectService : IBaseService<UMSDbContext, AppProjectDto>
	{
		Task<MessageGetList<AppProjectDto>> GetAllDataProxy(QueryObject query, string additionalName = "");
		Task<AppProjectDto?> GetByIdProxy(string Id, QueryObject query);
	}
}
