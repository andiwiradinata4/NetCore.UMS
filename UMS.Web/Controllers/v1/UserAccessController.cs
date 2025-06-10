using AW.Core.DTOs;
using AW.Web.Controllers.v1;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "IT-SUPPORT")]
        public override IActionResult Create(AppUserAccess obj)
        {
            return base.Create(obj);
        }

        [Authorize(Roles = "IT-SUPPORT")]
        public override IActionResult Update([FromRoute] string id, [FromBody] AppUserAccess obj)
        {
            return base.Update(id, obj);
        }

        [Authorize(Roles = "IT-SUPPORT")]
        public override IActionResult Disable([FromRoute] string id, [FromBody] AppUserAccess obj)
        {
            return base.Disable(id, obj);
        }

        [Authorize(Roles = "IT-SUPPORT")]
        public override IActionResult Delete(string id)
        {
            return base.Delete(id);
        }

        [Authorize(Roles = "IT-SUPPORT")]
        public override Task<IActionResult> Get()
        {
            return base.Get();
        }

        [Authorize(Roles = "IT-SUPPORT")]
        public override IActionResult Get(string id, [FromBody] QueryObject query)
        {
            return base.Get(id, query);
        }

        [Authorize]
        public override IActionResult GetColumns()
        {
            return base.GetColumns();
        }

        [Authorize(Roles = "IT-SUPPORT")]
        public override IActionResult PostPageList([FromBody] QueryObject query)
        {
            return base.PostPageList(query);
        }

        [Authorize(Roles = "IT-SUPPORT")]
        public override IActionResult PostPageListWithDisabledRecord([FromBody] QueryObject query)
        {
            return base.PostPageListWithDisabledRecord(query);
        }
    }
}
