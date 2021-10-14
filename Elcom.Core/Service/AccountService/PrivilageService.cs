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
    public class PrivilegeService : IPrivilegeService
    {
        private readonly IRepositoryBase<Privilege> _privilegeRepository;
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<RolePrivilege> _rolePrivilegeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PrivilegeService(
            IRepositoryBase<Privilege> privilegeRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepositoryBase<User> userRepository,
            IRepositoryBase<RolePrivilege> rolePrivilegeRepository)
        {
            _privilegeRepository = privilegeRepository;
            _httpContextAccessor = httpContextAccessor;
            _rolePrivilegeRepository = rolePrivilegeRepository;
            _userRepository = userRepository;
        }
        public async Task<OperationStatusResponse> Delete(long id)
        {
            var privilege = await _privilegeRepository.FirstOrDefaultAsync(u => u.Id == id);
            if (privilege == null)
                return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
            else
            {
                privilege.RecordStatus = RecordStatus.Deleted;
                if (_privilegeRepository.Update(privilege))
                    return new OperationStatusResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };

                return new OperationStatusResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };

            }
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

            var privilegeList = _privilegeRepository.Where(u => u.RecordStatus == RecordStatus.Active && u.ClientUserId == Convert.ToInt64(_httpContextAccessor.HttpContext.Session.GetString("ClientUserId")));

            if (privilegeList != null)
            {
                foreach (var privilege in privilegeList)
                {
                    privilegeListResponse.Privileges.Add(new PrivilegeRes
                    {
                        Id = privilege.Id,
                        Action = privilege.Action,
                        Module = privilege.Module,
                        Description = privilege.Description,
                        IsMorePermission = privilege.IsMorePermission
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
                        privledgeSplited.IsMorePermission = privilege.IsMorePermission;
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

                            if (cont.IsMorePermission)
                                morePermissions.Add(action);
                            else
                                actions.Add(action);

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

        public GroupPrivilegesResponse GetGroupPrivilegePerUser(long userId)
        {
            if (userId <= 0)
                return new GroupPrivilegesResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };

            var privilegeGroupsResponse = new GroupPrivilegesResponse();
            List<PrivilegeModule> modules = new List<PrivilegeModule>();
            var privilegeListResponse = new PrivilegesResponse();

            //get the user roles
            //get all priveledges of the user roles(many)
            //filter unique priveledges from all the roles of the user
            var user = _userRepository.Where(ur => ur.Id == userId && ur.RecordStatus == RecordStatus.Active).FirstOrDefault();

            if (user == null)
                return new GroupPrivilegesResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };

            List<long> priId = new List<long>();

            var priList = _rolePrivilegeRepository.Where(rp => rp.RoleId == user.RoleId && rp.RecordStatus == RecordStatus.Active).ToList();
            foreach (var pri in priList)
            {
                priId.Add(pri.PrivilegeId);
            }

            var privilegeList = _privilegeRepository.Where(u => u.RecordStatus == RecordStatus.Active && priId.Contains(u.Id)).ToList();

            if (privilegeList != null)
            {
                foreach (var privilege in privilegeList)
                {
                    privilegeListResponse.Privileges.Add(new PrivilegeRes
                    {
                        Id = privilege.Id,
                        Action = privilege.Action,
                        Module = privilege.Module,
                        Description = privilege.Description,
                        IsMorePermission = privilege.IsMorePermission
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
                        privledgeSplited.IsMorePermission = privilege.IsMorePermission;
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

                            if (cont.IsMorePermission)
                                morePermissions.Add(action);
                            else
                                actions.Add(action);

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

        public GroupPrivilegesResponse GetGroupPrivilegePerRole(long roleId)
        {
            if (roleId <= 0)
                return new GroupPrivilegesResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };

            var privilegeGroupsResponse = new GroupPrivilegesResponse();
            List<PrivilegeModule> modules = new List<PrivilegeModule>();
            var privilegeListResponse = new PrivilegesResponse();

            //get all priveledges of the user roles(many)
            //filter unique priveledges from all the roles of the user


            List<long> priId = new List<long>();

            var priList = _rolePrivilegeRepository.Where(rp => rp.RoleId == roleId && rp.RecordStatus == RecordStatus.Active).ToList();

            if (priList.Count() == 0)
                return new GroupPrivilegesResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };

            foreach (var pri in priList)
            {
                priId.Add(pri.PrivilegeId);
            }

            List<long> priIdDupRemoved = priId.Distinct().ToList();

            var privilegeList = _privilegeRepository.Where(u => u.RecordStatus == RecordStatus.Active && priIdDupRemoved.Contains(u.Id)).ToList();

            if (privilegeList != null)
            {
                foreach (var privilege in privilegeList)
                {
                    privilegeListResponse.Privileges.Add(new PrivilegeRes
                    {
                        Id = privilege.Id,
                        Action = privilege.Action,
                        Module = privilege.Module,
                        Description = privilege.Description,
                        IsMorePermission = privilege.IsMorePermission
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
                        privledgeSplited.IsMorePermission = privilege.IsMorePermission;
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

                            if (cont.IsMorePermission)
                                morePermissions.Add(action);
                            else
                                actions.Add(action);

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

            var privilege = new Privilege
            {
                Action = request.Action,
                Module = request.Module,
                Description = request.Description,
                ClientUserId = 1,// Convert.ToInt64(_httpContextAccessor.HttpContext.Session.GetString("ClientUserId")),
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
            privilege.Action = request.Action;
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
