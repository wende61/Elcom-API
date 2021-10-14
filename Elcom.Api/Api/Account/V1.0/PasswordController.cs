using System.Threading.Tasks;
using Elcom.Common;
using Elcom.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Elcom.Api
{

    [ApiController]
    [Route("api/V1.0/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class PasswordController : ControllerBase
    {
        private IPasswordService _passwordService;
        public PasswordController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }
        [AllowAnonymous]
        [HttpPost(nameof(ForgotPassword))]
        public async Task<ActionResult<OperationStatusResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _passwordService.ForgotPassword(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpPost(nameof(ChangePassword))]
        public async Task<ActionResult<OperationStatusResponse>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _passwordService.ChangePassword(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpPost(nameof(ResetPassword))]
        public async Task<ActionResult<OperationStatusResponse>> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _passwordService.ResetPassword(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [AllowAnonymous]
        [HttpPost(nameof(ResetForgotPassword))]
        public async Task<ActionResult<RecoverPasswordResponse>> ResetForgotPassword([FromBody] ResetForgotPasswordRequest request)
        {
            var result = await _passwordService.ResetForgotPassword(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
    }
}