using AW.Infrastructure.Interfaces.Repositories;
using AW.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Contexts;
using UMS.Core.Entities;
using UMS.Infrastructure.Interfaces.Services;

namespace UMS.Infrastructure.Services
{
    public class UserAccessGroupModuleService : BaseService<UMSDbContext, AppUserAccessGroupModule>, IUserAccessGroupModuleService
    {
        public UserAccessGroupModuleService(IBaseRepository<UMSDbContext, AppUserAccessGroupModule> baseRepo) : base(baseRepo)
        {
        }
    }
}
