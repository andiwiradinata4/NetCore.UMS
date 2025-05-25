using AW.Web.Controllers.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UMS.Core.Contexts;
using UMS.Core.Entities;
using UMS.Infrastructure.Interfaces.Services;

namespace UMS.Web.Controllers.v1
{
    [Route("api/v1/user-access-group")]
    [ApiController]
    public class UserAccessGroupController : BaseController<UMSDbContext, AppUserAccessGroup>
    {
        private readonly IUserAccessGroupService _svc;
        public UserAccessGroupController(IUserAccessGroupService svc) : base(svc) => _svc = svc;
    }
}
