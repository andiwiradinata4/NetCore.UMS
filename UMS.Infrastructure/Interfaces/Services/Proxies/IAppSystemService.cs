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
	public interface IAppSystemService : IBaseService<UMSDbContext, AppSystemDto>
	{
		Task<MessageGetList<AppSystemDto>> GetAllDataProxy(QueryObject query, string additionalName = "");
		Task<AppSystemDto?> GetByIdProxy(string Id, QueryObject query);
	}
}
