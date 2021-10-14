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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ClientAccountController : ControllerBase
    {
        private IAccountService<AccountServiceClient> _accountService;
        private IBaseService<ClientUserRequest, ClientUserResponse, ClientUsersResponse> _userService;

        public ClientAccountController(IAccountService<AccountServiceClient> accountService,
            IBaseService<ClientUserRequest, ClientUserResponse, ClientUsersResponse> userService)
        {
            _accountService = accountService;
            _userService = userService;
        }


        [HttpGet(nameof(GetAll))]
        public ActionResult<ActionResult<ClientUsersResponse>> GetAll()
        {
            var result = _userService.GetAll();
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }


        [HttpGet(nameof(GetById))]
        public ActionResult<ClientUserResponse> GetById(long id)
        {
            var result = _userService.GetById(id);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }

        [HttpPost(nameof(Save))]
        public async Task<ActionResult<ClientUserResponse>> Save([FromBody] ClientUserRequest request)
        {
            var result = await _userService.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }


        [HttpPost(nameof(Update))]
        public async Task<ActionResult<ClientUserResponse>> Update([FromBody] ClientUserRequest request)
        {
            var result = await _userService.Update(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
    }
}
