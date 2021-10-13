using EProcurement.Common.Helper;
using EProcurement.Common.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EProcurement.Api.Api.MasterData.V1._0
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet(nameof(UploadFileToSharePointOnlineAsync))]
        public async Task<string> UploadFileToSharePointOnlineAsync()
        {
            var sharePointOnline = new SharePointOnline();
            string filePath = @"c:\Temp\computerPurchaseRequestt.docx";

            await sharePointOnline.UploadFileToSharePointOnlineAsync(new FileConfig
            {
                FilePath = filePath,
                SiteUrl = @"https://azuredevelopersethiopianair.sharepoint.com/sites/eprocurement",
                FileName = filePath.Substring(filePath.LastIndexOf("\\") + 1)
            });

            return "OK";
        }
    }
}
