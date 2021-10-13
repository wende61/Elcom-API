using EProcurement.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace EProcurement.Core.Interface.Helper
{
    public interface ISharePoint
    {
        Task<bool> UploadFileToSharePointOnlineAsync(FileConfig fileConfig);
    }
}
