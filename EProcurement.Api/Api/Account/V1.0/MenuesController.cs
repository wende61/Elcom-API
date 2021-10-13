using EProcurement.Common;
using EProcurement.Core;
using EProcurement.Core.Interface.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EProcurement.Api
{
    [ApiController]
    [Route("api/V1.0/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class MenuesController : ControllerBase
    {
        private IMenusService _menuService;
        public MenuesController(IMenusService menuService)
        {
            _menuService = menuService;
        }
        [HttpGet(nameof(GetAll))]
        public ActionResult<ActionResult<MenusResponse>> GetAll()
        {
            var result = _menuService.getallmenus();
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpGet(nameof(GetById))]
        public ActionResult<MenuResponse> GetById(long id)
        {
            var result = _menuService.getmenubyid(id);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpPost(nameof(Save))]
        public async Task<ActionResult<MenuResponse>> Save([FromBody] MenuRequest request)
        {
            var result = await _menuService.addmenu(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpPost(nameof(Update))]
        public async Task<ActionResult<MenuResponse>> Update([FromBody] MenuRequest request)
        {
            var result = await _menuService.updatemenu(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
    }
}
