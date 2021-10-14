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
	public class RoleController : ControllerBase
	{
		private IRoleService _roleService;
		public RoleController(IRoleService roleService)
		{
			_roleService = roleService;

		}

		[HttpGet(nameof(GetAll))]
		public ActionResult<RolesResponse> GetAll()
		{
			var result = _roleService.GetAll();
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpGet(nameof(GetRoleByUser))]
		public ActionResult<RoleResponse> GetRoleByUser(long userId)
		{
			var result = _roleService.GetRoleByUser(userId);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpGet(nameof(GetRoleGropedPrivilegeByRole))]
		public ActionResult<RoleResponse> GetRoleGropedPrivilegeByRole(long roleId)
		{
			var result = _roleService.GetRoleGroupedPriviledgeByRole(roleId);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpGet(nameof(GetById))]
        public ActionResult<RoleResponse> GetById(long id)
		{
			var result = _roleService.GetById(id);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpPost(nameof(Save))]
        public async Task<ActionResult<RoleResponse>> Save([FromBody] RoleRequest request)
		{
			var result = await _roleService.Create(request);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpPut(nameof(Update))]
        public async Task<ActionResult<RoleResponse>> Update([FromBody] RoleRequest request)
		{
			var result = await _roleService.Update(request);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpDelete(nameof(Delete))]
		public async Task<ActionResult<RoleResponse>> Delete(long Id)
		{
			var result = await _roleService.Delete(Id);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}
	}
}