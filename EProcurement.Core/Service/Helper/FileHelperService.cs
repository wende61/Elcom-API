using CargoProrationAPI.DataObjects.Models.Operational;
using EProcurement.Common;
using EProcurement.Core.Interface.Helper;
using EProcurement.DataObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Service.Helper
{
    public class FileHelperService : IFileHelper
    {
        private readonly string[] permitedExtensions = new string[] { ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx", ".jpg", ".jpeg", ".png", ".gif" };
        private readonly IRepositoryBase<StoredFiles> _filesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FileSettings fileSettings;
        public FileHelperService(IConfiguration config, IRepositoryBase<StoredFiles> filesRepository, IHttpContextAccessor httpContextAccessor)
        {
            var fileSettingsSection = config.GetSection("FileSettings");
            fileSettings = fileSettingsSection.Get<FileSettings>();
            _httpContextAccessor = httpContextAccessor;
            _filesRepository = filesRepository;
        }
        public async Task<StoredFileResponse> Upload(IFormFile file)
        {
            try
            {
                var checkFileSize = CheckFileSize(file.Length);
                if (checkFileSize.Status == OperationStatus.ERROR)
                    return new StoredFileResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = checkFileSize.MessageList };
                var checkFileExtension = CheckFileExtension(file.FileName);
                if (checkFileExtension.Status == OperationStatus.ERROR)
                    return new StoredFileResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = checkFileExtension.MessageList };
                var trustedFileName = GenerateTrustedFileName(file.FileName);
                return UploadFile(file, trustedFileName);
            }
            catch (Exception ex)
            {
                return new StoredFileResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { ex.Message } };
            }
        }
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
        protected StoredFiles GenerateTrustedFileName(string fileName)
        {
            try
            {
                var result = new StoredFiles();
                var extension = Path.GetExtension(fileName);
                var isSuccess = false;
                do
                {
                    var newFileName = Path.GetRandomFileName().Replace(".", "") + extension;
                    var usedFileNames = _filesRepository.Where(x => x.TrustedName == newFileName).ToList();
                    if (usedFileNames.Count() == 0)
                    {
                        result = new StoredFiles();
                        result.TrustedName = newFileName;
                        result.UnTrustedName = fileName;
                        result.StartDate = DateTime.Now;
                        result.EndDate = DateTime.MaxValue;
                        result.RegisteredDate = DateTime.Now;
                        result.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                        result.RecordStatus = RecordStatus.Active;
                        result.IsReadOnly = false;
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false;
                    }

                } while (!isSuccess);

                return result;
            }
            catch (Exception ex)
            {
               throw;
            }
        }
        protected StoredFileResponse UploadFile(IFormFile file, StoredFiles fileData)
        {
            try
            {
                var filePath = Path.Combine(fileSettings.StoredFilesPath, fileData.TrustedName);
                using (var stream = File.Create(filePath))
                {
                    file.CopyToAsync(stream);
                }
                if (_filesRepository.Add(fileData))
                {
                    return new StoredFileResponse
                    {
                        Status = OperationStatus.SUCCESS,
                        TrustedName = fileData.TrustedName,
                        UnTrustedName=fileData.UnTrustedName,
                    };
                }
                else
                {
                    return new StoredFileResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { "Unable To upload" } };
                }

            }
            catch (Exception ex)
            {
                return new StoredFileResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { ex.Message } };

            }

        }

    }
}
