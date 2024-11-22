using AW.Infrastructure.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Contexts;
using UMS.Core.Entities;

namespace UMS.Infrastructure.Interfaces.Repositories
{
    public interface IUserRepository: IBaseRepository<UMSDbContext, AppUser>
    {
    }
}
