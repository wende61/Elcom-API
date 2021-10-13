using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EProcurement.Common;
using EProcurement.DataObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EProcurement.Core
{
    public class ClientUserService : IBaseService<ClientUserRequest, ClientUserResponse, ClientUsersResponse>
    {
        private readonly IRepositoryBase<ClientUser> _userRepository;
        private readonly IPasswordHasher<ClientUser> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClientUserService(
            IRepositoryBase<ClientUser> userRepository,
            IPasswordHasher<ClientUser> passwordHasher,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }

        //public async Task<OperationStatusResponse> Delete(long Id)
        //{
        //    var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == Id);
        //    if (user == null)
        //        return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

        //    var userAccountSubscription = await _accountSubscriptionUserRepository.FirstOrDefaultAsync(acctSub => acctSub.Email == user.Username);
        //    if (userAccountSubscription == null)
        //        return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

        //    using (var appUow = new AppUnitOfWork(_appTransaction.GetDbContext()))
        //    {
        //        using (var appTransaction = appUow.BeginTrainsaction())
        //        {
        //            using (var masterUow = new MasterUnitOfWork(_masterTransaction.GetDbContext()))
        //            {
        //                using (var masterTransaction = masterUow.BeginTrainsaction())
        //                {
        //                    try
        //                    {
        //                        RepositoryBase<UserRole> userRoleRepository = new RepositoryBase<UserRole>(appUow);
        //                        RepositoryBase<User> userRepository = new RepositoryBase<User>(appUow);
        //                        MasterRepositoryBase<AccountSubscriptionUser> accountSubscriptionUserRepository = new MasterRepositoryBase<AccountSubscriptionUser>(masterUow);

        //                        var newUser = new User
        //                        {
        //                            RecordStatus = RecordStatus.Deleted,
        //                            EndDate = DateTime.UtcNow,
        //                            LastUpdateDate = DateTime.UtcNow,
        //                            UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername")
        //                        };

        //                        userRepository.Update(newUser);
        //                        await appUow.SaveChangesAsync();


        //                        var userSubscriptionInfo = new AccountSubscriptionUser
        //                        {
        //                            RecordStatus = RecordStatus.Deleted,
        //                            EndDate = DateTime.UtcNow,
        //                            LastUpdateDate = DateTime.UtcNow,
        //                            UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername")
        //                        };
        //                        accountSubscriptionUserRepository.Update(userSubscriptionInfo);
        //                        await masterUow.SaveChangesAsync();

        //                        appTransaction.Commit();
        //                        masterTransaction.Commit();
        //                        return new UserResponse
        //                        {
        //                            Message = Resources.OperationSucessfullyCompleted,
        //                            Status = OperationStatus.SUCCESS
        //                        };
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        appTransaction.Rollback();
        //                        masterTransaction.Rollback();
        //                        return new UserResponse
        //                        {
        //                            Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
        //                            Status = OperationStatus.ERROR
        //                        };
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public async Task<OperationStatusResponse> Delete(long Id)
        {
            return new OperationStatusResponse { Status = OperationStatus.SUCCESS };
        }

        public ClientUsersResponse GetAll()
        {
            var usersResponse = new ClientUsersResponse();
            var users = _userRepository.
                Where(u => (u.RecordStatus == RecordStatus.Active ||
                            u.RecordStatus == RecordStatus.Inactive));

            if (users != null)
            {
                foreach (var userSubscription in users)
                {

                    var user = _userRepository.FirstOrDefault(u => u.Username == userSubscription.Email);

                    var userDetail = new ClientUserRes();

                    userDetail.Id = userSubscription.Id;
                    userDetail.FirstName = userSubscription.FirstName;
                    userDetail.LastName = userSubscription.LastName;
                    userDetail.Username = userSubscription.Email;
                    userDetail.RecordStatus = userSubscription.RecordStatus;
                    userDetail.IsSuperAdmin = userSubscription.IsSuperAdmin;
                    userDetail.IsAccountLocked = userSubscription.RecordStatus == RecordStatus.Active ? false : true;
                    userDetail.ClientRoleId = Convert.ToInt64(userSubscription.ClientRoleId);

                    usersResponse.ClientUsers.Add(userDetail);
                }
            }
            usersResponse.Status = OperationStatus.SUCCESS;
            usersResponse.Message = Resources.OperationSucessfullyCompleted;
            return usersResponse;
        }

        public ClientUserResponse GetById(long id)
        {
            var userSubscription = _userRepository.FirstOrDefault(u => u.Id == id && u.RecordStatus == RecordStatus.Active);

            if (userSubscription != null)
            {
                var userResponse = new ClientUserResponse();

                userResponse.Status = OperationStatus.SUCCESS;
                userResponse.Message = Resources.OperationSucessfullyCompleted;

                var userDetail = new ClientUserRes();

                userDetail.Id = userSubscription.Id;
                userDetail.FirstName = userSubscription.FirstName;
                userDetail.LastName = userSubscription.LastName;
                userDetail.Username = userSubscription.Email;
                userDetail.RecordStatus = userSubscription.RecordStatus;
                userDetail.IsSuperAdmin = userSubscription.IsSuperAdmin;
                userDetail.IsAccountLocked = userSubscription.RecordStatus == RecordStatus.Active ? false : true;
                userDetail.ClientRoleId = Convert.ToInt64(userSubscription.ClientRoleId);

                userResponse.ClientUser = userDetail;
                return userResponse;
            }
            return new ClientUserResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
        }

        public async Task<ClientUserResponse> Create(ClientUserRequest request)
        {
            var previousUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Username);
            if (previousUser != null)
                return new ClientUserResponse { Message = Resources.RecordAlreadyExist, Status = OperationStatus.ERROR };

            if (request.ClientRoleId <= 0)
                return new ClientUserResponse { Message = Resources.PleaseSelectAtLeastOneRole, Status = OperationStatus.ERROR };

                var userSubscriptionInfo = new ClientUser
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    IsSuperAdmin = request.IsSuperAdmin,
                    IsAccountLocked = false,
                    IsConfirmationEmailSent = true,
                    Email = request.Username,
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    StartDate = DateTime.UtcNow,
                    ClientRoleId = request.ClientRoleId,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    LastUpdateDate = DateTime.UtcNow,
                    UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername")
                };
                userSubscriptionInfo.Password = _passwordHasher.HashPassword(userSubscriptionInfo, request.Password);
                if (_userRepository.Add(userSubscriptionInfo))
                    return new ClientUserResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };

      
            return new ClientUserResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR };
        }

        public async Task<ClientUserResponse> Update(ClientUserRequest request)
        {

            if (request.ClientRoleId <= 0)
                return new ClientUserResponse { Message = Resources.PleaseSelectAtLeastOneRole, Status = OperationStatus.ERROR };


            var user = await _userRepository.FirstOrDefaultAsync(u => u.Username == request.Username);

            var prevAccountSubscription = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Username);
            if (prevAccountSubscription == null || user == null)
                return new ClientUserResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

            prevAccountSubscription.FirstName = request.FirstName;
            prevAccountSubscription.LastName = request.LastName;
            prevAccountSubscription.IsSuperAdmin = request.IsSuperAdmin;
            prevAccountSubscription.TimeZoneInfo = TimeZoneInfo.Local.StandardName;
            prevAccountSubscription.LastUpdateDate = DateTime.UtcNow;
            prevAccountSubscription.ClientRoleId = request.ClientRoleId;
            prevAccountSubscription.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");

            if (_userRepository.Update(prevAccountSubscription))
                return new ClientUserResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };

            return new ClientUserResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR };


        }

    }
}
