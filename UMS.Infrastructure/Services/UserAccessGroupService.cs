using AW.Core.DTOs;
using AW.Infrastructure.Interfaces.Repositories;
using AW.Infrastructure.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Contexts;
using UMS.Core.DTOs;
using UMS.Core.Entities;
using UMS.Infrastructure.Interfaces.Services;
using UMS.Infrastructure.Interfaces.Services.Proxies;
using UMS.Infrastructure.Services.Proxies;

namespace UMS.Infrastructure.Services
{
    public class UserAccessGroupService : BaseService<UMSDbContext, AppUserAccessGroup>, IUserAccessGroupService
    {
        private readonly IAppSystemService _appSystemService;
        private readonly IAppProjectService _appProjectService;
        private readonly ICompanyService _companyService;
        private readonly ICompanyLocationService _companyLocationService;

        public UserAccessGroupService(IBaseRepository<UMSDbContext, AppUserAccessGroup> baseRepo, IAppSystemService appSystemService, IAppProjectService appProjectService, ICompanyService companyService, ICompanyLocationService companyLocationService) : base(baseRepo)
        {
            _appSystemService = appSystemService;
            _appProjectService = appProjectService;
            _companyService = companyService;
            _companyLocationService = companyLocationService;
        }

        protected override MessageObject<AppUserAccessGroup> ValidateCreate(AppUserAccessGroup entity)
        {
            MessageObject<AppUserAccessGroup> messageObject = base.ValidateCreate(entity);
            if (entity.AppSystemId == string.Empty)
            {
                messageObject.AddMessage(MessageType.Error, "400", "App System Id not allow Empty", "AppSystemId");
            }
            else if (entity.AppSystemId == null)
            {
                messageObject.AddMessage(MessageType.Error, "400", "App System Id not allow null", "AppSystemId");
            }
            else
            {
                var refData = _appSystemService.GetByIdProxy(entity.AppSystemId, new QueryObject()).Result;
                if (refData == null) messageObject.AddMessage(MessageType.Error, "400", $"App System Id {entity.AppSystemId} not found", "AppSystemId");
            }

            if (entity.AppProjectId == string.Empty)
            {
                messageObject.AddMessage(MessageType.Error, "400", "App Project Id not allow Empty", "AppProjectId");
            }
            else if (entity.AppProjectId == null)
            {
                messageObject.AddMessage(MessageType.Error, "400", "App Project Id not allow null", "AppProjectId");
            }
            else
            {
                var refData = _appProjectService.GetByIdProxy(entity.AppProjectId, new QueryObject()).Result;
                if (refData == null) messageObject.AddMessage(MessageType.Error, "400", $"App Project Id {entity.AppProjectId} not found", "AppProjectId");
            }

            if (entity.CompanyId == string.Empty)
            {
                messageObject.AddMessage(MessageType.Error, "400", "Company Id not allow Empty", "CompanyId");
            }
            else if (entity.CompanyId == null)
            {
                messageObject.AddMessage(MessageType.Error, "400", "Company Id not allow null", "CompanyId");
            }
            else
            {
                var refData = _companyService.GetByIdProxy(entity.CompanyId, new QueryObject()).Result;
                if (refData == null) messageObject.AddMessage(MessageType.Error, "400", $"Company Id {entity.CompanyId} not found", "CompanyId");
            }

            if (entity.CompanyLocationId == string.Empty)
            {
                messageObject.AddMessage(MessageType.Error, "400", "Company Location Id not allow Empty", "CompanyLocationId");
            }
            else if (entity.CompanyLocationId == null)
            {
                messageObject.AddMessage(MessageType.Error, "400", "Company Location Id not allow null", "CompanyLocationId");
            }
            else
            {
                var refData = _companyService.GetByIdProxy(entity.CompanyLocationId, new QueryObject()).Result;
                if (refData == null) messageObject.AddMessage(MessageType.Error, "400", $"Company Location Id {entity.CompanyLocationId} not found", "CompanyLocationId");
            }
            return messageObject;
        }

        protected override MessageObject<AppUserAccessGroup> ValidateUpdate(AppUserAccessGroup entity)
        {
            MessageObject<AppUserAccessGroup> messageObject = base.ValidateUpdate(entity);
            if (entity.AppSystemId == string.Empty)
            {
                messageObject.AddMessage(MessageType.Error, "400", "App System Id not allow Empty", "AppSystemId");
            }
            else if (entity.AppSystemId == null)
            {
                messageObject.AddMessage(MessageType.Error, "400", "App System Id not allow null", "AppSystemId");
            }
            else
            {
                var refData = _appSystemService.GetByIdProxy(entity.AppSystemId, new QueryObject()).Result;
                if (refData == null) messageObject.AddMessage(MessageType.Error, "400", $"App System Id {entity.AppSystemId} not found", "AppSystemId");
            }

            if (entity.AppProjectId == string.Empty)
            {
                messageObject.AddMessage(MessageType.Error, "400", "App Project Id not allow Empty", "AppProjectId");
            }
            else if (entity.AppProjectId == null)
            {
                messageObject.AddMessage(MessageType.Error, "400", "App Project Id not allow null", "AppProjectId");
            }
            else
            {
                var refData = _appProjectService.GetByIdProxy(entity.AppProjectId, new QueryObject()).Result;
                if (refData == null) messageObject.AddMessage(MessageType.Error, "400", $"App Project Id {entity.AppProjectId} not found", "AppProjectId");
            }

            if (entity.CompanyId == string.Empty)
            {
                messageObject.AddMessage(MessageType.Error, "400", "Company Id not allow Empty", "CompanyId");
            }
            else if (entity.CompanyId == null)
            {
                messageObject.AddMessage(MessageType.Error, "400", "Company Id not allow null", "CompanyId");
            }
            else
            {
                var refData = _companyService.GetByIdProxy(entity.CompanyId, new QueryObject()).Result;
                if (refData == null) messageObject.AddMessage(MessageType.Error, "400", $"Company Id {entity.CompanyId} not found", "CompanyId");
            }

            if (entity.CompanyLocationId == string.Empty)
            {
                messageObject.AddMessage(MessageType.Error, "400", "Company Location Id not allow Empty", "CompanyLocationId");
            }
            else if (entity.CompanyLocationId == null)
            {
                messageObject.AddMessage(MessageType.Error, "400", "Company Location Id not allow null", "CompanyLocationId");
            }
            else
            {
                var refData = _companyService.GetByIdProxy(entity.CompanyLocationId, new QueryObject()).Result;
                if (refData == null) messageObject.AddMessage(MessageType.Error, "400", $"Company Location Id {entity.CompanyLocationId} not found", "CompanyLocationId");
            }
            return messageObject;
        }

        public override object GetAll(QueryObject query, bool withDisabled)
        {
            string Includes = query.Includes;
            if (query.Includes.Contains("AppProject")) query.Includes = query.Includes.Replace("AppProject", "");
            if (query.Includes.Contains("AppSystem")) query.Includes = query.Includes.Replace("AppSystem", "");
            if (query.Includes.Contains("CompanyLocation")) query.Includes = query.Includes.Replace("CompanyLocation", "");
            if (query.Includes.Contains("Company")) query.Includes = query.Includes.Replace("Company", "");

            dynamic? data = base.GetAll(query, withDisabled);
            if (data != null)
            {
                var dataObj = JObject.FromObject(data);
                if (Includes.Contains("AppSystem"))
                {
                    MessageGetList<AppSystemDto> listReferences = _appSystemService.GetAllDataProxy(new QueryObject()).Result;
                    if (dataObj != null) AW.Infrastructure.Utils.Helper.EnrichWithReferences(dataObj, (JArray)listReferences.DataSet!, "AppSystemId", "Id", "AppSystem");
                }

                if (Includes.Contains("AppProject"))
                {
                    MessageGetList<AppProjectDto> listReferences = _appProjectService.GetAllDataProxy(new QueryObject()).Result;
                    if (dataObj != null) AW.Infrastructure.Utils.Helper.EnrichWithReferences(dataObj, (JArray)listReferences.DataSet!, "AppProjectId", "Id", "AppProject");
                }

                if (Includes.Contains("Company"))
                {
                    MessageGetList<CompanyDto> listReferences = _companyService.GetAllDataProxy(new QueryObject()).Result;
                    if (dataObj != null) AW.Infrastructure.Utils.Helper.EnrichWithReferences(dataObj, (JArray)listReferences.DataSet!, "CompanyId", "Id", "Company");
                }

                if (Includes.Contains("CompanyLocation"))
                {
                    MessageGetList<CompanyLocationDto> listReferences = _companyLocationService.GetAllDataProxy(new QueryObject()).Result;
                    if (dataObj != null) AW.Infrastructure.Utils.Helper.EnrichWithReferences(dataObj, (JArray)listReferences.DataSet!, "CompanyLocationId", "Id", "CompanyLocation");
                }

                if (dataObj != null) return dataObj;
            }
            return data!;
        }

        public override object? GetByIdWithQueryObject(string Id, QueryObject query)
        {
            string Includes = query.Includes;
            if (query.Includes.Contains("AppProject")) query.Includes = query.Includes.Replace("AppProject", "");
            if (query.Includes.Contains("AppSystem")) query.Includes = query.Includes.Replace("AppSystem", "");
            if (query.Includes.Contains("CompanyLocation")) query.Includes = query.Includes.Replace("CompanyLocation", "");
            if (query.Includes.Contains("Company")) query.Includes = query.Includes.Replace("Company", "");

            var data = base.GetByIdWithQueryObject(Id, query);
            if (data != null)
            {
                JObject jData = JObject.FromObject(data);
                if (Includes.Contains("AppSystem") && jData["AppSystemId"]?.ToString() != null)
                {
                    var refData = _appSystemService.GetByIdProxy(jData["AppSystemId"]!.ToString(), new QueryObject()).Result;
                    if (refData != null) jData["AppSystem"] = JObject.FromObject(refData);
                }

                if (Includes.Contains("AppProject") && jData["AppProjectId"]?.ToString() != null)
                {
                    var refData = _appProjectService.GetByIdProxy(jData["AppProjectId"]!.ToString(), new QueryObject()).Result;
                    if (refData != null) jData["AppProject"] = JObject.FromObject(refData);
                }

                if (Includes.Contains("Company") && jData["CompanyId"]?.ToString() != null)
                {
                    var refData = _companyService.GetByIdProxy(jData["CompanyId"]!.ToString(), new QueryObject()).Result;
                    if (refData != null) jData["Company"] = JObject.FromObject(refData);
                }

                if (Includes.Contains("CompanyLocation") && jData["CompanyLocationId"]?.ToString() != null)
                {
                    var refData = _companyLocationService.GetByIdProxy(jData["CompanyLocationId"]!.ToString(), new QueryObject()).Result;
                    if (refData != null) jData["CompanyLocation"] = JObject.FromObject(refData);
                }

                return jData;
            }
            return data;
        }
    }
}
