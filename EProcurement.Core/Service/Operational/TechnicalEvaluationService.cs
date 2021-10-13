using AutoMapper;
using EProcurement.DataObjects.Models.Operational;
using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Operational;
using EProcurement.DataObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EProcurement.Core.Service.Operational
{
    public class TechnicalEvaluationService : IEvaluation<TechnicalEvaluationResponse, TechnicalEvaluationsResponse,TechnicalEvaluationRequest, TechnicalEvaluationUpdateRequest>
    {
        private readonly IRepositoryBase<TechnicalEvaluation> _technicalEvaluationRepository;
        private readonly IRepositoryBase<Criterion> _criteriaRepository;
        private readonly IAppDbTransactionContext _transactionContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;
        private readonly IServiceUtility _serviceUtility;
        private readonly IMapper _mapper;
        public TechnicalEvaluationService(IHttpContextAccessor httpContextAccessor, IUserService userService,
            ILoggerManager logger ,IServiceUtility serviceUtility,IMapper mapper,IAppDbTransactionContext transactionContext
            , IRepositoryBase<TechnicalEvaluation> technicalEvaluation, IRepositoryBase<Criterion> criteriaRepository)
        {
            _transactionContext = transactionContext;
            _technicalEvaluationRepository = technicalEvaluation;
            _criteriaRepository = criteriaRepository;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _logger = logger;
            _serviceUtility = serviceUtility;
            _mapper = mapper;
        }
        public async Task<TechnicalEvaluationResponse> CreateAsync(TechnicalEvaluationRequest request)
        {
            try
            {
                using (var uow = new AppUnitOfWork(_transactionContext.GetDbContext()))
                {
                    RepositoryBaseWork<TechnicalEvaluation> technicalEvaluationRepo = new RepositoryBaseWork<TechnicalEvaluation>(uow);
                    using (var transaction= uow.BeginTrainsaction())
                    {
                        try
                        {
                            var technicalEvaluation = _mapper.Map<TechnicalEvaluation>(request);
                            technicalEvaluation.CriteriaGroup = new List<CriteriaGroup>();
                            technicalEvaluation.StartDate = DateTime.Now;
                            technicalEvaluation.EndDate = DateTime.MaxValue;
                            technicalEvaluation.RegisteredDate = DateTime.Now;
                            technicalEvaluation.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                            technicalEvaluation.RecordStatus = RecordStatus.Active;
                            technicalEvaluation.IsReadOnly = false;
                            foreach (var cg in request.CriteriaGroup)
                            {
                                var criteriaGroup = _mapper.Map<CriteriaGroup>(cg);
                                criteriaGroup.TechnicalEvaluationId = technicalEvaluation.Id;
                                criteriaGroup.StartDate = DateTime.Now;
                                criteriaGroup.EndDate = DateTime.MaxValue;
                                criteriaGroup.RegisteredDate = DateTime.Now;
                                criteriaGroup.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                criteriaGroup.RecordStatus = RecordStatus.Active;
                                criteriaGroup.IsReadOnly = false;
                                foreach (var criterion in cg.Criteria)
                                {
                                    var criteria = _mapper.Map<Criterion>(criterion);
                                    criteria.CriteriaGroupId = criteriaGroup.Id;
                                    criteria.StartDate = DateTime.Now;
                                    criteria.EndDate = DateTime.MaxValue;
                                    criteria.RegisteredDate = DateTime.Now;
                                    criteria.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                    criteria.RecordStatus = RecordStatus.Active;
                                    criteria.IsReadOnly = false;
                                    criteriaGroup.Criteria.Add(criteria);
                                }
                                technicalEvaluation.CriteriaGroup.Add(criteriaGroup);
                            }
                            technicalEvaluationRepo.Add(technicalEvaluation);
                            if (await uow.SaveChangesAsync() > 0)
                            {
                                transaction.Commit();
                                return new TechnicalEvaluationResponse {
                                    Response = _mapper.Map<TechnicalEvaluationDTO>(technicalEvaluation),
                                    Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                            }
                            transaction.Rollback();
                            return new TechnicalEvaluationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                        catch (Exception ex)
                        {
                            return new TechnicalEvaluationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return new TechnicalEvaluationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var technicalEvaluation = await _technicalEvaluationRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (technicalEvaluation == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    if (_technicalEvaluationRepository.Remove(technicalEvaluation))
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
        public TechnicalEvaluationResponse GetById(long id)
        {
            try
            {
                var result = new TechnicalEvaluationResponse();
                var technicalEvaluation = _technicalEvaluationRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).Include(x=>x.CriteriaGroup).FirstOrDefault();
                if (technicalEvaluation == null)
                    return new TechnicalEvaluationResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var technicalEvaluationDTO = _mapper.Map<TechnicalEvaluationDTO>(technicalEvaluation);
                result.Response = technicalEvaluationDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new TechnicalEvaluationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public TechnicalEvaluationResponse GetByParentId(long id)
        {
            try
            {
                var result = new TechnicalEvaluationResponse();
                var technicalEvaluation = _technicalEvaluationRepository.Where(x => x.ProjectId == id && x.RecordStatus == RecordStatus.Active).Include(x => x.CriteriaGroup).Cast<TechnicalEvaluation>().FirstOrDefault();
                if (technicalEvaluation == null)
                    return new TechnicalEvaluationResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
                    var technicalEvaluationDTO = _mapper.Map<TechnicalEvaluationDTO>(technicalEvaluation);
                    foreach (var criteriaGroup in technicalEvaluationDTO.CriteriaGroup)
                    {
                        var criterias = _criteriaRepository.Where(x => x.CriteriaGroupId == criteriaGroup.Id && x.RecordStatus == RecordStatus.Active).ToList();
                        foreach (var criteria in criterias)
                        {
                            var CriteriaDTO = _mapper.Map<CriterionDTO>(criteria);
                            criteriaGroup.Criteria.Add(CriteriaDTO);
                        }
                    }
                    result.Response=technicalEvaluationDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new TechnicalEvaluationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }      
        public async Task<TechnicalEvaluationResponse> Update(TechnicalEvaluationUpdateRequest request)
        {
            var technicalEvaluation = _technicalEvaluationRepository.Where(c => c.Id == request.Id).Include(x=>x.CriteriaGroup).Include(x=>x.CriteriaGroup).FirstOrDefault();
            if (technicalEvaluation == null)
                return new TechnicalEvaluationResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                technicalEvaluation.EvaluationName = request.EvaluationName;
                technicalEvaluation.CutOffPoint = request.CutOffPoint;
                technicalEvaluation.TechnicalEvaluationValue = request.TechnicalEvaluationValue;
                technicalEvaluation.ProjectId = request.ProjectId;
                technicalEvaluation.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                technicalEvaluation.LastUpdateDate = DateTime.UtcNow;
                if (_technicalEvaluationRepository.Update(technicalEvaluation))
                {
                    return new TechnicalEvaluationResponse
                    {
                        Response = _mapper.Map<TechnicalEvaluationDTO>(technicalEvaluation),
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new TechnicalEvaluationResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new TechnicalEvaluationResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
