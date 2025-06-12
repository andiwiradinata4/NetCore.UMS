using AW.Core.Contexts;
using AW.Core.DTOs;
using AW.Core.DTOs.Interfaces;
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
    public class UserAccessService : BaseService<UMSDbContext, AppUserAccess>, IUserAccessService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAccessGroupRepository _userAccessGroupRepository;
        public UserAccessService(IBaseRepository<UMSDbContext, AppUserAccess> baseRepo, IUserRepository userRepository, IUserAccessGroupRepository userAccessGroupRepository) : base(baseRepo)
        {
            _userRepository = userRepository;
            _userAccessGroupRepository = userAccessGroupRepository;
        }

        private MessageObject<AppUserAccess> ValidateDefault(MessageObject<AppUserAccess> messageObject, AppUserAccess entity)
        {
            if (!_userRepository.ExistsInDb(e => e.Id == entity.UserId)) messageObject.AddMessage(MessageType.Error, "400", $"User Id {entity.UserId} not found.", "UserId");
            if (!_userAccessGroupRepository.ExistsInDb(e => e.Id == entity.UserGroupId)) messageObject.AddMessage(MessageType.Error, "400", $"User Group Id {entity.UserGroupId} not found.", "UserGroupId");
            if (base.ExistsInDb(e => e.UserId == entity.UserId && e.UserGroupId == entity.UserGroupId && e.Id != entity.Id)) messageObject.AddMessage(MessageType.Error, "400", $"Data with User Id {entity.UserId}, User Group Id {entity.UserGroupId} already exists.", "");
            return messageObject;
        }

        protected override MessageObject<AppUserAccess> ValidateCreate(AppUserAccess entity)
        {
            MessageObject<AppUserAccess> messageObject = base.ValidateCreate(entity);
            messageObject = ValidateDefault(messageObject, entity);
            return messageObject;
        }

        protected override MessageObject<AppUserAccess> ValidateUpdate(AppUserAccess entity)
        {
            MessageObject<AppUserAccess> messageObject = base.ValidateUpdate(entity);
            messageObject = ValidateDefault(messageObject, entity);
            return messageObject;
        }
    }
}
