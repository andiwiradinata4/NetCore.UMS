﻿using AW.Core.DTOs;
using AW.Core.DTOs.Interfaces;
using AW.Infrastructure.Interfaces.Repositories;
using AW.Infrastructure.Services;
using AW.Infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Contexts;
using UMS.Core.DTOs;
using UMS.Core.Entities;
using UMS.Infrastructure.Interfaces.Services;

namespace UMS.Infrastructure.Services
{
    public class UserService : BaseService<UMSDbContext, AppUser>, IUserService
    {
        public UserService(IBaseRepository<UMSDbContext, AppUser> repo) : base(repo)
        {
        }

        protected override MessageObject<AppUser> ValidateCreate(AppUser entity)
        {
            MessageObject<AppUser> message = new MessageObject<AppUser>(entity);
            if (string.IsNullOrEmpty(entity.PasswordHash))
            {
                message.AddMessage(new Message(MessageType.Error, "400", "Password not allow blank", "PasswordHash"));
            }
            else if (string.IsNullOrEmpty(entity.Name))
            {
                message.AddMessage(new Message(MessageType.Error, "400", "Name not allow blank", "Name"));
            }
            else if (string.IsNullOrEmpty(entity.Email))
            {
                message.AddMessage(new Message(MessageType.Error, "400", "Email not allow blank", "Email"));
            }
            else if (base.ExistsInDbWithDisabledRecord(e => e.Username == entity.Username))
            {
                message.AddMessage(new Message(MessageType.Error, "400", "Username already exists", "Username"));
            }
            else if (base.ExistsInDbWithDisabledRecord(e => e.Email == entity.Email))
            {
                message.AddMessage(new Message(MessageType.Error, "400", "Email already exists", "Email"));
            }
            return message;
        }

        protected override MessageObject<AppUser> ValidateUpdate(AppUser entity)
        {
            MessageObject<AppUser> message = new MessageObject<AppUser>(entity);
            if (base.ExistsInDbWithDisabledRecord(e => e.Email == entity.Email && e.Id != entity.Id))
            {
                message.AddMessage(new Message(MessageType.Error, "400", "Email already exists", "Email"));
            }
            return base.ValidateUpdate(entity);
        }

        protected override AppUser BeforeCreate(AppUser entity)
        {
            entity.PasswordHash = PasswordHasher.HashPassword(entity.PasswordHash);
            return base.BeforeCreate(entity);
        }

        public override AppUser GetNewID(AppUser entity)
        {
            entity.Id = "S" + DateTime.Now.ToString("yyMMdd");
            var max = base.repo.CountByCondition(e => e.Id.Contains(entity.Id)) + 1;
            entity.Id += max.ToString("000");
            return entity;
        }

        public override ICollection<AppUser> GetAll()
        {
            ICollection<AppUser> result = base.GetAll();
            foreach (var item in result)
            {
                item.PasswordHash = "";
            }
            return result;
        }

        public override async Task<List<AppUser>> GetAllAsync()
        {
            var result = await base.GetAllAsync();
            foreach (var item in result)
            {
                item.PasswordHash = "";
            }
            return result;
        }

        public override Task<AppUser?> GetByIDAsync(string Id)
        {
            AppUser result = base.GetById(Id) ?? new AppUser();
            if (result != null) result.PasswordHash = "";
            return Task.FromResult<AppUser?>(result);
        }

        public override AppUser? GetById(string Id)
        {
            AppUser? result = base.GetById(Id);
            if (result != null) result.PasswordHash = "";
            return result;
        }

        protected override AppUser BeforeUpdate(AppUser entity)
        {
            AppUser? user = repo.GetById(entity.Id);
            if (user != null)
            {
                entity.PasswordHash = user.PasswordHash;
            }

            return base.BeforeUpdate(entity);
        }

    }
}
