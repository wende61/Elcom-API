﻿using EProcurement.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Core
{
    public interface IPrivilegeService : IBaseService<PrivilegeRequest, PrivilegeResponse, PrivilegesResponse>
    {
        public GroupPrivilegesResponse GetGroupPrivilege();
        public GroupPrivilegesResponse GetGroupPrivilegePerUser(long userId);
        public GroupPrivilegesResponse GetGroupPrivilegePerRole(long roleId);
    }
}
