using AW.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Contexts;
using UMS.Core.Entities;
using UMS.Infrastructure.Interfaces.Repositories;

namespace UMS.Infrastructure.Repositories
{
    public class TokenRepository : BaseRepository<UMSDbContext, AppToken>, ITokenRepository
    {
        public TokenRepository(UMSDbContext context, IPrincipal pctx) : base(context, pctx)
        {
        }
    }
}