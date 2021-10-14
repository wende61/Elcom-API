using Elcom.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elcom.Core
{
    public interface IRoleService :  IBaseService<RoleRequest, RoleResponse, RolesResponse>
    {
        public RoleResponse GetRoleByUser(long userId);
        public RoleResponse GetRoleGroupedPriviledgeByRole(long roleId);  

    }
}
