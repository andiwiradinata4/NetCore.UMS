using AW.Core.DTOs;
using AW.Infrastructure.Interfaces.Services;
using AW.Web.Controllers.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UMS.Core.Contexts;
using UMS.Core.DTOs;
using UMS.Core.Entities;
using UMS.Infrastructure.Interfaces.Services;

namespace UMS.Web.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : BaseController<UMSDbContext, AppUser>
    {
        private readonly IUserService _svc;
        public UserController(IBaseService<UMSDbContext, AppUser> baseSvc, IUserService svc) : base(baseSvc) { _svc = svc; }

        [Authorize]
        public override Task<IActionResult> Get()
        {
            return base.Get();
        }

        [Authorize]
        public override IActionResult Create(AppUser obj)
        {
            return base.Create(obj);
        }

        [Authorize]
        public override IActionResult Update([FromRoute] string id, [FromBody] AppUser obj)
        {
            return base.Update(id, obj);
        }

        [Authorize]
        public override IActionResult Delete(string id)
        {
            return base.Delete(id);
        }

        [Authorize]
        public override IActionResult Disable([FromRoute] string id, [FromBody] AppUser obj)
        {
            return base.Disable(id, obj);
        }

        [Authorize]
        public override IActionResult PostPageList(QueryObject query)
        {
            return base.PostPageList(query);
        }

        [Authorize]
        public override IActionResult PostPageListWithDisabledRecord([FromBody] QueryObject query)
        {
            return base.PostPageListWithDisabledRecord(query);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AppUser obj)
        {
            if (!base.ModelState.IsValid)
            {
                return BadRequest(base.ModelState);
            }

            MessageObject<AppUser> messageObject = await _svc.CreateAsync(obj);
            if (messageObject.ProcessingStatus)
            {
                return Ok(messageObject);
            }

            return BadRequest(messageObject);
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            AppUser? data = _svc.GetById(userId ?? "");
            MessageObject<AppUser> messageObject = new MessageObject<AppUser>();
            if (data != null)
            {
                messageObject.Data = data;
                messageObject.Data.PasswordHash = "";
                if (messageObject.ProcessingStatus) return Ok(messageObject);
            }

            return Unauthorized(messageObject);
        }
    }
}
