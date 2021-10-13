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
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Service.Operational
{
    public class CriteriaGroupService : ICrud<CriteriaGroupResponse, CriteriaGroupsResponse, CriteriaGroupRequest, CriteriaGroupUpdateRequest>
    {
        private readonly IRepositoryBase<CriteriaGroup> _criterionGroupRepository;
        private readonly IAppDbTransactionContext _contextTransaction;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CriteriaGroupService(IRepositoryBase<CriteriaGroup> criterionGroupRepository, IHttpContextAccessor httpContextAccessor, ILoggerManager logger, IMapper mapper, IAppDbTransactionContext contextTransaction)
        {
            _contextTransaction = contextTransaction;
            _httpContextAccessor = httpContextAccessor;
            _criterionGroupRepository = criterionGroupRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<CriteriaGroupResponse> CreateAsync(CriteriaGroupRequest request)
        {
            try
            {
                using (var uow = new AppUnitOfWork(_contextTransaction.GetDbContext()))
                {
                    RepositoryBaseWork<CriteriaGroup> criteriaRepo = new RepositoryBaseWork<CriteriaGroup>(uow);
                    using (var transaction = uow.BeginTrainsaction())
                    {
                        var criteriaGroup = _mapper.Map<CriteriaGroup>(request);
                        criteriaGroup.StartDate = DateTime.Now;
                        criteriaGroup.EndDate = DateTime.MaxValue;
                        criteriaGroup.RegisteredDate = DateTime.Now;
                        criteriaGroup.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                        criteriaGroup.RecordStatus = RecordStatus.Active;
                        criteriaGroup.IsReadOnly = false;
                        foreach (var criterion in request.Criteria)
                        {
                            var criteria = _mapper.Map<Criterion>(criterion);
                            criteria.CriteriaGroup = criteriaGroup;
                            criteria.StartDate = DateTime.Now;
                            criteria.EndDate = DateTime.MaxValue;
                            criteria.RegisteredDate = DateTime.Now;
                            criteria.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                            criteria.RecordStatus = RecordStatus.Active;
                            criteria.IsReadOnly = false;
                            criteriaGroup.Criteria.Add(criteria);
                        }
                        criteriaRepo.Add(criteriaGroup);
                        if (await uow.SaveChangesAsync()>0)
                        {
                            transaction.Commit();
                            return new CriteriaGroupResponse {
                                Response = _mapper.Map<CriteriaGroupDTO>(criteriaGroup),
                                Message = Resources.OperationSucessfullyCompleted, 
                                Status = OperationStatus.SUCCESS };
                        }
                        transaction.Rollback();
                        return new CriteriaGroupResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                    }

                }

            }
            catch (Exception ex)
            {
                return new CriteriaGroupResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }                
                
        }
        public async Task<OperationStatusResponse> Delete(long id)
        {

            try
            {
                var criterionGroup = await _criterionGroupRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (criterionGroup == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    criterionGroup.RecordStatus = RecordStatus.Deleted;
                    if (_criterionGroupRepository.Update(criterionGroup))
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
        public CriteriaGroupResponse GetById(long id)
        {
            try
            {
                var result = new CriteriaGroupResponse();
                var criterionGroup = _criterionGroupRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (criterionGroup == null)
                    return new CriteriaGroupResponse { Status = OperationStatus.EMPTY, Message = Resources.RecordDoesNotExist };
                var criterionGroupDTO = _mapper.Map<CriteriaGroupDTO>(criterionGroup);
                result.Response = criterionGroupDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new CriteriaGroupResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public CriteriaGroupsResponse GetByParentId(long id)
        {
            try
            {
                var result = new CriteriaGroupsResponse();
                var criterieaGroups = _criterionGroupRepository.Where(x => x.TechnicalEvaluationId == id && x.RecordStatus == RecordStatus.Active).Include(x=>x.Criteria).ToList();
                foreach (var criterieaGroup in criterieaGroups)
                {
                    if (criterieaGroups == null)
                        return new CriteriaGroupsResponse { Status = OperationStatus.EMPTY, Message = Resources.RecordDoesNotExist };
                    var criterieaGroupDTO = _mapper.Map<CriteriaGroupDTO>(criterieaGroup);
                    result.Response.Add(criterieaGroupDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                
                return new CriteriaGroupsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<CriteriaGroupResponse> Update(CriteriaGroupUpdateRequest request)
        {
            var criterionGroup = _criterionGroupRepository.Where(c => c.Id == request.Id).Include(x=>x.Criteria).FirstOrDefault();
            if (criterionGroup == null)
                return new CriteriaGroupResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                criterionGroup.GroupName = request.GroupName;
                criterionGroup.Sum = request.Sum;
                criterionGroup.TechnicalEvaluationId = request.TechnicalEvaluationId;
                criterionGroup.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                criterionGroup.LastUpdateDate = DateTime.UtcNow;
                if (_criterionGroupRepository.Update(criterionGroup))
                {
                    return new CriteriaGroupResponse
                    {
                        Response = _mapper.Map<CriteriaGroupDTO>(criterionGroup),
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new CriteriaGroupResponse
                    {
                        Response = _mapper.Map<CriteriaGroupDTO>(criterionGroup),
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new CriteriaGroupResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
