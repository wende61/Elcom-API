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
    public class ClientRoleService : IBaseService<ClientRoleRequest, ClientRoleResponse, ClientRolesResponse>
    {
        private readonly IRepositoryBase<ClientRole> _roleRepository;
        private readonly IRepositoryBase<ClientRolePrivilege> _rolePrivilageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientRoleService(
            IRepositoryBase<ClientRole> roleRepository,
            IRepositoryBase<ClientRolePrivilege> rolePrivilageRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _roleRepository = roleRepository;
            _rolePrivilageRepository = rolePrivilageRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public ClientRolesResponse GetAll()
        {
            var roleResponseList = new ClientRolesResponse();
            var roleList = _roleRepository.Where(u => u.RecordStatus == RecordStatus.Active ||
                            u.RecordStatus == RecordStatus.Inactive);

            if (roleList != null)
            {
                foreach (var role in roleList)
                {
                    var rolePrivilageList = _rolePrivilageRepository.Where(rp => rp.RoleId == role.Id, new string[] { nameof(Privilege) });

                    var roleResponse = new ClientRoleRes
                    {
                        Id = role.Id,
                        Description = role.Description,
                        Name = role.Name,
                        RecordStatus = role.RecordStatus
                    };

                    if (rolePrivilageList != null)
                        foreach (var rolePrivilage in rolePrivilageList)
                            roleResponse.Privileges.Add(new ClientPrivilegeRes { Id = rolePrivilage.PrivilegeId, Action = rolePrivilage.Privilege.Action });
                    roleResponseList.Roles.Add(roleResponse);

                }
            }
            roleResponseList.Status = OperationStatus.SUCCESS;
            roleResponseList.Message = Resources.OperationSucessfullyCompleted;
            return roleResponseList;
        }

        public ClientRoleResponse GetById(long id)
        {
            var role = _roleRepository.FirstOrDefault(r => r.Id == id);

            if (role != null)
            {
                var roleResponse = new ClientRoleResponse();

                roleResponse.Status = OperationStatus.SUCCESS;
                roleResponse.Message = Resources.OperationSucessfullyCompleted;


                roleResponse.Role = new ClientRoleRes
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    RecordStatus = role.RecordStatus,
                };

                var rolePrivilageList = _rolePrivilageRepository.Where(rp => rp.RoleId == role.Id, new string[] { nameof(Privilege) });
                if (rolePrivilageList != null)
                    foreach (var rolePrivilage in rolePrivilageList)
                        roleResponse.Role.Privileges.Add(new ClientPrivilegeRes { Id = rolePrivilage.PrivilegeId, Action = rolePrivilage.Privilege.Action });

                return roleResponse;
            }
            return new ClientRoleResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };

        }

        public async Task<ClientRoleResponse> Create(ClientRoleRequest request)
        {

            var prevRole = await _roleRepository.FirstOrDefaultAsync(r => r.Name == request.Name);
            if (prevRole == null)
            {
                if (request.Privileges != null && request.Privileges.Count > 0)
                {
                    var role = new ClientRole
                    {
                        Name = request.Name,
                        Description = request.Description,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.MaxValue,
                        RecordStatus = RecordStatus.Active,
                        UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername"),
                        TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                        LastUpdateDate = DateTime.UtcNow,
                    };

                    if (_roleRepository.Add(role))
                    {
                        foreach (var previlageId in request.Privileges)
                        {
                            var rolePrivilage = new ClientRolePrivilege
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

                            _rolePrivilageRepository.Add(rolePrivilage);
                        }
                        return new ClientRoleResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };

                    }
                }
                else
                    return new ClientRoleResponse { Message = Resources.AtleastSelectOnePrivilage, Status = OperationStatus.ERROR };
            }
            else
                return new ClientRoleResponse { Message = Resources.RecordAlreadyExist, Status = OperationStatus.ERROR };

            return new ClientRoleResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR };

        }

        public async Task<ClientRoleResponse> Update(ClientRoleRequest request)
        {
            var prevRole = await _roleRepository.FirstOrDefaultAsync(r => r.Id == request.Id);
            if (prevRole != null)
            {
                if (request.Privileges != null && request.Privileges.Count > 0)
                {

                    prevRole.Description = request.Description;
                    prevRole.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                    prevRole.TimeZoneInfo = TimeZoneInfo.Local.StandardName;
                    prevRole.LastUpdateDate = DateTime.UtcNow;

                    if (_roleRepository.Update(prevRole))
                    {
                        var prevRolePrevilageList = _rolePrivilageRepository.Where(rp => rp.RoleId == request.Id).ToList();

                        foreach (var previlageId in request.Privileges)
                        {
                            if (prevRolePrevilageList != null)
                            {
                                var preRolePrevilage = prevRolePrevilageList.Where(rp => rp.RoleId == prevRole.Id && rp.PrivilegeId == previlageId).FirstOrDefault();
                                if (preRolePrevilage != null)
                                {
                                    prevRolePrevilageList.Remove(preRolePrevilage);
                                    continue;
                                }
                            }
                            var rolePrivilage = new ClientRolePrivilege
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

                            _rolePrivilageRepository.Add(rolePrivilage);
                        }

                        //remove role-privliage which do not belong to the new list
                        if (prevRolePrevilageList != null)
                        {
                            foreach (var prevRolePrevilage in prevRolePrevilageList)
                            {
                                _rolePrivilageRepository.Remove(prevRolePrevilage);
                            }
                        }
                        return new ClientRoleResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                    }



                }
                else
                    return new ClientRoleResponse { Message = Resources.AtleastSelectOnePrivilage, Status = OperationStatus.ERROR };
            }
            else
                return new ClientRoleResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

            return new ClientRoleResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR };
        }

        public async Task<OperationStatusResponse> Delete(long Id)
        {
            //if (bulkAction?.Ids == null || bulkAction.Ids.Count < 1)
            //    return new OperationStatusResponse { Message = Resources.PleaseSelectOneRecordToDelete, Status = OperationStatus.ERROR };

            //foreach (var id in bulkAction.Ids)
            //{
            //    var role = await _roleRepository.FindAsync(id);
            //    if (role != null)
            //    {
            //        role.RecordStatus = RecordStatus.Deleted;
            //        role.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
            //        role.TimeZoneInfo = TimeZoneInfo.Local.StandardName;
            //        role.LastUpdateDate = DateTime.UtcNow;
            //        _roleRepository.Update(role);
            //    }
            //}
            return new OperationStatusResponse { Message = string.Format(Resources.OperationSucessfullyCompletedNumberOfRecordAffected), Status = OperationStatus.SUCCESS };

        }

    }
}
