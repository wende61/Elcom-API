using Elcom.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Elcom.Core.Interface.Helper
{
    public interface IFileHelper
    {
        Task<StoredFileResponse> Upload(IFormFile file);
        Task<OperationStatusResponse> UploadMultipleFiles(IFormFile file);
    }
   
}
