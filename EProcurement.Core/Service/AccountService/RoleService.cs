using EProcurement.Common;
using EProcurement.DataObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EProcurement.Core
{
    public class RoleService : IRoleService
    {
        private readonly IRepositoryBase<Role> _roleRepository;
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<RolePrivilege> _rolePrivilageRepository;
        private readonly IServiceUtility _serviceUtility;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbTransactionContext _appTransaction;
        private readonly IPrivilegeService _privilegeService;
        public RoleService(
            IRepositoryBase<Role> roleRepository,
            IRepositoryBase<RolePrivilege> rolePrivilageRepository,
             IServiceUtility serviceUtility, IHttpContextAccessor httpContextAccessor, IRepositoryBase<User> userRepository,
            IAppDbTransactionContext appTransaction, IPrivilegeService privilegeService)
        {
            _roleRepository = roleRepository;
            _rolePrivilageRepository = rolePrivilageRepository;
            _serviceUtility = serviceUtility;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _appTransaction = appTransaction;
            _privilegeService = privilegeService;

        }
        public RolesResponse GetAll()
        {
            var roleResponseList = new RolesResponse();
            var dd = _httpContextAccessor.HttpContext.Session.GetString("AccountSubscriptionId");
            var roleList = _roleRepository.Where(u => u.RecordStatus == RecordStatus.Active
            /*&& u.AccountSubscriptionId == Convert.ToInt64(_httpContextAccessor.HttpContext.Session.GetString("AccountSubscriptionId"))*/).ToList();
            if (roleList.Count() > 0)
            {
                foreach (var role in roleList)
                {
                    var rolePrivilageList = _rolePrivilageRepository.Where(rp => rp.RoleId == role.Id, new string[] { nameof(Privilege) });
                    var roleResponse = new RoleRes
                    {
                        Id = role.Id,
                        Description = role.Description,
                        Name = role.Name,
                        IsReadOnly = role.IsReadOnly,
                        RecordStatus = role.RecordStatus,
                        Privileges = new List<PrivilegeRes>()
                    };
                    if (rolePrivilageList != null)
                        foreach (var rolePrivilage in rolePrivilageList)
                            roleResponse.Privileges.Add(new PrivilegeRes { Id = rolePrivilage.PrivilegeId, Action = rolePrivilage.Privilege.Action, Module = rolePrivilage.Privilege.Module, IsMorePermission = rolePrivilage.Privilege.IsMorePermission });
                    roleResponseList.Roles.Add(roleResponse);
                }
            }
            roleResponseList.Status = OperationStatus.SUCCESS;
            roleResponseList.Message = Resources.OperationSucessfullyCompleted;
            return roleResponseList;
        }
        public RoleResponse GetById(long id)
        {
            var role = _roleRepository.FirstOrDefault(r => r.Id == id && r.RecordStatus == RecordStatus.Active
            /*&& r.AccountSubscriptionId == Convert.ToInt64(_httpContextAccessor.HttpContext.Session.GetString("AccountSubscriptionId"))*/);
            if (role != null)
            {
                var roleResponse = new RoleResponse();
                roleResponse.Status = OperationStatus.SUCCESS;
                roleResponse.Message = Resources.OperationSucessfullyCompleted;
                roleResponse.Role = new RoleRes
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    RecordStatus = role.RecordStatus,
                    IsReadOnly = role.IsReadOnly,
                    Privileges = new List<PrivilegeRes>()
                };
                var rolePrivilageList = _rolePrivilageRepository.Where(rp => rp.RoleId == role.Id, new string[] { nameof(Privilege) });
                if (rolePrivilageList != null)
                    foreach (var rolePrivilage in rolePrivilageList)
                        roleResponse.Role.Privileges.Add(new PrivilegeRes { Id = rolePrivilage.PrivilegeId, Action = rolePrivilage.Privilege.Action, Module = rolePrivilage.Privilege.Module, IsMorePermission = rolePrivilage.Privilege.IsMorePermission });
                return roleResponse;
            }
            return new RoleResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
        }
        public async Task<RoleResponse> Create(RoleRequest request)
        {
            if (request.Privileges == null || (request.Privileges.Count() == 1 && request.Privileges[0] == 0) || request.Privileges.Count() == 0)
                return new RoleResponse { Message = Resources.AtleastSelectOnePrivilage, Status = OperationStatus.ERROR };
            var prevRole = await _roleRepository.FirstOrDefaultAsync(r => r.Name == request.Name/* && r.AccountSubscriptionId == Convert.ToInt64(_httpContextAccessor.HttpContext.Session.GetString("AccountSubscriptionId"))*/);
            if (prevRole == null)
            {
                using (var uow = new AppUnitOfWork(_appTransaction.GetDbContext()))
                {
                    RepositoryBaseWork<Role> roleRespository = new RepositoryBaseWork<Role>(uow);
                    RepositoryBaseWork<RolePrivilege> rolePrivilegeRespository = new RepositoryBaseWork<RolePrivilege>(uow);

                    using (var transaction = uow.BeginTrainsaction())
                    {
                        try
                        {

                            var role = new Role
                            {
                                Name = request.Name,
                                Description = request.Description,
                                StartDate = DateTime.UtcNow,
                                EndDate = DateTime.MaxValue,
                                //AccountSubscriptionId = Convert.ToInt64(_httpContextAccessor.HttpContext.Session.GetString("AccountSubscriptionId")),
                                RecordStatus = RecordStatus.Active,
                                UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername"),
                                TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                                LastUpdateDate = DateTime.UtcNow,
                            };
                            roleRespository.Add(role);
                            await uow.SaveChangesAsync();

                            foreach (var previlageId in request.Privileges)
                            {
                                if (previlageId > 0)
                                {
                                    var rolePrivilage = new RolePrivilege
                                    {
                                        RoleId = role.Id,
                                        PrivilegeId = previlageId,
                                        StartDate = DateTime.UtcNow,
                                        EndDate = DateTime.MaxValue,
                                        RecordStatus = RecordStatus.Active,
                                        UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername"),
                                        TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                                        LastUpdateDate = DateTime.UtcNow,
                                    };
                                    rolePrivilegeRespository.Add(rolePrivilage);
                                }

                            }
                            await uow.SaveChangesAsync();
                            transaction.Commit();
                            return new RoleResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return new RoleResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR, BusinessErrorCode = ex.Message };
                        }

                    }
                }
            }
            else
                return new RoleResponse { Message = Resources.RecordAlreadyExist, Status = OperationStatus.ERROR };
        }
        public async Task<RoleResponse> Update(RoleRequest request)
        {
            var prevRole = await _roleRepository.FirstOrDefaultAsync(r => r.Id == request.Id);

            if (prevRole != null)
            {
                if (prevRole.IsReadOnly == false)
                {
                    if (request.Privileges != null && request.Privileges.Count > 0)
                    {
                        using (var uow = new AppUnitOfWork(_appTransaction.GetDbContext()))
                        {
                            RepositoryBaseWork<Role> roleRespository = new RepositoryBaseWork<Role>(uow);
                            RepositoryBaseWork<RolePrivilege> rolePrivilegeRespository = new RepositoryBaseWork<RolePrivilege>(uow);
                            using (var transaction = uow.BeginTrainsaction())
                            {
                                try
                                {
                                    prevRole.Description = request.Description;
                                    prevRole.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                    prevRole.TimeZoneInfo = TimeZoneInfo.Local.StandardName;
                                    //if (request.roleOrgWarehouseRelationRequest != null)
                                    //    prevRole.AccessToAllOrganization = request.roleOrgWarehouseRelationRequest.AccessToAllOrganization;

                                    prevRole.LastUpdateDate = DateTime.UtcNow;

                                    roleRespository.Update(prevRole);
                                    await uow.SaveChangesAsync();

                                    var prevRolePrevilageList = rolePrivilegeRespository.Where(rp => rp.RoleId == request.Id).ToList();

                                    foreach (var previlageId in request.Privileges)
                                    {
                                        if (prevRolePrevilageList.Count > 0)
                                        {
                                            var preRolePrevilage = prevRolePrevilageList.Where(rp => rp.RoleId == prevRole.Id && rp.PrivilegeId == previlageId).FirstOrDefault();
                                            if (preRolePrevilage != null)
                                            {
                                                //remove from the list(not to be deleted later)  -- which means this priveldge was associated with the role
                                                prevRolePrevilageList.Remove(preRolePrevilage);
                                                continue;
                                            }
                                        }
                                        var rolePrivilage = new RolePrivilege
                                        {
                                            RoleId = request.Id,
                                            PrivilegeId = previlageId,
                                            StartDate = DateTime.UtcNow,
                                            EndDate = DateTime.MaxValue,
                                            RecordStatus = RecordStatus.Active,
                                            UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername"),
                                            TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                                            LastUpdateDate = DateTime.UtcNow,
                                        };

                                        rolePrivilegeRespository.Add(rolePrivilage);
                                        await uow.SaveChangesAsync();
                                    }

                                    //remove role-privliage which do not belong to the new list
                                    if (prevRolePrevilageList.Count() > 0)
                                    {
                                        foreach (var prevRolePrevilage in prevRolePrevilageList)
                                        {
                                            rolePrivilegeRespository.Remove(prevRolePrevilage);
                                        }
                                        await uow.SaveChangesAsync();
                                    }
                                    transaction.Commit();
                                    return new RoleResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    return new RoleResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR, BusinessErrorCode = ex.Message };
                                }
                            }
                        }
                    }
                    else
                        return new RoleResponse { Message = Resources.AtleastSelectOnePrivilage, Status = OperationStatus.ERROR };
                }
                else
                    return new RoleResponse { Message = Resources.RoleCantBeEditedOrDeleted, Status = OperationStatus.ERROR };
            }
            else
                return new RoleResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
        }
        public async Task<OperationStatusResponse> Delete(long Id)
        {
              var role = await _roleRepository.FirstOrDefaultAsync(r => r.Id == Id);
                if (role == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
                //check if the role is assigned to a user if not delete
                var user = _userRepository.Where(ur => ur.RoleId == Id && ur.RecordStatus != RecordStatus.Deleted).ToList();
                if (user.Count() > 0)
                    return new OperationStatusResponse { Message = Resources.RoleDeleteNotAllowedMessage, Status = OperationStatus.ERROR };
                if (role.IsReadOnly == false)
                {
                    role.RecordStatus = RecordStatus.Deleted;
                    role.LastUpdateDate = DateTime.UtcNow;
                    role.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                    if (_roleRepository.Update(role))
                        return new OperationStatusResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                    else
                        return new OperationStatusResponse { Message = Resources.DatabaseOperationFailed, Status = OperationStatus.ERROR };
                }
                else
                    return new RoleResponse { Message = Resources.RoleCantBeEditedOrDeleted, Status = OperationStatus.ERROR };
          
        }
        public RoleResponse GetRoleByUser(long userId)
        {
            var roleResponseList = new RoleResponse();
            var user = _userRepository.Where(ur => ur.Id == userId && ur.RecordStatus == RecordStatus.Active).FirstOrDefault();
            if (user == null)
                return new RoleResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            var role = _roleRepository.Where(u => u.Id == user.RoleId && u.RecordStatus == RecordStatus.Active 
            //&&
            //                u.AccountSubscriptionId == Convert.ToInt64(_httpContextAccessor.HttpContext.Session.GetString("AccountSubscriptionId"))
                            ).FirstOrDefault();
            if (role == null)
                return new RoleResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            var rolePrivilageList = _rolePrivilageRepository.Where(rp => rp.RoleId == role.Id, new string[] { nameof(Privilege) }).ToList();
            if (rolePrivilageList.Count() == 0)
                return new RoleResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            var roleResponse = new RoleRes
            {
                Id = role.Id,
                Description = role.Description,
                Name = role.Name,
                IsReadOnly = role.IsReadOnly,
                RecordStatus = role.RecordStatus
            };
            foreach (var rolePrivilage in rolePrivilageList)
                roleResponse.Privileges.Add(new PrivilegeRes { Id = rolePrivilage.PrivilegeId, Action = rolePrivilage.Privilege.Action, IsMorePermission = rolePrivilage.Privilege.IsMorePermission, Module = rolePrivilage.Privilege.Module });
            roleResponseList.Role = roleResponse;
            roleResponseList.Status = OperationStatus.SUCCESS;
            roleResponseList.Message = Resources.OperationSucessfullyCompleted;
            return roleResponseList;
        }
        public RoleResponse GetRoleGroupedPriviledgeByRole(long roleId)
        {
            var roleResponseList = new RoleResponse();
            var role = _roleRepository.Where(u => u.Id == roleId && u.RecordStatus == RecordStatus.Active 
            //&&
            //                u.AccountSubscriptionId == Convert.ToInt64(_httpContextAccessor.HttpContext.Session.GetString("AccountSubscriptionId"))
                            ).FirstOrDefault();
            if (role == null)
                return new RoleResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            var roleResponse = new RoleRes
            {
                Id = role.Id,
                Description = role.Description,
                Name = role.Name,
                IsReadOnly = role.IsReadOnly,
                RecordStatus = role.RecordStatus
            };
            var roleGroupPriveledge = _privilegeService.GetGroupPrivilegePerRole(roleId);
            if (roleGroupPriveledge.Status == OperationStatus.SUCCESS)
            {
                roleResponse.Modules = roleGroupPriveledge.Modules;
                roleResponseList.Role = roleResponse;
                roleResponseList.Status = OperationStatus.SUCCESS;
                roleResponseList.Message = Resources.OperationSucessfullyCompleted;
                return roleResponseList;
            }
            roleResponseList.Status = OperationStatus.ERROR;
            roleResponseList.Message = Resources.OperationEndWithError;
            return roleResponseList;
        }
    }
}
