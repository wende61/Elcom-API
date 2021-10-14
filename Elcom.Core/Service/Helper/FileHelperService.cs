using Elcom.Common;
using Elcom.Core.Interface.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Elcom.Core.Service.Helper
{
    public class FileHelperService : IFileHelper
    {
        private readonly string[] permitedExtensions = new string[] { ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx", ".jpg", ".jpeg", ".png", ".gif" };
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FileSettings fileSettings;
        public FileHelperService(IConfiguration config,  IHttpContextAccessor httpContextAccessor)
        {
            var fileSettingsSection = config.GetSection("FileSettings");
            fileSettings = fileSettingsSection.Get<FileSettings>();
            _httpContextAccessor = httpContextAccessor;
        }
        //public async Task<StoredFileResponse> Upload(IFormFile file)
        //{
        //    try
        //    {
        //        var checkFileSize = CheckFileSize(file.Length);
        //        if (checkFileSize.Status == OperationStatus.ERROR)
        //            return new StoredFileResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = checkFileSize.MessageList };
        //        var checkFileExtension = CheckFileExtension(file.FileName);
        //        if (checkFileExtension.Status == OperationStatus.ERROR)
        //            return new StoredFileResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = checkFileExtension.MessageList };
        //        var trustedFileName = GenerateTrustedFileName(file.FileName);
        //        return UploadFile(file, trustedFileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new StoredFileResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { ex.Message } };
        //    }
        //}
        public async Task<OperationStatusResponse> UploadMultipleFiles(IFormFile file)
        {
            try
            {
                return new OperationStatusResponse { Status = OperationStatus.SUCCESS, Message = Resources.OperationSucessfullyCompleted };

            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { ex.Message } };
            }
        }
        protected OperationStatusResponse CheckFileSize(long fileLength)
        {
            try
            {
                if (fileLength == 0) return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { "File is empty (0MB)." } };
                if (fileLength > fileSettings.FileSizeLimit) return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { "File exceed the limit." } };
                return new OperationStatusResponse { Status = OperationStatus.SUCCESS };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { ex.Message } };
            }
        }
        protected OperationStatusResponse CheckFileExtension(string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName)) return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { "The fileName is empty" } };
                var extension = Path.GetExtension(fileName);
                if (permitedExtensions.Contains(extension))
                    return new OperationStatusResponse { Status = OperationStatus.SUCCESS };
                return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { "The file type(" + extension + ") is not supported." } };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { ex.Message } };
            }
        }

        public Task<StoredFileResponse> Upload(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
