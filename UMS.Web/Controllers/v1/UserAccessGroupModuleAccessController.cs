using AW.Web.Controllers.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UMS.Core.Contexts;
using UMS.Core.Entities;
using UMS.Infrastructure.Interfaces.Services;

namespace UMS.Web.Controllers.v1
{
    [Route("api/v1/user-access-group-module-access")]
    [ApiController]
    public class UserAccessGroupModuleAccessController : BaseController<UMSDbContext, AppUserAccessGroupModuleAccess>
    {
        private readonly IUserAccessGroupModuleAccessService _svc;
        public UserAccessGroupModuleAccessController(IUserAccessGroupModuleAccessService svc) : base(svc) => _svc = svc;
    }
}
