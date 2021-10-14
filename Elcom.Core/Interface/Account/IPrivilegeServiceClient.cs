using Elcom.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elcom.Core
{
    public interface IPrivilegeServiceClient : IBaseService<PrivilegeRequest, PrivilegeResponse, PrivilegesResponse>
    {
        public GroupPrivilegesResponse GetGroupPrivilege();
    }
}
