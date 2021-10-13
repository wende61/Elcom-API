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
    public class OfficeService : ICrud<OfficeResponse, OfficesResponse, OfficeRequest>, IBulkInsertion<OfficeResponse, OfficeRequest>
    {
        private readonly IRepositoryBase<Office> _officeRepository;
        private readonly IRepositoryBase<CostCenter> _costCenterRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbTransactionContext _appDbTransaction;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public OfficeService(IRepositoryBase<Office> officeRepository, ILoggerManager logger, IHttpContextAccessor httpContextAccessor, IMapper mapper, IAppDbTransactionContext appDbTransaction, IRepositoryBase<CostCenter> costCenterRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _officeRepository = officeRepository;
            _appDbTransaction = appDbTransaction;
            _costCenterRepository = costCenterRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<OfficeResponse> Create(OfficeRequest request)
        {
            try
            {
                var office = _mapper.Map<Office>(request);
                office.StartDate = DateTime.Now;
                office.EndDate = DateTime.MaxValue;
                office.RegisteredDate = DateTime.Now;
                office.RegisteredBy =  _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                office.RecordStatus = RecordStatus.Active;
                office.IsReadOnly = false;
                var existingOffice = _officeRepository.FirstOrDefaultAsync(x => x.OfficeName == office.OfficeName && x.CostCenterId == office.CostCenterId && x.RecordStatus == RecordStatus.Active);
                if (existingOffice.Result != null)
                    return new OfficeResponse { Message = "Office Already Existed", Status = OperationStatus.ERROR };
                if (_officeRepository.Add(office))
                    return new OfficeResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new OfficeResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new OfficeResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<OfficeResponse> BulkInsertion(List<OfficeRequest> requests)
        {
            try
            {
                using (var uow = new AppUnitOfWork(_appDbTransaction.GetDbContext()))
                {
                    RepositoryBaseWork<Office> officeRepo = new RepositoryBaseWork<Office>(uow);
                    using (var transaction= uow.BeginTrainsaction())
                    {
                        try
                        {
                            foreach (var request in requests)
                            {
                                var office = _mapper.Map<Office>(request);
                                office.StartDate = DateTime.Now;
                                office.EndDate = DateTime.MaxValue;
                                office.RegisteredDate = DateTime.Now;
                                office.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                office.RecordStatus = RecordStatus.Active;
                                office.IsReadOnly = false;
                                officeRepo.Add(office);
                            }
                            if (await uow.SaveChangesAsync()>0)
                            {
                                transaction.Commit();
                                return new OfficeResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                            }
                            transaction.Rollback();
                            return new OfficeResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return new OfficeResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new OfficeResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
        }

        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var office = await _officeRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (office == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    office.RecordStatus = RecordStatus.Deleted;
                    if (_officeRepository.Update(office))
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
        public OfficesResponse GetAll()
        {
            try
            {
                var result = new OfficesResponse();
                var Offices = _officeRepository.Where(x => x.RecordStatus == RecordStatus.Active);
                foreach (var office in Offices)
                {
                    var costCenter = _costCenterRepository.Find(office.CostCenterId);
                    var officeDTO = _mapper.Map<OfficeDTO>(office);
                    officeDTO.CostCenter = _mapper.Map<CostCenterDTO>(costCenter);
                    result.Response.Add(officeDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;

            }
            catch (Exception ex)
            {
                return new OfficesResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public OfficeResponse GetById(long id)
        {
            try
            {
                var result = new OfficeResponse();
                var office = _officeRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (office == null)
                    return new OfficeResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var costCenter = _costCenterRepository.Find(office.CostCenterId);
                var officeDTO = _mapper.Map<OfficeDTO>(office);
                officeDTO.CostCenter = _mapper.Map<CostCenterDTO>(costCenter);
                result.Response = officeDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new OfficeResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<OfficeResponse> Update(OfficeRequest request)
        {
            var office = _officeRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (office == null)
                return new OfficeResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                office.OfficeName = request.OfficeName;
                office.Description = request.Description;
                office.CostCenterId = request.CostCenterId;               
                office.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                office.LastUpdateDate = DateTime.UtcNow;
                if (_officeRepository.Update(office))
                {
                    return new OfficeResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new OfficeResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new OfficeResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
