using Elcom.Common.Helper;
using Elcom.Common.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elcom.Api.Api.MasterData.V1._0
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet(nameof(UploadFileToSharePointOnlineAsync))]
        public async Task<string> UploadFileToSharePointOnlineAsync()
        {
            return "OK";
        }
    }
}
