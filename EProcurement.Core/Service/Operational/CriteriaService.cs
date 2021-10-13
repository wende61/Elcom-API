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
    public class CriteriaService : ICrud<CriterionResponse, CriterionsResponse, CriterionRequest, CriterionRequest>
    {
        private readonly IRepositoryBase<Criterion> _criterionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CriteriaService(IRepositoryBase<Criterion> criterionRepository, IHttpContextAccessor httpContextAccessor, ILoggerManager logger, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _criterionRepository = criterionRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<CriterionResponse> CreateAsync(CriterionRequest request)
        {
            try
            {
                var criterion = _mapper.Map<Criterion>(request);
                criterion.StartDate = DateTime.Now;
                criterion.EndDate = DateTime.MaxValue;
                criterion.RegisteredDate = DateTime.Now;
                criterion.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                criterion.RecordStatus = RecordStatus.Active;
                criterion.IsReadOnly = false;
                if (_criterionRepository.Add(criterion))
                    return new CriterionResponse {
                        Response = _mapper.Map<CriterionDTO>(criterion),
                        Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new CriterionResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new CriterionResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> Delete(long id)
        {

            try
            {
                var criterion = await _criterionRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (criterion == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    criterion.RecordStatus = RecordStatus.Deleted;
                    if (_criterionRepository.Update(criterion))
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
        public CriterionResponse GetById(long id)
        {
            try
            {
                var result = new CriterionResponse();
                var criterion = _criterionRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (criterion == null)
                    return new CriterionResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var countryDOT = _mapper.Map<CriterionDTO>(criterion);
                result.Response = countryDOT;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new CriterionResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public CriterionsResponse GetByParentId(long id)
        {
            try
            {
                var result = new CriterionsResponse();
                var criterias = _criterionRepository.Where(x => x.CriteriaGroupId == id && x.RecordStatus == RecordStatus.Active).ToList();
                if (criterias == null)
                    return new CriterionsResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                foreach (var criteria in criterias)
                {
                    var criteriaDTO = _mapper.Map<CriterionDTO>(criteria);
                    result.Response.Add(criteriaDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new CriterionsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<CriterionResponse> Update(CriterionRequest request)
        {
            var criterion = _criterionRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (criterion == null)
                return new CriterionResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                criterion.Title = request.Title;
                criterion.Measurment = request.Measurment;
                criterion.Value = request.Value;
                criterion.Necessity = request.Necessity;
                criterion.CriteriaGroupId = request.CriteriaGroupID;
                criterion.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                criterion.LastUpdateDate = DateTime.UtcNow;
                if (_criterionRepository.Update(criterion))
                {
                    return new CriterionResponse
                    {
                        Response = _mapper.Map<CriterionDTO>(criterion),
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new CriterionResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new CriterionResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
