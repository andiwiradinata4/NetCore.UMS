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
using UMS.Infrastructure.Repositories;

namespace UMS.Infrastructure.Services
{
    public class UserAccessGroupModuleAccessService : BaseService<UMSDbContext, AppUserAccessGroupModuleAccess>, IUserAccessGroupModuleAccessService
    {
        private readonly IAppAccessService _appAccessService;
        private readonly IUserAccessGroupModuleRepository _userAccessGroupModuleRepository;

        public UserAccessGroupModuleAccessService(IBaseRepository<UMSDbContext, AppUserAccessGroupModuleAccess> baseRepo, IAppAccessService appAccessService, IUserAccessGroupModuleRepository userAccessGroupModuleRepository) : base(baseRepo)
        {
            _appAccessService = appAccessService;
            _userAccessGroupModuleRepository = userAccessGroupModuleRepository;
        }

        private MessageObject<AppUserAccessGroupModuleAccess> ValidateDefault(MessageObject<AppUserAccessGroupModuleAccess> messageObject , AppUserAccessGroupModuleAccess entity)
        {
            if (entity.AppAccessId == string.Empty)
            {
                messageObject.AddMessage(MessageType.Error, "400", "App Access Id not allow Empty", "AppAccessId");
            }
            else if (entity.AppAccessId == null)
            {
                messageObject.AddMessage(MessageType.Error, "400", "App Module Id not allow null", "AppAccessId");
            }
            else
            {
                var refData = _appAccessService.GetByIdProxy(entity.AppAccessId, new QueryObject()).Result;
                if (refData == null) messageObject.AddMessage(MessageType.Error, "400", $"App Access Id {entity.AppAccessId} not found", "AppAccessId");
            }

            if (!_userAccessGroupModuleRepository.ExistsInDb(e => e.Id == entity.AppUserAccessGroupModuleId)) messageObject.AddMessage(MessageType.Error, "400", $"App User Access Group Module Id {entity.AppUserAccessGroupModuleId} not found.", "AppUserAccessGroupModuleId ");
            if (base.ExistsInDb(e => e.AppUserAccessGroupModuleId == entity.AppUserAccessGroupModuleId && e.AppAccessId == entity.AppAccessId && e.Id != entity.Id)) messageObject.AddMessage(MessageType.Error, "400", $"Data with App Access Id {entity.AppAccessId}, App User Access Group Module Id {entity.AppUserAccessGroupModuleId} already exists.", "");
            return messageObject;
        }
        protected override MessageObject<AppUserAccessGroupModuleAccess> ValidateCreate(AppUserAccessGroupModuleAccess entity)
        {
            MessageObject<AppUserAccessGroupModuleAccess> messageObject = base.ValidateCreate(entity);
            messageObject = ValidateDefault(messageObject, entity);
            return messageObject;
        }

        protected override MessageObject<AppUserAccessGroupModuleAccess> ValidateUpdate(AppUserAccessGroupModuleAccess entity)
        {
            MessageObject<AppUserAccessGroupModuleAccess> messageObject = base.ValidateUpdate(entity);
            messageObject = ValidateDefault(messageObject, entity);
            return messageObject;
        }

        public override object GetAll(QueryObject query, bool withDisabled)
        {
            string[] Includes = query.Includes.Split(",", StringSplitOptions.TrimEntries);
            query.Includes = string.Join(",", Includes.Except(["AppAccess"]));
            dynamic? data = base.GetAll(query, withDisabled);
            if (data != null)
            {
                var dataObj = JObject.FromObject(data);
                if (Includes.Contains("AppAccess"))
                {
                    MessageGetList<AppAccessDto> listReferences = _appAccessService.GetAllDataProxy(new QueryObject()).Result;
                    if (dataObj != null) AW.Infrastructure.Utils.Helper.EnrichWithReferences(dataObj, (JArray)listReferences.DataSet!, "AppAccessId", "Id", "AppAccess");
                }

                if (dataObj != null) return dataObj;
            }
            return data!;
        }

        public override object? GetByIdWithQueryObject(string Id, QueryObject query)
        {
            string[] Includes = query.Includes.Split(",", StringSplitOptions.TrimEntries);
            query.Includes = string.Join(",", Includes.Except(["AppAccess"]));
            var data = base.GetByIdWithQueryObject(Id, query);
            if (data != null)
            {
                JObject jData = JObject.FromObject(data);
                if (Includes.Contains("AppAccess") && jData["AppAccessId"]?.ToString() != null)
                {
                    var refData = _appAccessService.GetByIdProxy(jData["AppAccessId"]!.ToString(), new QueryObject()).Result;
                    if (refData != null) jData["AppAccess"] = JObject.FromObject(refData);
                }
                return jData;
            }
            return data;
        }
    }
}
