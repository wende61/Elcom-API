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
    public class FinancialEvaluationService : IEvaluation<FinancialEvaluationResponse,FinancialEvaluationsResponse, FinancialEvaluationRequest, FinancialEvaluationUpdateRequest>
    {

        private readonly IRepositoryBase<Project> _projectRepository;
        private readonly IRepositoryBase<FinancialEvaluation> _financialEvaluationRepository;
        private readonly IRepositoryBase<FinancialCriteria> _criteriaRepository;
        private readonly IRepositoryBase<FinancialCriteriaItem> _criteriaItemRepository;
        private readonly IAppDbTransactionContext _transactionContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;
        private readonly IServiceUtility _serviceUtility;
        private readonly IMapper _mapper;
        public FinancialEvaluationService(IHttpContextAccessor httpContextAccessor, IUserService userService,
           ILoggerManager logger, IServiceUtility serviceUtility, IMapper mapper, IAppDbTransactionContext transactionContext
           , IRepositoryBase<FinancialEvaluation> financialEvaluationRepository, IRepositoryBase<FinancialCriteria> criteriaRepository,IRepositoryBase<FinancialCriteriaItem> criteriaItemRepository, IRepositoryBase<Project> projectRepository)
        {
            _transactionContext = transactionContext;
            _financialEvaluationRepository = financialEvaluationRepository;
            _criteriaItemRepository = criteriaItemRepository;
            _criteriaRepository = criteriaRepository;
            _projectRepository = projectRepository;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _logger = logger;
            _serviceUtility = serviceUtility;
            _mapper = mapper;
        }
        public async Task<FinancialEvaluationResponse> CreateAsync(FinancialEvaluationRequest request)
        {
            try
            {
                var project = _projectRepository.Where(x => x.Id == request.ProjectId).Include(x => x.TechnicalEvaluations).FirstOrDefault();
                if (project == null)
                    return new FinancialEvaluationResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.EMPTY, MessageList = { "Unable to find Project!" }};
                if (project.TechnicalEvaluations.Count()==0)
                    return new FinancialEvaluationResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.EMPTY, MessageList = { "Unable to find Technical Evaluation!" }};
                if (project.TechnicalEvaluations.FirstOrDefault().TechnicalEvaluationValue> 100)
                    return new FinancialEvaluationResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.EMPTY, MessageList = { "Technical Evaluation value must be <= 100!" } };
                var awardFactorResult = CalculateAwardFactor(project.TechnicalEvaluations.FirstOrDefault().TechnicalEvaluationValue);
                using (var uow = new AppUnitOfWork(_transactionContext.GetDbContext()))
                {
                    RepositoryBaseWork<FinancialEvaluation> FinancialEvaluationRepo = new RepositoryBaseWork<FinancialEvaluation>(uow);
                    using (var transaction = uow.BeginTrainsaction())
                    {
                        try
                        {
                            var financialEvaluation = _mapper.Map<FinancialEvaluation>(request);
                            financialEvaluation.AwardFactor = awardFactorResult.AwardFactor;
                            financialEvaluation.FinancialEvaluationValue = awardFactorResult.FinancialEvaluationValue;
                            financialEvaluation.StartDate = DateTime.Now;
                            financialEvaluation.EndDate = DateTime.MaxValue;
                            financialEvaluation.RegisteredDate = DateTime.Now;
                            financialEvaluation.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                            financialEvaluation.RecordStatus = RecordStatus.Active;
                            financialEvaluation.IsReadOnly = false;
                            foreach (var financialCriteriaGroup in request.FinancialCriteriaGroups)
                            {
                                var criteriaGroup = _mapper.Map<FinancialCriteriaGroup>(financialCriteriaGroup);
                                criteriaGroup.FinancialEvaluationId = financialEvaluation.Id;
                                criteriaGroup.StartDate = DateTime.Now;
                                criteriaGroup.EndDate = DateTime.MaxValue;
                                criteriaGroup.RegisteredDate = DateTime.Now;
                                criteriaGroup.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                criteriaGroup.RecordStatus = RecordStatus.Active;
                                criteriaGroup.IsReadOnly = false;
                                foreach (var criterion in financialCriteriaGroup.FinancialCriterias)
                                {
                                    var criteria = _mapper.Map<FinancialCriteria>(criterion);
                                    criteria.FinancialCriteriaGroupId = criteriaGroup.Id;
                                    criteria.StartDate = DateTime.Now;
                                    criteria.EndDate = DateTime.MaxValue;
                                    criteria.RegisteredDate = DateTime.Now;
                                    criteria.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                    criteria.RecordStatus = RecordStatus.Active;
                                    criteria.IsReadOnly = false;
                                    criteriaGroup.FinancialCriterias.Add(criteria);
                                    foreach (var item in criterion.FinancialCriteriaItems)
                                    {
                                        var criteriaItem = _mapper.Map<FinancialCriteriaItem>(item);
                                        criteriaItem.FinancialCriteriaId = criteria.Id;
                                        criteriaItem.StartDate = DateTime.Now;
                                        criteriaItem.EndDate = DateTime.MaxValue;
                                        criteriaItem.RegisteredDate = DateTime.Now;
                                        criteriaItem.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                        criteriaItem.RecordStatus = RecordStatus.Active;
                                        criteriaItem.IsReadOnly = false;
                                        criteria.FinancialCriteriaItems.Add(criteriaItem);
                                    }
                                }
                                financialEvaluation.FinancialCriteriaGroups.Add(criteriaGroup);
                            }
                            FinancialEvaluationRepo.Add(financialEvaluation);
                            if (await uow.SaveChangesAsync() > 0)
                            {
                                transaction.Commit();
                                return new FinancialEvaluationResponse { 
                                    Response= _mapper.Map<FinancialEvaluationDTO>(financialEvaluation),
                                    Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                            }
                            transaction.Rollback();
                            return new FinancialEvaluationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                        catch (Exception ex)
                        {
                            return new FinancialEvaluationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new FinancialEvaluationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var financialEvaluation = await _financialEvaluationRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (financialEvaluation == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    if (_financialEvaluationRepository.Remove(financialEvaluation))
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
        public FinancialEvaluationResponse GetById(long id)
        {
            try
            {
                var result = new FinancialEvaluationResponse();
                var financialEvaluation = _financialEvaluationRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).Include(x => x.FinancialCriteriaGroups).FirstOrDefault();
                if (financialEvaluation == null)
                    return new FinancialEvaluationResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var financialEvaluationDTO = _mapper.Map<FinancialEvaluationDTO>(financialEvaluation);
                foreach (var criteriaGroup in financialEvaluationDTO.FinancialCriteriaGroups)
                {
                    var criteriaGroupDTO = _mapper.Map<FinancialCriteriaGroupDTO>(criteriaGroup);
                    var criterias = _criteriaRepository.Where(x => x.FinancialCriteriaGroupId == criteriaGroup.Id && x.RecordStatus == RecordStatus.Active).ToList();
                    foreach (var criteria in criterias)
                    {
                        var criteriaDTO = _mapper.Map<FinancialCriteriaDTO>(criteria);
                        var criteriaItems = _criteriaItemRepository.Where(x => x.FinancialCriteriaId == criteria.Id && x.RecordStatus == RecordStatus.Active).ToList();
                        foreach (var criteriaItem in criteriaItems)
                        {
                            var criteriaItemDTO = _mapper.Map<FinancialCriteriaItemDTO>(criteriaItem);
                            criteriaDTO.FinancialCriteriaItems.Add(criteriaItemDTO);

                        }
                        criteriaGroupDTO.FinancialCriterias.Add(criteriaDTO);
                    }
                }
                result.Response = financialEvaluationDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new FinancialEvaluationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public FinancialEvaluationResponse GetByParentId(long id)
        {
            try
            {
                var result = new FinancialEvaluationResponse();
                var technicalEvaluation = _financialEvaluationRepository.Where(x => x.ProjectId == id && x.RecordStatus == RecordStatus.Active).Include(x => x.FinancialCriteriaGroups).FirstOrDefault();
                if (technicalEvaluation == null)
                    return new FinancialEvaluationResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
                var financialEvaluationDTO = _mapper.Map<FinancialEvaluationDTO>(technicalEvaluation);
                foreach (var criteriaGroup in financialEvaluationDTO.FinancialCriteriaGroups)
                {
                    var criteriaGroupDTO = _mapper.Map<FinancialCriteriaGroupDTO>(criteriaGroup);
                    var criterias = _criteriaRepository.Where(x => x.FinancialCriteriaGroupId == criteriaGroup.Id && x.RecordStatus == RecordStatus.Active).ToList();
                    foreach (var criteria in criterias)
                    {
                        var criteriaDTO = _mapper.Map<FinancialCriteriaDTO>(criteria);
                        var criteriaItems = _criteriaItemRepository.Where(x => x.FinancialCriteriaId == criteria.Id && x.RecordStatus == RecordStatus.Active).ToList();
                        foreach (var criteriaItem in criteriaItems)
                        {
                            var criteriaItemDTO = _mapper.Map<FinancialCriteriaItemDTO>(criteriaItem);
                            criteriaDTO.FinancialCriteriaItems.Add(criteriaItemDTO);
                        }
                        criteriaGroupDTO.FinancialCriterias.Add(criteriaDTO);
                    }
                }
                result.Response = financialEvaluationDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new FinancialEvaluationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<FinancialEvaluationResponse> Update(FinancialEvaluationUpdateRequest request)
        {
            var financialEvaluation = _financialEvaluationRepository.Where(c => c.Id == request.Id).Include(x=>x.FinancialCriteriaGroups).FirstOrDefault();
            if (financialEvaluation == null)
                return new FinancialEvaluationResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                financialEvaluation.EvaluationName = request.EvaluationName;
                financialEvaluation.FinancialEvaluationValue = request.FinancialEvaluationValue;
                financialEvaluation.AwardFactor = request.AwardFactor;
                financialEvaluation.ProjectId = request.ProjectId;
                financialEvaluation.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                financialEvaluation.LastUpdateDate = DateTime.UtcNow;
                if (_financialEvaluationRepository.Update(financialEvaluation))
                {
                    return new FinancialEvaluationResponse
                    {
                        Response = _mapper.Map<FinancialEvaluationDTO>(financialEvaluation),
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new FinancialEvaluationResponse
                    {

                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new FinancialEvaluationResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }

        public FinancialEvaluationDTO CalculateAwardFactor(double technicalEvaluation)
        {
            var result = new FinancialEvaluationDTO();
            try
            {
                if (technicalEvaluation==0)
                {
                    result.AwardFactor = AwardFactor.FinancialOnly;
                    result.FinancialEvaluationValue = 100;

                }
                else if (technicalEvaluation==100)
                {
                    result.AwardFactor = AwardFactor.TechnicalOnly;
                    result.FinancialEvaluationValue = 0;
                }
                else
                {
                    result.AwardFactor = AwardFactor.CombinedSuM;
                    result.FinancialEvaluationValue = (100 - technicalEvaluation);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException();
            }
        }
    }
}
