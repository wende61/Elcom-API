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
    public class CostCenterService : ICrud<CostCenterResponse, CostCentersResponse, CostCenterRequest>, IBulkInsertion<CostCenterResponse, CostCenterRequest>
    {
        private readonly IRepositoryBase<CostCenter> _costCenterRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbTransactionContext _appDbTransaction;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CostCenterService(IRepositoryBase<CostCenter> costCenterRepository, ILoggerManager logger, IHttpContextAccessor httpContextAccessor, IMapper mapper, IAppDbTransactionContext appDbTransaction)
        {
            _httpContextAccessor = httpContextAccessor;
            _costCenterRepository = costCenterRepository;
            _appDbTransaction = appDbTransaction;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CostCenterResponse> Create(CostCenterRequest request)
        {
            try
            {

                var costCenter = _mapper.Map<CostCenter>(request);
                costCenter.StartDate = DateTime.Now;
                costCenter.EndDate = DateTime.MaxValue;
                costCenter.RegisteredDate = DateTime.Now;
                costCenter.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                costCenter.RecordStatus = RecordStatus.Active;
                costCenter.IsReadOnly = false;
                var existingCostCenter = _costCenterRepository.FirstOrDefaultAsync(x => x.CostCenterName == costCenter.CostCenterName && x.Station == costCenter.Station && x.RecordStatus == RecordStatus.Active);
                if (existingCostCenter.Result != null)
                    return new CostCenterResponse { Message = "Cost-Center Already Existed", Status = OperationStatus.ERROR };
                if (_costCenterRepository.Add(costCenter))
                    return new CostCenterResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new CostCenterResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new CostCenterResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<CostCenterResponse> BulkInsertion(List<CostCenterRequest> requests)
        {
            try
            {
                using (var uow = new AppUnitOfWork(_appDbTransaction.GetDbContext()))
                {
                    RepositoryBaseWork<CostCenter> costCenterRepo = new RepositoryBaseWork<CostCenter>(uow);
                    using (var transaction = uow.BeginTrainsaction())
                    {
                        try
                        {
                            foreach (var request in requests)
                            {
                                var costCenter = _mapper.Map<CostCenter>(request);
                                costCenter.StartDate = DateTime.Now;
                                costCenter.EndDate = DateTime.MaxValue;
                                costCenter.RegisteredDate = DateTime.Now;
                                costCenter.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                costCenter.RecordStatus = RecordStatus.Active;
                                costCenter.IsReadOnly = false;
                                costCenterRepo.Add(costCenter);
                            }
                            if (await uow.SaveChangesAsync()>0)
                            {
                                transaction.Commit();
                                return new CostCenterResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                            }
                            transaction.Rollback();
                            return new CostCenterResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return new CostCenterResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new CostCenterResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var costCenter = await _costCenterRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (costCenter == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    costCenter.RecordStatus = RecordStatus.Deleted;
                    if (_costCenterRepository.Update(costCenter))
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

        public CostCentersResponse GetAll()
        {
            try
            {
                var result = new CostCentersResponse();
                var costCenters = _costCenterRepository.Where(x => x.RecordStatus == RecordStatus.Active);
                foreach (var costCenter in costCenters)
                {
                    var costCenterDOT = _mapper.Map<CostCenterDTO>(costCenter);
                    result.Response.Add(costCenterDOT);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;

            }
            catch (Exception ex)
            {
                return new CostCentersResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public CostCenterResponse GetById(long id)
        {
            try
            {
                var result = new CostCenterResponse();
                var costCenter = _costCenterRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (costCenter == null)
                    return new CostCenterResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var costCenterDOT = _mapper.Map<CostCenterDTO>(costCenter);
                result.Response = costCenterDOT;
                result.Status = OperationStatus.SUCCESS;    
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new CostCenterResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<CostCenterResponse> Update(CostCenterRequest request)
        {
            var costCenter = _costCenterRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (costCenter == null)
                return new CostCenterResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                costCenter.Country = request.Country;
                costCenter.Station = request.Station;
                costCenter.CostCenterName = request.CostCenterName;               
                costCenter.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                costCenter.LastUpdateDate = DateTime.UtcNow;
                if (_costCenterRepository.Update(costCenter))
                {
                    return new CostCenterResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new CostCenterResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new CostCenterResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
