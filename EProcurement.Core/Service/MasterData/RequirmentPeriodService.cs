using EProcurement.DataObjects.Models.MasterData;
using EProcurement.DataObjects;
using Microsoft.AspNetCore.Http;
using EProcurement.Common;
using AutoMapper;
using EProcurement.DataObjects.Models.MasterData;
using EProcurement.Common.RequestModel.MasterData;
using EProcurement.Core.Interface.MasterData;
using EProcurement.Common.ResponseModel.MasterData;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

namespace EProcurement.Core.Service.MasterData
{
    public class RequirmentPeriodService:ICrud<RequirmentPeriodResponse, RequirmentPeriodsResponse, RequirmentPeriodRequest>,IRequirementPeriod<RequirmentPeriodsResponse>
    {
        private readonly IRepositoryBase<RequirmentPeriod> _requirmentPRepository;
        private readonly IRepositoryBase<PurchaseGroup> _pgRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public RequirmentPeriodService(IRepositoryBase<RequirmentPeriod> requirmentPRepository, IHttpContextAccessor httpContextAccessor, ILoggerManager logger, IMapper mapper, IRepositoryBase<PurchaseGroup> pgRepository)
        {
            _requirmentPRepository = requirmentPRepository;
            _httpContextAccessor = httpContextAccessor;
            _pgRepository = pgRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<RequirmentPeriodResponse> Create(RequirmentPeriodRequest request)
        {
            try
            {
                var requirmentPeriod = _mapper.Map<RequirmentPeriod>(request);
                requirmentPeriod.StartDate = DateTime.Now;
                requirmentPeriod.EndDate = DateTime.MaxValue;
                requirmentPeriod.RegisteredDate = DateTime.Now;
                requirmentPeriod.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                requirmentPeriod.RecordStatus = RecordStatus.Active;
                requirmentPeriod.IsReadOnly = false;
                var existingrp = _requirmentPRepository.FirstOrDefaultAsync(x => x.Period == requirmentPeriod.Period && x.RecordStatus == RecordStatus.Active);
                if (existingrp.Result != null)
                    return new RequirmentPeriodResponse { Message = "RequirmentPeriod Already Existed", Status = OperationStatus.ERROR };
                if (_requirmentPRepository.Add(requirmentPeriod))
                    return new RequirmentPeriodResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new RequirmentPeriodResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new RequirmentPeriodResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var requirmentPeriod = await _requirmentPRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (requirmentPeriod == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    requirmentPeriod.RecordStatus = RecordStatus.Deleted;
                    if (_requirmentPRepository.Update(requirmentPeriod))
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

        public RequirmentPeriodsResponse GetAll()
        {
            try
            {
                var result = new RequirmentPeriodsResponse();
                var requirmentPeriods = _requirmentPRepository.Where(x => x.RecordStatus == RecordStatus.Active);
                foreach (var requirmentPeriod in requirmentPeriods)
                {
                    var purchaseGroup = _pgRepository.Find(requirmentPeriod.PurchaseGroupId);
                    var requirmentPeriodDTO = _mapper.Map<RequirmentPeriodDTO>(requirmentPeriod);
                    requirmentPeriodDTO.PurchaseGroup = _mapper.Map<PurchaseGroupDTO>(purchaseGroup);
                    result.Response.Add(requirmentPeriodDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new RequirmentPeriodsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public RequirmentPeriodResponse GetById(long id)
        {
            try
            {
                var result = new RequirmentPeriodResponse();
                var requirmentPeriod = _requirmentPRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (requirmentPeriod == null)
                    return new RequirmentPeriodResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var purchaseGroup = _pgRepository.Find(requirmentPeriod.PurchaseGroupId);
                var requirmentPeriodDTO = _mapper.Map<RequirmentPeriodDTO>(requirmentPeriod);
                requirmentPeriodDTO.PurchaseGroup = _mapper.Map<PurchaseGroupDTO>(purchaseGroup);
                result.Response = requirmentPeriodDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new RequirmentPeriodResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public RequirmentPeriodsResponse GetByPurchaseGroupId(long id)
        {
            try
            {
                var result = new RequirmentPeriodsResponse();
                var requirmentPeriods = _requirmentPRepository.Where(x =>x.PurchaseGroupId==id && x.RecordStatus == RecordStatus.Active);
                foreach (var requirmentPeriod in requirmentPeriods)
                {
                    var purchaseGroup = _pgRepository.Find(requirmentPeriod.PurchaseGroupId);
                    var requirmentPeriodDTO = _mapper.Map<RequirmentPeriodDTO>(requirmentPeriod);
                    requirmentPeriodDTO.PurchaseGroup = _mapper.Map<PurchaseGroupDTO>(purchaseGroup);
                    result.Response.Add(requirmentPeriodDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new RequirmentPeriodsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<RequirmentPeriodResponse> Update(RequirmentPeriodRequest request)
        {
            var requirmentPeriod = _requirmentPRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (requirmentPeriod == null)
                return new RequirmentPeriodResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                requirmentPeriod.Period = request.Period;
                requirmentPeriod.Description = request.Description;
                requirmentPeriod.PurchaseGroupId = request.PurchaseGroupId;
                requirmentPeriod.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                requirmentPeriod.LastUpdateDate = DateTime.UtcNow;
                if (_requirmentPRepository.Update(requirmentPeriod))
                {
                    return new RequirmentPeriodResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new RequirmentPeriodResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new RequirmentPeriodResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
