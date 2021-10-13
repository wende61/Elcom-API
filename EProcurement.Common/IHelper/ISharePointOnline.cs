using EProcurement.Common.Model;
using System.Threading.Tasks;

namespace EProcurement.Common.IHelper
{
    public interface ISharePointOnline
    {
        public Task<bool> UploadFileToSharePointOnlineAsync(FileConfig fileConfig);
    }
}
