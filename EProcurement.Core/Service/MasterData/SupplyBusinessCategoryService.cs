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
    public class SupplyBusinessCategoryService : ICrud<BusinessCategoryResponse, BusinessCategoriesResponse, BusinessCategoryRequest>
    {
        private readonly IRepositoryBase<BusinessCategory> _sbcRepository;
        private readonly IRepositoryBase<BusinessCategoryType> _sbctRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public SupplyBusinessCategoryService(IRepositoryBase<BusinessCategory> sbcRepository, ILoggerManager logger, IHttpContextAccessor httpContextAccessor, IMapper mapper,   IRepositoryBase<BusinessCategoryType> sbctRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _sbctRepository = sbctRepository;
            _sbcRepository = sbcRepository;
            _mapper = mapper;
            _logger = logger;
        }
        //
        public async Task<BusinessCategoryResponse> Create(BusinessCategoryRequest request)
        {
            try
            {
                var sbc = _mapper.Map<BusinessCategory>(request);
                sbc.StartDate = DateTime.Now;
                sbc.EndDate = DateTime.MaxValue;
                sbc.RegisteredDate = DateTime.Now;
                sbc.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                sbc.RecordStatus = RecordStatus.Active;
                sbc.IsReadOnly = false;
                var existingSbct = _sbcRepository.FirstOrDefaultAsync(x => x.Category == sbc.Category && x.BusinessCategoryTypeId == sbc.BusinessCategoryTypeId && x.RecordStatus == RecordStatus.Active);
                if (existingSbct.Result != null)
                    return new BusinessCategoryResponse { Message = "Supply/Business Category  Already Existed", Status = OperationStatus.ERROR };
                if (_sbcRepository.Add(sbc))
                    return new BusinessCategoryResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new BusinessCategoryResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new BusinessCategoryResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var sbct = await _sbcRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (sbct == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    sbct.RecordStatus = RecordStatus.Deleted;
                    if (_sbcRepository.Update(sbct))
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

        public BusinessCategoriesResponse GetAll()
        {
            try
            {
                var result = new BusinessCategoriesResponse();
                var sbcs = _sbcRepository.Where(x => x.RecordStatus == RecordStatus.Active);
                foreach (var sbc in sbcs)
                {
                    var sbct = _sbctRepository.Find(sbc.BusinessCategoryTypeId);
                    var sbcDOT = _mapper.Map<BusinessCategoryDTO>(sbc);
                    sbcDOT.SupplyBusinessCategoryType = _mapper.Map<BusinessCategoryTypeDTO>(sbct);
                    result.Response.Add(sbcDOT);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;

            }
            catch (Exception ex)
            {
                return new BusinessCategoriesResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public BusinessCategoryResponse GetById(long id)
        {
            try
            {
                var result = new BusinessCategoryResponse();
                var sbc = _sbcRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (sbc == null)
                    return new BusinessCategoryResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var sbct = _sbctRepository.Find(sbc.BusinessCategoryTypeId);
                var sbcDOT = _mapper.Map<BusinessCategoryDTO>(sbc);
                sbcDOT.SupplyBusinessCategoryType = _mapper.Map<BusinessCategoryTypeDTO>(sbct);
                result.Response = sbcDOT;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new BusinessCategoryResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<BusinessCategoryResponse> Update(BusinessCategoryRequest request)
        {
            var sbc= _sbcRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (sbc == null)
                return new BusinessCategoryResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                sbc.Category = request.Category;
                sbc.Description = request.Description;
                sbc.BusinessCategoryTypeId = request.BusinessCategoryTypeId;
                sbc.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                sbc.LastUpdateDate = DateTime.UtcNow;
                if (_sbcRepository.Update(sbc))
                {
                    return new BusinessCategoryResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new BusinessCategoryResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new BusinessCategoryResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
