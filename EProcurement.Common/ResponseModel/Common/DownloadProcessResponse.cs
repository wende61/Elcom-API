using System.IO;

namespace EProcurement.Common
{
    public class DownloadProcessResponse : OperationStatusResponse
    {
        public MemoryStream FileMemoryStream { get; set; }
        public string ContentType { get; set; }
        public string GetFileName { get; set; }
    }
}
