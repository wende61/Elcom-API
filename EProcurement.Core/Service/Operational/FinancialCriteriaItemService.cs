using AutoMapper;
using EProcurement.DataObjects.Models.Operational;
using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Operational;
using EProcurement.DataObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EProcurement.Core.Service.Operational
{
    public class FinancialCriteriaItemService : ICrud<FinancialCriteriaItemResponse, FinancialCriteriaItemsResponse, FinancialCriteriaItemRequest, FinancialCriteriaItemRequest>
    {
        private readonly IRepositoryBase<FinancialCriteriaItem> _financialCriteriaItemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public FinancialCriteriaItemService(IRepositoryBase<FinancialCriteriaItem> financialCriteriaItemRepository, IHttpContextAccessor httpContextAccessor,ILoggerManager logger,IMapper mapper)
        {
            _financialCriteriaItemRepository = financialCriteriaItemRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _mapper = mapper;
        }
        public  async Task<FinancialCriteriaItemResponse> CreateAsync(FinancialCriteriaItemRequest request)
        {
            try
            {
                try
                {
                    var financialCriteriaItem = _mapper.Map<FinancialCriteriaItem>(request);
                    financialCriteriaItem.StartDate = DateTime.Now;
                    financialCriteriaItem.EndDate = DateTime.MaxValue;
                    financialCriteriaItem.RegisteredDate = DateTime.Now;
                    financialCriteriaItem.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                    financialCriteriaItem.RecordStatus = RecordStatus.Active;
                    financialCriteriaItem.IsReadOnly = false;
                    if (_financialCriteriaItemRepository.Add(financialCriteriaItem))
                        return new FinancialCriteriaItemResponse { 
                            Response = _mapper.Map<FinancialCriteriaItemDTO>(financialCriteriaItem),
                            Message = Resources.OperationSucessfullyCompleted,
                            Status = OperationStatus.SUCCESS };
                    else
                        return new FinancialCriteriaItemResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                }
                catch (Exception ex)
                {
                    return new FinancialCriteriaItemResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var financialCriteriaItem = await _financialCriteriaItemRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (financialCriteriaItem == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    if (_financialCriteriaItemRepository.Remove(financialCriteriaItem))
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

        public FinancialCriteriaItemResponse GetById(long id)
        {
            try
            {
                var result = new FinancialCriteriaItemResponse();
                var criteriaItem = _financialCriteriaItemRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (criteriaItem==null)
                    return new FinancialCriteriaItemResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.EMPTY };
                var criteriaItemDTO = _mapper.Map<FinancialCriteriaItemDTO>(criteriaItem);
                 result.Response = criteriaItemDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new FinancialCriteriaItemResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public FinancialCriteriaItemsResponse GetByParentId(long id)
        {
            try
            {
                var result = new FinancialCriteriaItemsResponse();
                var criteriaItems = _financialCriteriaItemRepository.Where(x => x.FinancialCriteriaId == id && x.RecordStatus == RecordStatus.Active).ToList();
                if (criteriaItems == null)
                    return new FinancialCriteriaItemsResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.EMPTY };
                foreach (var criteriaItem in criteriaItems)
                {
                    var criteriaItemDTO = _mapper.Map<FinancialCriteriaItemDTO>(criteriaItem);
                    result.Response.Add(criteriaItemDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new FinancialCriteriaItemsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<FinancialCriteriaItemResponse> Update(FinancialCriteriaItemRequest request)
        {
            var financialCriteriaItem = _financialCriteriaItemRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (financialCriteriaItem == null)
                return new FinancialCriteriaItemResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                financialCriteriaItem.FiledName = request.FiledName;
                financialCriteriaItem.DataType = request.DataType;
                financialCriteriaItem.Value = request.Value;
                financialCriteriaItem.FinancialCriteriaId = request.FinancialCriteriaId;
                financialCriteriaItem.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                financialCriteriaItem.LastUpdateDate = DateTime.UtcNow;
                if (_financialCriteriaItemRepository.Update(financialCriteriaItem))
                {
                    return new FinancialCriteriaItemResponse
                    {
                        Response = _mapper.Map<FinancialCriteriaItemDTO>(financialCriteriaItem),
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new FinancialCriteriaItemResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new FinancialCriteriaItemResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
