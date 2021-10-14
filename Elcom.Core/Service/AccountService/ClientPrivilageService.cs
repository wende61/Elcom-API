using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elcom.Common;
using Elcom.DataObjects;
using Microsoft.AspNetCore.Http;

namespace Elcom.Core
{
    public class ClientPrivilegeService : IPrivilegeServiceClient
    {
        private readonly IRepositoryBase<ClientPrivilege> _privilegeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public ClientPrivilegeService(
            IRepositoryBase<ClientPrivilege> privilegeRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _privilegeRepository = privilegeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationStatusResponse> Delete(long Id)
        {
            //if (bulkAction?.Ids == null || bulkAction.Ids.Count < 1)
            //    return new OperationStatusResponse { Message = Resources.PleaseSelectOneRecordToDelete, Status = OperationStatus.ERROR };

            //foreach (var id in bulkAction.Ids)
            //{
            //    var privilege = await _privilegeRepository.FindAsync(id);
            //    if (privilege != null)
            //    {
            //        privilege.RecordStatus = RecordStatus.Deleted;
            //        privilege.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
            //        privilege.TimeZoneInfo = TimeZoneInfo.Local.StandardName;
            //        privilege.LastUpdateDate = DateTime.UtcNow;
            //        _privilegeRepository.Update(privilege);

            //    }
            //}
            return new OperationStatusResponse { Message = string.Format(Resources.OperationSucessfullyCompletedNumberOfRecordAffected), Status = OperationStatus.SUCCESS };
        }

        public PrivilegesResponse GetAll()
        {
            var privilegeListResponse = new PrivilegesResponse();
            var privilegeList = _privilegeRepository.Where(u => u.RecordStatus == RecordStatus.Active ||
                           u.RecordStatus == RecordStatus.Inactive);

            if (privilegeList != null)
            {
                foreach (var privilege in privilegeList)
                {
                    privilegeListResponse.Privileges.Add(new PrivilegeRes
                    {
                        Id = privilege.Id,
                        Action = privilege.Action,
                        Module = privilege.Module,
                        Description = privilege.Description
                    });
                }
            }
            privilegeListResponse.Status = OperationStatus.SUCCESS;
            privilegeListResponse.Message = Resources.OperationSucessfullyCompleted;
            return privilegeListResponse;
        }

        public PrivilegeResponse GetById(long id)
        {
            var privilege = _privilegeRepository.FirstOrDefault(p => p.Id == id);

            if (privilege != null)
            {
                var privilegeResponse = new PrivilegeResponse();

                privilegeResponse.Status = OperationStatus.SUCCESS;
                privilegeResponse.Message = Resources.OperationSucessfullyCompleted;

                privilegeResponse.Privilege = new PrivilegeRes
                {
                    Id = privilege.Id,
                    Action = privilege.Action,
                    Module = privilege.Module,
                    Description = privilege.Description
                };
                return privilegeResponse;
            }
            return new PrivilegeResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
        }

        public GroupPrivilegesResponse GetGroupPrivilege()
        {
            var privilegeGroupsResponse = new GroupPrivilegesResponse();
            List<PrivilegeModule> modules = new List<PrivilegeModule>();
            var privilegeListResponse = new PrivilegesResponse();
            var privilegeList = _privilegeRepository.Where(u => u.RecordStatus == RecordStatus.Active ||
                           u.RecordStatus == RecordStatus.Inactive);

            if (privilegeList != null)
            {
                foreach (var privilege in privilegeList)
                {
                    privilegeListResponse.Privileges.Add(new PrivilegeRes
                    {
                        Id = privilege.Id,
                        Action = privilege.Action,
                        Module = privilege.Module,
                        Description = privilege.Description
                    });
                }
                //group by module
                var groupedPrivelegeList = privilegeListResponse.Privileges.GroupBy(p => p.Module).ToList();
                foreach (var privileges in groupedPrivelegeList)
                {
                    List<PrivilegeController> controllers = new List<PrivilegeController>();
                    PrivilegeModule module = new PrivilegeModule();
                    module.Name = privileges.Key;
                    List<PrivilegeRes> privledgeSpliteds = new List<PrivilegeRes>();
                    foreach (var privilege in privileges)
                    {
                        PrivilegeRes privledgeSplited = new PrivilegeRes();
                        char[] delimiterChars = { '-' };
                        string[] splited = privilege.Action.Split(delimiterChars);
                        privledgeSplited.Controller = splited[0];
                        privledgeSplited.Action = splited[1];
                        privledgeSplited.Id = privilege.Id;
                        privledgeSpliteds.Add(privledgeSplited);
                    }
                    //group by controller
                    var groupedControllerList = privledgeSpliteds.GroupBy(p => p.Controller).ToList();

                    foreach (var groupedController in groupedControllerList)
                    {
                        List<PrivilegeAction> actions = new List<PrivilegeAction>();
                        List<PrivilegeAction> morePermissions = new List<PrivilegeAction>();
                        PrivilegeController controller = new PrivilegeController();
                        controller.Name = groupedController.Key;
                        foreach (var cont in groupedController)
                        {
                            PrivilegeAction action = new PrivilegeAction();
                            action.Name = cont.Action;
                            action.Id = cont.Id;
                            //create,read,update,delete
                            if (action.Name.ToLower().Equals("create") || action.Name.ToLower().Equals("read") || action.Name.ToLower().Equals("update") || action.Name.ToLower().Equals("delete"))
                                actions.Add(action);
                            else
                                morePermissions.Add(action);
                        }
                        controller.Actions = actions;
                        controller.MorePermissions = morePermissions;

                        controllers.Add(controller);
                    }
                    module.Controllers = controllers;
                    modules.Add(module);
                }
            }

            privilegeGroupsResponse.Modules = modules;
            privilegeGroupsResponse.Status = OperationStatus.SUCCESS;
            privilegeGroupsResponse.Message = Resources.OperationSucessfullyCompleted;
            return privilegeGroupsResponse;
        }

        public async Task<PrivilegeResponse> Create(PrivilegeRequest request)
        {
            var prevPrivilege = await _privilegeRepository.FirstOrDefaultAsync(p => p.Action == request.Action);
            if (prevPrivilege != null)
                return new PrivilegeResponse { Message = Resources.RecordAlreadyExist, Status = OperationStatus.ERROR };

            var privilege = new ClientPrivilege
            {
                Action = request.Action,
                Module = request.Module,
                Description = request.Description,
                RecordStatus = RecordStatus.Active,
                StartDate = DateTime.UtcNow,
                TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                LastUpdateDate = DateTime.UtcNow,
                EndDate = DateTime.MaxValue,
                UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername")
            };

            if (_privilegeRepository.Add(privilege))
                return new PrivilegeResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };

            return new PrivilegeResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR };

        }

        public async Task<PrivilegeResponse> Update(PrivilegeRequest request)
        {
            var privilege = await _privilegeRepository.FirstOrDefaultAsync(p => p.Id == request.Id);
            if (privilege == null)
                return new PrivilegeResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

            privilege.Module = request.Module;
            privilege.Description = request.Description;
            privilege.TimeZoneInfo = TimeZoneInfo.Local.StandardName;
            privilege.LastUpdateDate = DateTime.UtcNow;
            privilege.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");


            if (_privilegeRepository.Update(privilege))
                return new PrivilegeResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };

            return new PrivilegeResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR };
        }
    }
}
