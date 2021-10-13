using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EProcurement.DataObjects.Models.Operational;
using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Operational;
using EProcurement.DataObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EProcurement.Core.Service.Operational
{
    public class FinancialCriteriaGroupService : ICrud<FinancialCriteriaGroupResponse, FinancialCriteriaGroupsResponse ,FinancialCriteriaGroupRequest, FinancialCriteriaGroupUpdateRequest>
    {
        private readonly IRepositoryBase<FinancialCriteriaGroup> _financialCriteriaGroupRepository;
        private readonly IRepositoryBase<FinancialCriteria> _financialCriteriaRepository;
        private readonly IRepositoryBase<FinancialCriteriaItem> _financialCriteriaItemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbTransactionContext _transactionContext;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public FinancialCriteriaGroupService(IRepositoryBase<FinancialCriteriaGroup> financialCriteriaGroupRepo, IHttpContextAccessor httpContextAccessor, ILoggerManager logger, IMapper mapper,
            IAppDbTransactionContext transactionContext, IRepositoryBase<FinancialCriteria> financialCriteriaRepository, IRepositoryBase<FinancialCriteriaItem> financialCriteriaItemRepository)
        {
            _financialCriteriaGroupRepository = financialCriteriaGroupRepo;
            _financialCriteriaRepository = financialCriteriaRepository;
            _financialCriteriaItemRepository = financialCriteriaItemRepository;
            _httpContextAccessor = httpContextAccessor;
            _transactionContext = transactionContext;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<FinancialCriteriaGroupResponse> CreateAsync(FinancialCriteriaGroupRequest request)
        {
            try
            {
                using (var uow = new AppUnitOfWork(_transactionContext.GetDbContext()))
                {
                    RepositoryBaseWork<FinancialCriteriaGroup> _financecriteriaGroupRepo = new RepositoryBaseWork<FinancialCriteriaGroup>(uow);
                    using (var transaction= uow.BeginTrainsaction())
                    {
                        var financialCriteriaGroup = _mapper.Map<FinancialCriteriaGroup>(request);
                        financialCriteriaGroup.StartDate = DateTime.Now;
                        financialCriteriaGroup.EndDate = DateTime.MaxValue;
                        financialCriteriaGroup.RegisteredDate = DateTime.Now;
                        financialCriteriaGroup.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                        financialCriteriaGroup.RecordStatus = RecordStatus.Active;
                        financialCriteriaGroup.IsReadOnly = false;
                        foreach (var criterias in request.FinancialCriterias)
                        {
                            var financialCriteria = _mapper.Map<FinancialCriteria>(criterias);
                            financialCriteria.StartDate = DateTime.Now;
                            financialCriteria.EndDate = DateTime.MaxValue;
                            financialCriteria.RegisteredDate = DateTime.Now;
                            financialCriteria.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                            financialCriteria.RecordStatus = RecordStatus.Active;
                            financialCriteria.IsReadOnly = false;
                            foreach (var items in criterias.FinancialCriteriaItems)
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
                            financialCriteriaGroup.FinancialCriterias.Add(financialCriteria);
                        }
                        _financecriteriaGroupRepo.Add(financialCriteriaGroup);
                        if (await uow.SaveChangesAsync()>0)
                        {
                            transaction.Commit();
                            return new FinancialCriteriaGroupResponse {
                                Response = _mapper.Map<FinancialCriteriaGroupDTO>(financialCriteriaGroup),
                                Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                        }
                        transaction.Rollback();
                        return new FinancialCriteriaGroupResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                    }
                }
            }
            catch (Exception ex)
            {
                return new FinancialCriteriaGroupResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var financialCriteriaGroup = await _financialCriteriaGroupRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (financialCriteriaGroup == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    if (_financialCriteriaGroupRepository.Remove(financialCriteriaGroup))
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
        public FinancialCriteriaGroupResponse GetById(long id)
        {
            try
            {
                var result = new FinancialCriteriaGroupResponse();
                var financialCriteriaGroup = _financialCriteriaGroupRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).Include(x => x.FinancialEvaluation).FirstOrDefault();
                if (financialCriteriaGroup == null)
                    return new FinancialCriteriaGroupResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var criteriaGroupDTO = _mapper.Map<FinancialCriteriaGroupDTO>(financialCriteriaGroup);
                var criterias = _financialCriteriaRepository.Where(x => x.FinancialCriteriaGroupId == financialCriteriaGroup.Id && x.RecordStatus == RecordStatus.Active).ToList();
                foreach (var criteria in criterias)
                {
                    var criteriaDTO = _mapper.Map<FinancialCriteriaDTO>(criteria);
                    var criteriaItems = _financialCriteriaItemRepository.Where(x => x.FinancialCriteriaId == criteria.Id && x.RecordStatus == RecordStatus.Active).ToList();
                    foreach (var criteriaItem in criteriaItems)
                    {
                        var criteriaItemDTO = _mapper.Map<FinancialCriteriaItemDTO>(criteriaItem);
                        criteriaDTO.FinancialCriteriaItems.Add(criteriaItemDTO);
                    }
                    criteriaGroupDTO.FinancialCriterias.Add(criteriaDTO);
                }            
                result.Response = criteriaGroupDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new FinancialCriteriaGroupResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public FinancialCriteriaGroupsResponse GetByParentId(long id)
        {
            try
            {
                var result = new FinancialCriteriaGroupsResponse();
                var financialCriteriaGroup = _financialCriteriaGroupRepository.Where(x => x.FinancialEvaluationId == id && x.RecordStatus == RecordStatus.Active).Include(x => x.FinancialEvaluation).FirstOrDefault();
                if (financialCriteriaGroup == null)
                    return new FinancialCriteriaGroupsResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var criteriaGroupDTO = _mapper.Map<FinancialCriteriaGroupDTO>(financialCriteriaGroup);
                var criterias = _financialCriteriaRepository.Where(x => x.FinancialCriteriaGroupId == financialCriteriaGroup.Id && x.RecordStatus == RecordStatus.Active).ToList();
                foreach (var criteria in criterias)
                {
                    var criteriaDTO = _mapper.Map<FinancialCriteriaDTO>(criteria);
                    var criteriaItems = _financialCriteriaItemRepository.Where(x => x.FinancialCriteriaId == criteria.Id && x.RecordStatus == RecordStatus.Active).ToList();
                    foreach (var criteriaItem in criteriaItems)
                    {
                        var criteriaItemDTO = _mapper.Map<FinancialCriteriaItemDTO>(criteriaItem);
                        criteriaDTO.FinancialCriteriaItems.Add(criteriaItemDTO);
                    }
                    criteriaGroupDTO.FinancialCriterias.Add(criteriaDTO);
                }
                result.Response.Add(criteriaGroupDTO);
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new FinancialCriteriaGroupsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<FinancialCriteriaGroupResponse> Update(FinancialCriteriaGroupUpdateRequest request)
        {
            var financialCriteriaGroup = _financialCriteriaGroupRepository.Where(c => c.Id == request.Id).Include(x=>x.FinancialCriterias).FirstOrDefault();
            if (financialCriteriaGroup == null)
                return new FinancialCriteriaGroupResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                financialCriteriaGroup.Name = request.Name;
                financialCriteriaGroup.Sum = request.Sum;
                financialCriteriaGroup.FinancialEvaluationId = request.FinancialEvaluationId;
                financialCriteriaGroup.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                financialCriteriaGroup.LastUpdateDate = DateTime.UtcNow;
                if (_financialCriteriaGroupRepository.Update(financialCriteriaGroup))
                {
                    return new FinancialCriteriaGroupResponse
                    {
                        Response = _mapper.Map<FinancialCriteriaGroupDTO>(financialCriteriaGroup),
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new FinancialCriteriaGroupResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new FinancialCriteriaGroupResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
