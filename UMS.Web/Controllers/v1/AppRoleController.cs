using AW.Web.Controllers.v1;
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
	}
}
