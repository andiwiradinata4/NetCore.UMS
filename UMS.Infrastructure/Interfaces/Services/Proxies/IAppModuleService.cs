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
    public interface IAppModuleService : IBaseService<UMSDbContext, AppModuleDto>
    {
        Task<MessageGetList<AppModuleDto>> GetAllDataProxy(QueryObject query, string additionalName = "");
        Task<AppModuleDto?> GetByIdProxy(string Id, QueryObject query);
    }
}
