using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Infrastructure.Middlewares
{
    public class AuthenticationRule
    {
        //private readonly RequestDelegate _next;

        //public AuthenticationRule(RequestDelegate next)
        //{
        //    _next = next;
        //}

        //public async Task InvokeAsync(HttpContext context)
        //{
        //    //try
        //    //{
        //    //    var authenticationToken = context.Request.Headers["Authorization"];
        //    //    if (string.IsNullOrEmpty(authenticationToken) == false)
        //    //    {
        //    //        var token = authenticationToken.First()!.Replace("Bearer ", "");
        //    //        JwtSecurityToken jwtSecurity = new JwtSecurityToken(token);
        //    //        Int64 expiredResult = Convert.ToInt64(jwtSecurity.ValidTo.ToString("yyyyMMddHHmmss"));
        //    //        Int64 nowResult = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));

        //    //        if (nowResult > expiredResult) throw new Exception("Token has been expired");
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw ex;
        //    //}
        //    await _next(context);
        //}


    }
}
