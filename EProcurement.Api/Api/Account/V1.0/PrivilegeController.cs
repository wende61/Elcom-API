using System.Threading.Tasks;
using EProcurement.Common;
using EProcurement.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace EProcurement.Api
{
	[ApiController]
	[Route("api/V1.0/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class PrivilegeController : ControllerBase
	{
		private IPrivilegeService _privilageService;
		public PrivilegeController(IPrivilegeService privilageService)
		{
			_privilageService = privilageService;

		}
		[HttpGet(nameof(GetAll))]
        public ActionResult<PrivilegesResponse> GetAll()
		{
			var result = _privilageService.GetAll();
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}
        [HttpGet(nameof(GetAllByModule))]
        public ActionResult<GroupPrivilegesResponse> GetAllByModule()
        {
            var result =_privilageService.GetGroupPrivilege();
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}
		[HttpGet(nameof(GetGroupedPrivilegePerUser))]
		public ActionResult<GroupPrivilegesResponse> GetGroupedPrivilegePerUser(long userId)
		{
			var result = _privilageService.GetGroupPrivilegePerUser(userId);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}
		[HttpGet(nameof(GetGroupedPrivilegePerRole))]
		public ActionResult<GroupPrivilegesResponse> GetGroupedPrivilegePerRole(long roleId)
		{
			var result = _privilageService.GetGroupPrivilegePerRole(roleId);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}
		[HttpGet(nameof(GetById))]
        public ActionResult<PrivilegeResponse> GetById(long id)
		{
			var result = _privilageService.GetById(id);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}
		[HttpPost(nameof(Save))]
        public async Task<ActionResult<PrivilegeResponse>> Save([FromBody] PrivilegeRequest request)
		{
			var result = await _privilageService.Create(request);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}
		[HttpPost(nameof(Update))]
        public async Task<ActionResult<PrivilegeResponse>> Update([FromBody] PrivilegeRequest request)
		{
			var result = await _privilageService.Update(request);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}
		[HttpDelete(nameof(Delete))]
		public async Task<ActionResult<PrivilegeResponse>> Delete(long Id)
		{
			var result = await _privilageService.Delete(Id);
			if (result.Status == OperationStatus.SUCCESS)
				return Ok(result);
			else
				return StatusCode(500, result);
		}
	}
}