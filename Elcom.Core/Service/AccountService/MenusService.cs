using Elcom.DataObjects.Models.MasterData;
using Elcom.Common;
using Elcom.Core.Interface.Account;
using Elcom.DataObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
namespace Elcom.core.service.accountservice
{
    public class menusservice : IMenusService
    {
        private readonly IRepositoryBase<Menus> _menurepository;
        private readonly IHttpContextAccessor _httpcontextaccessor;
        public menusservice(IRepositoryBase<Menus> menurepository, IHttpContextAccessor httpcontextaccessor)
        {
            _menurepository = menurepository;
            _httpcontextaccessor = httpcontextaccessor;
        }
        public async Task<OperationStatusResponse> delete(long id)
        {
            var menu = _menurepository.FirstOrDefault(p => p.Id == id);
            if (menu == null)
                return new OperationStatusResponse { Message = string.Format(Resources.RecordDoesNotExist), Status = OperationStatus.ERROR };
            menu.RecordStatus = RecordStatus.Deleted;
            menu.LastUpdateDate = DateTime.UtcNow;
            menu.UpdatedBy = _httpcontextaccessor.HttpContext.Session.GetString("currentusername");
            if (!_menurepository.Update(menu))
                return new OperationStatusResponse { Message = string.Format(Resources.RecordDoesNotExist), Status = OperationStatus.ERROR };
            return new OperationStatusResponse { Message = string.Format(Resources.OperationSucessfullyCompletedNumberOfRecordAffected), Status = OperationStatus.SUCCESS };
        }
        public MenusResponse getallmenus()
        {
            var menulistresponse = new MenusResponse();
            var menulist = _menurepository.Where(u => u.RecordStatus == RecordStatus.Active ||
                           u.RecordStatus == RecordStatus.Inactive);

            if (menulist != null)
            {
                foreach (var menu in menulist)
                {
                    menulistresponse.Menus.Add(new MenuRes
                    {
                        Id = menu.Id,
                        Icon = menu.Icon,
                        Name = menu.Name,
                        ParentId = menu.ParentId,
                        Privilages = menu.Privilages,
                        Url = menu.Url
                    });
                }
            }
            menulistresponse.Status = OperationStatus.SUCCESS;
            menulistresponse.Message = Resources.OperationSucessfullyCompleted;
            return menulistresponse;
        }
        public MenuResponse getmenubyid(long id)
        {
            var menu = _menurepository.FirstOrDefault(p => p.Id == id);

            if (menu != null)
            {
                var menuresponse = new MenuResponse();

                menuresponse.Status = OperationStatus.SUCCESS;
                menuresponse.Message = Resources.OperationSucessfullyCompleted;
                menuresponse.Menu = new MenuRes
                {
                    Id = menu.Id,
                    Icon = menu.Icon,
                    Name = menu.Name,
                    ParentId = menu.ParentId,
                    Privilages = menu.Privilages,
                    Url = menu.Url
                };
                return menuresponse;
            }
            return new MenuResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
        }
        public async Task<MenuResponse> addmenu(MenuRequest request)
        {
            var prevprivilege = await _menurepository.FirstOrDefaultAsync(p => p.Name == request.Name);
            if (prevprivilege != null)
                return new MenuResponse { Message = Resources.RecordAlreadyExist, Status = OperationStatus.ERROR };

            var menu = new Menus
            {
                Icon = request.Icon,
                Name = request.Name,
                ParentId = request.ParentId,
                Privilages = request.Privilages,
                Url = request.Url,
                StartDate = DateTime.UtcNow,
                RecordStatus = RecordStatus.Active
            };

            if (_menurepository.Add(menu))
                return new MenuResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
            return new MenuResponse
            {
                Message = Resources.OperationEndWithError,
                Status = OperationStatus.ERROR
            };
        }
        public async Task<MenuResponse> updatemenu(MenuRequest request)
        {
            var menu = await _menurepository.FirstOrDefaultAsync(p => p.Id == request.Id);
            if (menu == null)
                return new MenuResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
            menu.Icon = request.Icon;
            menu.Name = request.Name;
            menu.ParentId = request.ParentId;
            menu.Privilages = request.Privilages;
            menu.Url = request.Url;
            if (_menurepository.Update(menu))
                return new MenuResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };

            return new MenuResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR };
        }
    }
}
