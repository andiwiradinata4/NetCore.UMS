﻿using AW.Infrastructure.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Contexts;
using UMS.Core.DTOs;
using UMS.Core.Entities;

namespace UMS.Infrastructure.Interfaces.Services
{
    public interface IUserService: IBaseService<UMSDbContext, AppUser>
    {
    }
}