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
    public class SupplyBusinessCategoryTypeService : ICrud<BusinessCategoryTypeResponse, BusinessCategoryTypesResponse, SupplyBusinessCategoryTypeRequest>
    {
        private readonly IRepositoryBase<BusinessCategoryType> _sbctRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public SupplyBusinessCategoryTypeService(IRepositoryBase<BusinessCategoryType> sbctRepository, ILoggerManager logger, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _sbctRepository = sbctRepository;
            _mapper = mapper;
            _logger = logger;
        }
        //
        public async Task<BusinessCategoryTypeResponse> Create(SupplyBusinessCategoryTypeRequest request)
        {
            try
            {
                var sbct = _mapper.Map<BusinessCategoryType>(request);
                sbct.StartDate = DateTime.Now;
                sbct.EndDate = DateTime.MaxValue;
                sbct.RegisteredDate = DateTime.Now;
                sbct.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                sbct.RecordStatus = RecordStatus.Active;
                sbct.IsReadOnly = false;
                var existingSbct = _sbctRepository.FirstOrDefaultAsync(x => x.CategoryType == sbct.CategoryType  && x.RecordStatus == RecordStatus.Active);
                if (existingSbct.Result != null)
                    return new BusinessCategoryTypeResponse { Message = "Supply/Business Category Type Already Existed", Status = OperationStatus.ERROR };
                if (_sbctRepository.Add(sbct))
                    return new BusinessCategoryTypeResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new BusinessCategoryTypeResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new BusinessCategoryTypeResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var sbct = await _sbctRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (sbct == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    sbct.RecordStatus = RecordStatus.Deleted;
                    if (_sbctRepository.Update(sbct))
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
        public BusinessCategoryTypesResponse GetAll()
        {
            try
            {
                var result = new BusinessCategoryTypesResponse();
                var sbcts = _sbctRepository.Where(x => x.RecordStatus == RecordStatus.Active);
                foreach (var sbct in sbcts)
                {
                    var sbctDOT = _mapper.Map<BusinessCategoryTypeDTO>(sbct);
                    result.Response.Add(sbctDOT);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;

            }
            catch (Exception ex)
            {
                return new BusinessCategoryTypesResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public BusinessCategoryTypeResponse GetById(long id)
        {
            try
            {
                var result = new BusinessCategoryTypeResponse();
                var sbct = _sbctRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (sbct == null)
                    return new BusinessCategoryTypeResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var sbctDOT = _mapper.Map<BusinessCategoryTypeDTO>(sbct);
                result.Response = sbctDOT;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new BusinessCategoryTypeResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<BusinessCategoryTypeResponse> Update(SupplyBusinessCategoryTypeRequest request)
        {
            var sbct = _sbctRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (sbct == null)
                return new BusinessCategoryTypeResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                sbct.CategoryType = request.CategoryType;
                sbct.Description = request.Description;                
                sbct.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                sbct.LastUpdateDate = DateTime.UtcNow;
                if (_sbctRepository.Update(sbct))
                {
                    return new BusinessCategoryTypeResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new BusinessCategoryTypeResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new BusinessCategoryTypeResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
