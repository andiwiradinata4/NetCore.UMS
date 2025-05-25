using AW.Web.Controllers.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UMS.Core.Contexts;
using UMS.Core.Entities;
using UMS.Infrastructure.Interfaces.Services;

namespace UMS.Web.Controllers.v1
{
    [Route("api/v1/user-access")]
    [ApiController]
    public class UserAccessController : BaseController<UMSDbContext, AppUserAccess>
    {
        private readonly IUserAccessService _svc;
        public UserAccessController(IUserAccessService svc) : base(svc) => _svc = svc;
    }
}
