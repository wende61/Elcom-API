using EProcurement.Common;
using EProcurement.Common.RequestModel.MasterData;
using EProcurement.DataObjects.Models.MasterData;
using System.Threading.Tasks;

namespace EProcurement.Core
{
    public interface IUserService : IBaseService<UserRequest, UserResponse, UsersResponse>
    {
        Task<UserResponse> UpdateProfile(UserRequest  userRequest);
        //Task<OperationStatusResponse> InviteUser(InviteUserRequest inviteUserRequest);
        Task<UserSignInResponse> ConfirmUser(InviteUserConfimationRequest inviteUserConfimationRequest);
        Task<OperationStatusResponse> ActivateDeactivateUser(ActivateDeactivateRequest activateDeactivateUserRequest);
        Person GetPerson(string userName);
        Supplier GetSupplier(string userName);
        Task<UserResponse> RegisterSupplier(SupplierRegistrationRequest request);
        string GetUserName(string? authorization);
    }
}
