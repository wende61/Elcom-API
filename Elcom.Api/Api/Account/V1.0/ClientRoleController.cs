using Elcom.Common;
using Elcom.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elcom.Api.Api.Account.V1._0
{    
    [ApiController]    
    [Route("api/V1.0/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ClientRoleController : ControllerBase
    {
        private readonly IBaseService<ClientRoleRequest, ClientRoleResponse, ClientRolesResponse> _clientRoleService;

        public ClientRoleController(IBaseService<ClientRoleRequest, ClientRoleResponse, ClientRolesResponse> clientRoleService)
        {
            _clientRoleService = clientRoleService;
        }

		[HttpGet(nameof(GetAll))]
		public ActionResult<ClientRolesResponse> GetAll()
		{
			var result = _clientRoleService.GetAll();
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}				

		[HttpGet(nameof(GetById))]
		public ActionResult<ClientRolesResponse> GetById(long id)
		{
			var result = _clientRoleService.GetById(id);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpPost(nameof(Save))]
		public async Task<ActionResult<ClientRolesResponse>> Save([FromBody] ClientRoleRequest request)
		{
			var result = await _clientRoleService.Create(request);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpPut(nameof(Update))]
		public async Task<ActionResult<ClientRolesResponse>> Update([FromBody] ClientRoleRequest request)
		{
			var result = await _clientRoleService.Update(request);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpDelete(nameof(Delete))]
		public async Task<ActionResult<OperationStatusResponse>> Delete(long Id)
		{
			var result = await _clientRoleService.Delete(Id);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}
	}
}
