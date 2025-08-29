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
	[Route("api/v1/app-role")]
	[ApiController]
	public class AppRoleController : BaseController<UMSDbContext, AppRole>
	{
		private readonly IAppRoleService _svc;
		public AppRoleController(IAppRoleService svc) : base(svc) => _svc = svc;

        //[Authorize(Roles = "IT-SUPPORT")]
        [Authorize()]
        public override IActionResult Create(AppRole obj)
        {
            return base.Create(obj);
        }

        [Authorize()]
        public override IActionResult Update([FromRoute] string id, [FromBody] AppRole obj)
        {
            return base.Update(id, obj);
        }

        [Authorize()]
        public override IActionResult Disable([FromRoute] string id, [FromBody] AppRole obj)
        {
            return base.Disable(id, obj);
        }

        [Authorize()]
        public override IActionResult Delete(string id)
        {
            return base.Delete(id);
        }

        [Authorize()]
        public override Task<IActionResult> Get()
        {
            return base.Get();
        }

        [Authorize()]
        public override IActionResult Get(string id, [FromBody] QueryObject query)
        {
            return base.Get(id, query);
        }

        [Authorize]
        public override IActionResult GetColumns()
        {
            return base.GetColumns();
        }

        [Authorize()]
        public override IActionResult PostPageList([FromBody] QueryObject query)
        {
            return base.PostPageList(query);
        }

        [Authorize()]
        public override IActionResult PostPageListWithDisabledRecord([FromBody] QueryObject query)
        {
            return base.PostPageListWithDisabledRecord(query);
        }
    }
}
