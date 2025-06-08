using AW.Web.Controllers.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UMS.Core.Contexts;
using UMS.Core.Entities;
using UMS.Infrastructure.Interfaces.Services;

namespace UMS.Web.Controllers.v1
{
	[Route("api/v1/user-role")]
	[ApiController]
	public class UserRoleController : BaseController<UMSDbContext, AppUserRole>
	{
		private readonly IUserRoleService _svc;
		public UserRoleController(IUserRoleService svc) : base(svc) => _svc = svc;
	}
}
