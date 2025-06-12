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
using UMS.Infrastructure.Interfaces.Repositories;
using UMS.Infrastructure.Interfaces.Services;
using UMS.Infrastructure.Interfaces.Services.Proxies;

namespace UMS.Infrastructure.Services
{
    public class UserAccessGroupModuleService : BaseService<UMSDbContext, AppUserAccessGroupModule>, IUserAccessGroupModuleService
    {
        private readonly IAppModuleService _appModuleService;
        private readonly IUserAccessGroupModuleAccessService _userAccessGroupModuleAccessService;
        private readonly IUserAccessGroupRepository _userAccessGroupRepository;

        public UserAccessGroupModuleService(IBaseRepository<UMSDbContext, AppUserAccessGroupModule> baseRepo, IAppModuleService appModuleService, IUserAccessGroupRepository userAccessGroupRepository, IUserAccessGroupModuleAccessService userAccessGroupModuleAccessService) : base(baseRepo)
        {
            _appModuleService = appModuleService;
            _userAccessGroupRepository = userAccessGroupRepository;
            _userAccessGroupModuleAccessService = userAccessGroupModuleAccessService;
        }

        private MessageObject<AppUserAccessGroupModule> ValidateDefault(MessageObject<AppUserAccessGroupModule> messageObject , AppUserAccessGroupModule entity)
        {
            if (entity.AppModuleId == string.Empty)
            {
                messageObject.AddMessage(MessageType.Error, "400", "App Module Id not allow Empty", "AppModuleId");
            }
            else if (entity.AppModuleId == null)
            {
                messageObject.AddMessage(MessageType.Error, "400", "App Module Id not allow null", "AppModuleId");
            }
            else
            {
                var refData = _appModuleService.GetByIdProxy(entity.AppModuleId, new QueryObject()).Result;
                if (refData == null) messageObject.AddMessage(MessageType.Error, "400", $"App Module Id {entity.AppModuleId} not found", "AppModuleId");
            }

            if (!_userAccessGroupRepository.ExistsInDb(e => e.Id == entity.AppUserAccessGroupId)) messageObject.AddMessage(MessageType.Error, "400", $"App User Access Group Id {entity.AppUserAccessGroupId} not found.", "AppUserAccessGroupId");
            if (base.ExistsInDb(e => e.AppModuleId == entity.AppModuleId && e.AppUserAccessGroupId == entity.AppUserAccessGroupId && e.Id != entity.Id)) messageObject.AddMessage(MessageType.Error, "400", $"Data with App Module Id {entity.AppModuleId}, App User Access Group Id {entity.AppUserAccessGroupId} already exists.", "");
            return messageObject;
        }

        protected override MessageObject<AppUserAccessGroupModule> ValidateCreate(AppUserAccessGroupModule entity)
        {
            MessageObject<AppUserAccessGroupModule> messageObject = base.ValidateCreate(entity);
            messageObject = ValidateDefault(messageObject, entity);
            return messageObject;
        }

        protected override MessageObject<AppUserAccessGroupModule> ValidateUpdate(AppUserAccessGroupModule entity)
        {
            MessageObject<AppUserAccessGroupModule> messageObject = base.ValidateUpdate(entity);
            messageObject = ValidateDefault(messageObject, entity);
            return messageObject;
        }

        public override object GetAll(QueryObject query, bool withDisabled)
        {
            string[] Includes = query.Includes.Split(",", StringSplitOptions.TrimEntries);
            query.Includes = string.Join(",", Includes.Except(["AppModule"]));
            dynamic? data = base.GetAll(query, withDisabled);
            if (data != null)
            {
                var dataObj = JObject.FromObject(data);
                if (Includes.Contains("AppModule"))
                {
                    MessageGetList<AppModuleDto> listReferences = _appModuleService.GetAllDataProxy(new QueryObject()).Result;
                    if (dataObj != null) AW.Infrastructure.Utils.Helper.EnrichWithReferences(dataObj, (JArray)listReferences.DataSet!, "AppModuleId", "Id", "AppModule");
                }

                if (dataObj != null) return dataObj;
            }
            return data!;
        }

        public override object? GetByIdWithQueryObject(string Id, QueryObject query)
        {
            string[] Includes = query.Includes.Split(",", StringSplitOptions.TrimEntries);
            query.Includes = string.Join(",", Includes.Except(["AppModuleAccesses","AppModule"]));
            var data = base.GetByIdWithQueryObject(Id, query);
            if (data != null)
            {
                JObject jData = JObject.FromObject(data);
                if (Includes.Contains("AppModule") && jData["AppModuleId"]?.ToString() != null)
                {
                    var refData = _appModuleService.GetByIdProxy(jData["AppModuleId"]!.ToString(), new QueryObject()).Result;
                    if (refData != null) jData["AppModule"] = JObject.FromObject(refData);
                }

                if (Includes.Contains("AppModuleAccesses"))
                {
                    var refData = _userAccessGroupModuleAccessService.GetAll(new QueryObject() { Columns = "Id,AppAccessId", Includes = "AppAccess", FilterParams = [new FilterParams() { Key = "AppUserAccessGroupModuleId", Option = "equals", Value = jData["Id"]!.ToString(), ValueType = "string" }] }, true);
                    if (refData != null)
                    {
                        JObject refObject = JObject.FromObject(refData);
                        jData["AppModuleAccesses"] = refObject["DataSet"];
                    }
                }

                return jData;
            }
            return data;
        }
    }
}
