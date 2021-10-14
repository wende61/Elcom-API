using Elcom.Common.Model;
using System.Threading.Tasks;

namespace Elcom.Common.IHelper
{
    public interface ISharePointOnline
    {
        public Task<bool> UploadFileToSharePointOnlineAsync(FileConfig fileConfig);
    }
}
