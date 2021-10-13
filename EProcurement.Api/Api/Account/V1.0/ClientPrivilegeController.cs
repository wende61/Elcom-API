using EProcurement.Common;
using EProcurement.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EProcurement.Api.Api.Account.V1._0
{
	[ApiController]
	[Route("api/V1.0/[controller]")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public class ClientPrivilegeController : ControllerBase
    {
        private readonly IPrivilegeServiceClient _privilegeServiceClient;

        public ClientPrivilegeController(IPrivilegeServiceClient privilegeServiceClient)
        {
            _privilegeServiceClient = privilegeServiceClient;
        }

		[HttpGet(nameof(GetAll))]
		public ActionResult<PrivilegeResponse> GetAll()
		{
			var result = _privilegeServiceClient.GetAll();
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}		

		[HttpGet(nameof(GetGroupPrivilege))]
		public ActionResult<GroupPrivilegesResponse> GetGroupPrivilege()
		{
			var result = _privilegeServiceClient.GetGroupPrivilege();
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpGet(nameof(GetById))]
		public ActionResult<PrivilegeResponse> GetById(long id)
		{
			var result = _privilegeServiceClient.GetById(id);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpPost(nameof(Save))]
		public async Task<ActionResult<PrivilegeResponse>> Save([FromBody] PrivilegeRequest request)
		{
			var result = await _privilegeServiceClient.Create(request);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpPut(nameof(Update))]
		public async Task<ActionResult<PrivilegeResponse>> Update([FromBody] PrivilegeRequest request)
		{
			var result = await _privilegeServiceClient.Update(request);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpDelete(nameof(Delete))]
		public async Task<ActionResult<OperationStatusResponse>> Delete(long Id)
		{
			var result = await _privilegeServiceClient.Delete(Id);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}
	}
}
