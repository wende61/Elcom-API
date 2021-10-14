using Elcom.Common;
using Elcom.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elcom.Core
{
    public interface ITokenService<T> where T : class
    {
        Task<UserSignInResponse> RefreshAccessToken(string refreshToken);
        Task<UserSignInResponse> GetAccessToken(string email);
        Task<UserSignInResponse> GetAccessToken(ClientUser user);
        Task<UserSignInResponse> GetAccessToken(User user);
    }
}
