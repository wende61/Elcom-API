using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EProcurement.Common;
using EProcurement.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using EProcurement.Common.RequestModel.MasterData;

namespace EProcurement.Api
{

    [ApiController]
    [Route("api/V1.0/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AccountController : ControllerBase
    {
        private IAccountService<AccountService> _accountService;
        private IUserService _userService;

        public AccountController(IAccountService<AccountService> accountService,
            IUserService userService)
        {
            _accountService = accountService;
            _userService = userService;
        }

        #region SignIn/Out
        [HttpPost(nameof(SignIn))]
        public async Task<ActionResult<UserSignInResponse>> SignIn([FromBody] UserSignInRequest request)
        {
            var result = await _accountService.SignIn(request);

            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }

        [HttpPost(nameof(SignOut))]
        public async Task<ActionResult<OperationStatusResponse>> SignOut([FromBody] UserSignOutRequest request)
        {
            var result = await _accountService.SignOut(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }

        #endregion
        #region User Managment
        //[HttpPost(nameof(InviteUser))]
        //public async Task<ActionResult<OperationStatusResponse>> InviteUser([FromBody] InviteUserRequest inviteUserRequest)
        //{
        //    var result = await _userService.InviteUser(inviteUserRequest);
        //    if (result.Status == OperationStatus.SUCCESS)
        //        return Ok(result);
        //    else
        //        return StatusCode(500, result);
        //}
        [HttpPost(nameof(ConfirmUser))]
        public async Task<ActionResult<OperationStatusResponse>> ConfirmUser([FromBody] InviteUserConfimationRequest inviteUserConfimationRequest)
        {
            var result = await _userService.ConfirmUser(inviteUserConfimationRequest);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }

        [HttpPost(nameof(MarkAsActiveInActive))]
        public async Task<ActionResult<OperationStatusResponse>> MarkAsActiveInActive([FromBody] ActivateDeactivateRequest activateDeactivateUserRequest)
        {
            var result = await _userService.ActivateDeactivateUser(activateDeactivateUserRequest);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }

        [HttpGet(nameof(GetAll))]
        public ActionResult<UsersResponse> GetAll()
        {
            var result = _userService.GetAll();
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }


        [HttpGet(nameof(GetById))]
        public ActionResult<UserResponse> GetById(long id)
        {
            var result = _userService.GetById(id);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }

        [HttpPost(nameof(Save))]
        public async Task<ActionResult<UserResponse>> Save([FromBody] UserRequest request)
        {
            var result = await _userService.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }


        [HttpPost(nameof(RegisterSupplier))]
        public async Task<ActionResult<UserResponse>> RegisterSupplier([FromBody] SupplierRegistrationRequest request)
        {
            var result = await _userService.RegisterSupplier(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpPut(nameof(Update))]
        public async Task<ActionResult<UserResponse>> Update([FromBody] UserRequest request)
        {
            var result = await _userService.Update(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpPut(nameof(UpdateProfile))]
        public async Task<ActionResult<UserResponse>> UpdateProfile([FromBody] UserRequest request)
        {
            var result = await _userService.UpdateProfile(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<ActionResult<UserResponse>> Delete(long Id)
        {
            var result = await _userService.Delete(Id);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        } 
        #endregion
    }
}