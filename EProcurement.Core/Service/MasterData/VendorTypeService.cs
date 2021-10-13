using AutoMapper;
using EProcurement.Common;
using EProcurement.Common.RequestModel.MasterData;
using EProcurement.Common.ResponseModel.MasterData;
using EProcurement.Core.Interface.MasterData;
using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.MasterData;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Service.MasterData
{
    public class VendorTypeService : ICrud<VendorTypeResponse, VendorTypesResponse, VendorTypeRequest>
    {
        private readonly IRepositoryBase<VendorType> _vendorTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public VendorTypeService(IRepositoryBase<VendorType> vendorTypeRepository , IHttpContextAccessor httpContextAccessor, ILoggerManager logger, IMapper mapper)
        {
            _vendorTypeRepository = vendorTypeRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _mapper = mapper;
        }
        //
        public async Task<VendorTypeResponse> Create(VendorTypeRequest request)
        {
            try
            {
                var vendorType= _mapper.Map<VendorType>(request);
                vendorType.StartDate = DateTime.Now;
                vendorType.EndDate = DateTime.MaxValue;
                vendorType.RegisteredDate = DateTime.Now;
                vendorType.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                vendorType.RecordStatus = RecordStatus.Active;
                vendorType.IsReadOnly = false;
                var existingVendorT = _vendorTypeRepository.FirstOrDefaultAsync(x => x.Type == vendorType.Type && x.RecordStatus == RecordStatus.Active);
                if (existingVendorT.Result != null)
                    return new VendorTypeResponse { Message = "Vendor Type Already Existed", Status = OperationStatus.ERROR };
                if (_vendorTypeRepository.Add(vendorType))
                    return new VendorTypeResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new VendorTypeResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new VendorTypeResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var vendorType = await _vendorTypeRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (vendorType == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    vendorType.RecordStatus = RecordStatus.Deleted;
                    if (_vendorTypeRepository.Update(vendorType))
                        return new OperationStatusResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                    else
                        return new OperationStatusResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public VendorTypesResponse GetAll()
        {
            try
            {
                var result = new VendorTypesResponse();
                var VendorTypes = _vendorTypeRepository.Where(x => x.RecordStatus == RecordStatus.Active);
                foreach (var vendorType in VendorTypes)
                {
                    var vendorTypeDOT = _mapper.Map<VendorTypeDTO>(vendorType);
                    result.Response.Add(vendorTypeDOT);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;

            }
            catch (Exception ex)
            {
                return new VendorTypesResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public VendorTypeResponse GetById(long id)
        {
            try
            {
                var result = new VendorTypeResponse();
                var vendorType = _vendorTypeRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (vendorType == null)
                    return new VendorTypeResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var vendorTypeDOT = _mapper.Map<VendorTypeDTO>(vendorType);
                result.Response = vendorTypeDOT;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new VendorTypeResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<VendorTypeResponse> Update(VendorTypeRequest request)
        {
            var vendor = _vendorTypeRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (vendor == null)
                return new VendorTypeResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                vendor.Type = request.Type;
                vendor.Description = request.Description;
                vendor.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                vendor.LastUpdateDate = DateTime.UtcNow;
                if (_vendorTypeRepository.Update(vendor))
                {
                    return new VendorTypeResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new VendorTypeResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new VendorTypeResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
