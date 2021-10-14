using Elcom.Common;
using Elcom.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elcom.Api
{
    [ApiController]
    [Route("api/V1.0/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class TokenController : ControllerBase
    {
        private IAccountService<AccountServiceClient> _accountServiceClient;
        private ITokenService<ClientTokenService> _tokenService;
        public TokenController(
           ITokenService<ClientTokenService> tokenService, IAccountService<AccountServiceClient> accountServiceClient)
        {
            _accountServiceClient = accountServiceClient;
            _tokenService = tokenService;
        }
       
        [HttpPost(nameof(GetToken))]
        public async Task<ActionResult<UserSignInResponse>> GetToken([FromBody] UserSignInRequest request)
        {
            var result = await _accountServiceClient.SignIn(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }

        [HttpPost(nameof(RefreshToken))]
        public async Task<ActionResult<UserSignInResponse>> RefreshToken(string refreshToken)
        {
            var result = await _tokenService.RefreshAccessToken(refreshToken);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }

    }
}
