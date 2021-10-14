using Elcom.Common;
using System.Threading.Tasks;

namespace Elcom.Core
{
    public interface IUserService : IBaseService<UserRequest, UserResponse, UsersResponse>
    {
        Task<UserResponse> UpdateProfile(UserRequest  userRequest);
        //Task<OperationStatusResponse> InviteUser(InviteUserRequest inviteUserRequest);
        Task<UserSignInResponse> ConfirmUser(InviteUserConfimationRequest inviteUserConfimationRequest);
        Task<OperationStatusResponse> ActivateDeactivateUser(ActivateDeactivateRequest activateDeactivateUserRequest);
        string GetUserName(string? authorization);
    }
}
