using AutoMapper;
using EProcurement.DataObjects.Models.Operational;
using EProcurement.Core.Interface.Operational;
using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.DataObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace EProcurement.Core.Service.Operational
{
    public class FinancialCriteriaService : ICrud<FinancialCriteriaResponse, FinancialCriteriasResponse, FinancialCriteriaRequest,FinancialCriteriaUpdateRequest>
    {
        private readonly IRepositoryBase<FinancialCriteria> _financialCriteriaRepository;
        private readonly IRepositoryBase<FinancialCriteriaItem> _financialCriteriaItemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbTransactionContext _transactionContext;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public FinancialCriteriaService(IHttpContextAccessor httpContextAccessor, ILoggerManager logger, IMapper mapper, IAppDbTransactionContext transactionContext, IRepositoryBase<FinancialCriteria> financialCriteriaRepository, IRepositoryBase<FinancialCriteriaItem> financialCriteriaItemRepository)
        {
            _financialCriteriaRepository = financialCriteriaRepository;
            _financialCriteriaItemRepository = financialCriteriaItemRepository;
            _httpContextAccessor = httpContextAccessor;
            _transactionContext = transactionContext;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<FinancialCriteriaResponse> CreateAsync(FinancialCriteriaRequest request)
        {
            using (var uow = new AppUnitOfWork(_transactionContext.GetDbContext()))
            {
                RepositoryBaseWork<FinancialCriteria> _financeCriteriaRepo = new RepositoryBaseWork<FinancialCriteria>(uow);
                using (var transaction = uow.BeginTrainsaction())
                {

                    var financialCriteria = _mapper.Map<FinancialCriteria>(request);
                    financialCriteria.StartDate = DateTime.Now;
                    financialCriteria.EndDate = DateTime.MaxValue;
                    financialCriteria.RegisteredDate = DateTime.Now;
                    financialCriteria.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                    financialCriteria.RecordStatus = RecordStatus.Active;
                    financialCriteria.IsReadOnly = false;
                    foreach (var items in request.FinancialCriteriaItems)
                    {
                        var financialCriteriaItem = _mapper.Map<FinancialCriteriaItem>(items);
                        financialCriteriaItem.StartDate = DateTime.Now;
                        financialCriteriaItem.EndDate = DateTime.MaxValue;
                        financialCriteriaItem.RegisteredDate = DateTime.Now;
                        financialCriteriaItem.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                        financialCriteriaItem.RecordStatus = RecordStatus.Active;
                        financialCriteriaItem.IsReadOnly = false;
                        financialCriteria.FinancialCriteriaItems.Add(financialCriteriaItem);
                    }

                    _financeCriteriaRepo.Add(financialCriteria);
                    if (await uow.SaveChangesAsync() > 0)
                    {
                        transaction.Commit();
                        return new FinancialCriteriaResponse {
                            Response = _mapper.Map<FinancialCriteriaDTO>(financialCriteria),
                            Message = Resources.OperationSucessfullyCompleted, 
                            Status = OperationStatus.SUCCESS };
                    }
                    transaction.Rollback();
                    return new FinancialCriteriaResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                }
            }
        }
        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var financialCriteria = await _financialCriteriaRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (financialCriteria == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    if (_financialCriteriaRepository.Remove(financialCriteria))
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
        public FinancialCriteriaResponse GetById(long id)
        {
            try
            {
                var result = new FinancialCriteriaResponse();
                var criteria = _financialCriteriaRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                var criteriaDTO = _mapper.Map<FinancialCriteriaDTO>(criteria);
                var criteriaItems = _financialCriteriaItemRepository.Where(x => x.FinancialCriteriaId == criteria.Id && x.RecordStatus == RecordStatus.Active).ToList();
                foreach (var criteriaItem in criteriaItems)
                {
                    var criteriaItemDTO = _mapper.Map<FinancialCriteriaItemDTO>(criteriaItem);
                    criteriaDTO.FinancialCriteriaItems.Add(criteriaItemDTO);
                }

                result.Response = criteriaDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new FinancialCriteriaResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public FinancialCriteriasResponse GetByParentId(long id)
        {
            try
            {
                var result = new FinancialCriteriasResponse();
                var criterias = _financialCriteriaRepository.Where(x => x.FinancialCriteriaGroupId == id && x.RecordStatus == RecordStatus.Active).ToList();
                foreach (var criteria in criterias)
                {
                    var criteriaDTO = _mapper.Map<FinancialCriteriaDTO>(criteria);
                    var criteriaItems = _financialCriteriaItemRepository.Where(x => x.FinancialCriteriaId == criteria.Id && x.RecordStatus == RecordStatus.Active).ToList();
                    foreach (var criteriaItem in criteriaItems)
                    {
                        var criteriaItemDTO = _mapper.Map<FinancialCriteriaItemDTO>(criteriaItem);
                        criteriaDTO.FinancialCriteriaItems.Add(criteriaItemDTO);
                    }
                    result.Response.Add(criteriaDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new FinancialCriteriasResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<FinancialCriteriaResponse> Update(FinancialCriteriaUpdateRequest request)
        {
            var financialcriteria = _financialCriteriaRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (financialcriteria == null)
                return new FinancialCriteriaResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                financialcriteria.Name = request.Name;
                financialcriteria.Quantity = request.Quantity;
                financialcriteria.FinancialCriteriaGroupId = request.FinancialCriteriaGroupId;
                financialcriteria.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                financialcriteria.LastUpdateDate = DateTime.UtcNow;
                if (_financialCriteriaRepository.Remove(financialcriteria))
                {
                    return new FinancialCriteriaResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new FinancialCriteriaResponse
                    {
                        Response = _mapper.Map<FinancialCriteriaDTO>(financialcriteria),
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new FinancialCriteriaResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
