using EProcurement.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Core
{
    public interface IPrivilegeServiceClient : IBaseService<PrivilegeRequest, PrivilegeResponse, PrivilegesResponse>
    {
        public GroupPrivilegesResponse GetGroupPrivilege();
    }
}
