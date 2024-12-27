﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Principal;
using AW.Core.Contexts;
using AW.Infrastructure.Extensions;
using AW.Infrastructure.Interfaces.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AW.Infrastructure.Services;
using UMS.Core.Contexts;
using UMS.Infrastructure.Services;
using UMS.Infrastructure.Interfaces.Services;
using UMS.Infrastructure.Interfaces.Repositories;
using UMS.Infrastructure.Repositories;
using AW.Infrastructure.Middlewares;

namespace UMS.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                options.DefaultScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option =>
            {
                option.ClaimsIssuer = Configuration[key: "JWT:Issuer"];
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration[key: "JWT:SigningKey"] ?? "")),
                    ClockSkew = new TimeSpan(0, 1, 5),
                };
            }
            );

            // Inject IPrincipal
            services.AddHttpContextAccessor();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            services.AddScoped<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            services.AddMvc(options =>
            {
                //options.Filters.Add(new ApiLoggingInfoFilter(services.BuildServiceProvider().CreateScope().ServiceProvider));
                //options.Filters.Add(new ApiExceptionFilter());
            });

            services.AddHelperServices(Configuration);
            services.AddDbContextModel<UMSDbContext>(Configuration);
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
                //avoid camel case names by default
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UMS API", Version = "v1" });
            });

            //services.AddHubConnection(Configuration);
            RegisterDIServices(services);
            //services.AddIIIIIIIIServices();

            ///// # Get Client Credential
            //services.Configure<ClientCredential>(Configuration.GetSection("ClientCredential"));

            // # Call Http Client
            services.AddHttpClient();
            services.AddTransient<IClientCredentialService, ClientCredentialService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // code to be executed in SIT environment 

            }
            else if (env.IsEnvironment("Debug"))
            {
                // code to be executed in SIT environment 

            }
            else if (env.IsEnvironment("Staging"))
            {
                // code to be executed in QA environment 

            }
            else if (env.IsProduction())
            {
                // code to be executed in Production environment 

            }
            else
            {

            }

            // code to be executed in development environment 
            //app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UMS.Web"));

            //app.UseRewriter(new RewriteOptions().Add(RewriteRouteRule.ReWriteRequests));
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            //app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseCors(x => x.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod().AllowCredentials());

            app.UseMiddleware<JwtMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();


            //app.UseMiddleware<MessageHandlerInterceptor>();
            //app.UseMiddleware<FilterComLoc>();
            //app.UsFilterComLoc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    name: "DefaultApi",
                //    pattern: "api/{controller}/{id}");
            });

            // Reponse a message when solution run in debug. 
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("WebApi Page");
            //});

            AWDbContext.ValidateDatabaseModel = true;
        }

        public void RegisterDIServices(IServiceCollection services)
        {
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: "CustomCors", policy => { policy.WithOrigins("https://localhost:3103/api"); });
            //});
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // #region "Custom Service"
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(ITokenService), typeof(TokenService));
            // #endregion 


            // #region "Custom Repository"
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(ITokenRepository), typeof(TokenRepository));
            // #endregion 
        }

    }
}
