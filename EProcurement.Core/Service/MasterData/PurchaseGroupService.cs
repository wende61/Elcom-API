using System;
using EProcurement.DataObjects.Models.MasterData;
using System.Collections.Generic;
using System.Text;
using EProcurement.Common;
using Microsoft.AspNetCore.Http;
using EProcurement.DataObjects;
using AutoMapper;
using EProcurement.Core.Interface.MasterData;
using EProcurement.Common.RequestModel.MasterData;
using EProcurement.Common.ResponseModel.MasterData;
using System.Threading.Tasks;
using System.Linq;

namespace EProcurement.Core.Service.MasterData
{
    public class PurchaseGroupService : ICrud<PurchaseGroupResponse,PurchaseGroupsResponse,PurchaseGroupRequest>
    {
        private readonly IRepositoryBase<PurchaseGroup> _stationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public PurchaseGroupService(IRepositoryBase<PurchaseGroup> stationRepository, IHttpContextAccessor httpContextAccessor, ILoggerManager logger, IMapper mapper)
        {
            _stationRepository = stationRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PurchaseGroupResponse> Create(PurchaseGroupRequest request)
        {
            try
            {
                var purchaseGroup = _mapper.Map<PurchaseGroup>(request);
                purchaseGroup.StartDate = DateTime.Now;
                purchaseGroup.EndDate = DateTime.MaxValue;
                purchaseGroup.RegisteredDate = DateTime.Now;
                purchaseGroup.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                purchaseGroup.RecordStatus = RecordStatus.Active;
                purchaseGroup.IsReadOnly = false;
                var existingStation = _stationRepository.FirstOrDefaultAsync(x => x.Group == purchaseGroup.Group  && x.RecordStatus == RecordStatus.Active);
                if (existingStation.Result != null)
                    return new PurchaseGroupResponse { Message = "Purchas Group Already Existed", Status = OperationStatus.ERROR };
                if (_stationRepository.Add(purchaseGroup))
                    return new PurchaseGroupResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new PurchaseGroupResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new PurchaseGroupResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var purchaseGroup = await _stationRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (purchaseGroup == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    purchaseGroup.RecordStatus = RecordStatus.Deleted;
                    if (_stationRepository.Update(purchaseGroup))
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

        public PurchaseGroupsResponse GetAll()
        {
            try
            {
                var result = new PurchaseGroupsResponse();
                var purchaseGroup = _stationRepository.Where(x => x.RecordStatus == RecordStatus.Active);
                foreach (var station in purchaseGroup)
                {
                    var purchaseGroupDTO = _mapper.Map<PurchaseGroupDTO>(station);
                    result.Response.Add(purchaseGroupDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new PurchaseGroupsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public PurchaseGroupResponse GetById(long id)
        {
            try
            {
                var result = new PurchaseGroupResponse();
                var purchaseGroup = _stationRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (purchaseGroup == null)
                    return new PurchaseGroupResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var purchaseGroupDTO = _mapper.Map<PurchaseGroupDTO>(purchaseGroup);
                result.Response = purchaseGroupDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new PurchaseGroupResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<PurchaseGroupResponse> Update(PurchaseGroupRequest request)
        {
            var purchaseGroup = _stationRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (purchaseGroup == null)
                return new PurchaseGroupResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                purchaseGroup.Group = request.Group;
                purchaseGroup.Description = request.Description;
                purchaseGroup.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                purchaseGroup.LastUpdateDate = DateTime.UtcNow;
                if (_stationRepository.Update(purchaseGroup))
                {
                    return new PurchaseGroupResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new PurchaseGroupResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new PurchaseGroupResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
