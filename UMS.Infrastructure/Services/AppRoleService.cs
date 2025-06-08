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
	public class AppRoleService : BaseService<UMSDbContext, AppRole>, IAppRoleService
	{
		public AppRoleService(IBaseRepository<UMSDbContext, AppRole> baseRepo) : base(baseRepo)
		{
		}
	}
}
