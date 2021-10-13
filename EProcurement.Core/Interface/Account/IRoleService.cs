using EProcurement.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Core
{
    public interface IRoleService :  IBaseService<RoleRequest, RoleResponse, RolesResponse>
    {
        public RoleResponse GetRoleByUser(long userId);
        public RoleResponse GetRoleGroupedPriviledgeByRole(long roleId);  

    }
}
