using Elcom.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elcom.Core.Interface.Account
{
   public interface IMenusService
    {
         Task<OperationStatusResponse> delete(long id);
         MenusResponse getallmenus();
         MenuResponse getmenubyid(long id);
         Task<MenuResponse> addmenu(MenuRequest request);
         Task<MenuResponse> updatemenu(MenuRequest request);
    }
}
