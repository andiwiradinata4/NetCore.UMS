using AW.Core.DTOs;
using AW.Infrastructure.Interfaces.Repositories;
using AW.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Contexts;
using UMS.Core.Entities;
using UMS.Infrastructure.Interfaces.Repositories;
using UMS.Infrastructure.Interfaces.Services;

namespace UMS.Infrastructure.Services
{
    public class UserRoleService : BaseService<UMSDbContext, AppUserRole>, IUserRoleService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAppRoleRepository _appRoleRepository;
        public UserRoleService(IBaseRepository<UMSDbContext, AppUserRole> baseRepo, IUserRepository userRepository, IAppRoleRepository appRoleRepository) : base(baseRepo)
        {
            _userRepository = userRepository;
            _appRoleRepository = appRoleRepository;
        }

        private MessageObject<AppUserRole> ValidateDefault(MessageObject<AppUserRole> messageObject, AppUserRole entity)
        {
            if (!_userRepository.ExistsInDb(e => e.Id == entity.UserId)) messageObject.AddMessage(MessageType.Error, "400", $"User Id {entity.UserId} not found.", "UserId");
            if (!_appRoleRepository.ExistsInDb(e => e.Id == entity.AppRoleId)) messageObject.AddMessage(MessageType.Error, "400", $"App Role Id {entity.AppRoleId} not found.", "AppRoleId");
            if (base.ExistsInDb(e => e.UserId == entity.UserId && e.AppRoleId == entity.AppRoleId && e.Id != entity.Id)) messageObject.AddMessage(MessageType.Error, "400", $"Data with User Id {entity.UserId}, App Role Id {entity.AppRoleId} already exists.", "");
            return messageObject;
        }

        protected override MessageObject<AppUserRole> ValidateCreate(AppUserRole entity)
        {
            MessageObject<AppUserRole> messageObject = base.ValidateCreate(entity);
            messageObject = ValidateDefault(messageObject, entity);
            return messageObject;
        }

        protected override MessageObject<AppUserRole> ValidateUpdate(AppUserRole entity)
        {
            MessageObject<AppUserRole> messageObject = base.ValidateUpdate(entity);
            messageObject = ValidateDefault(messageObject, entity);
            return messageObject;
        }
    }
}
