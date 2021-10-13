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
    public class StationService : ICrud<StationResponse, StationsResponse, StationRequest>, IBulkInsertion<StationResponse, StationRequest>
    {
        private readonly IRepositoryBase<Station> _stationRepository;
        private readonly IRepositoryBase<Country> _countryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbTransactionContext _appDbTransaction;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public StationService(IRepositoryBase<Station> stationRepository, IHttpContextAccessor httpContextAccessor, ILoggerManager logger ,IMapper mapper, IAppDbTransactionContext appDbTransaction, IRepositoryBase<Country> countryRepository)
        {
            _stationRepository = stationRepository;
            _httpContextAccessor = httpContextAccessor;
            _countryRepository = countryRepository;
            _appDbTransaction = appDbTransaction;
            _logger = logger;
            _mapper = mapper;
        }

        //
        public async Task<StationResponse> Create(StationRequest request)
        {
            try
            {
                var station = _mapper.Map<Station>(request);
                station.StartDate = DateTime.Now;
                station.EndDate = DateTime.MaxValue;
                station.RegisteredDate = DateTime.Now;
                station.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                station.RecordStatus = RecordStatus.Active;
                station.IsReadOnly = false;
                var existingStation = _stationRepository.FirstOrDefaultAsync(x => x.CityCode == station.CityCode && x.CityName == station.CityName && x.RecordStatus == RecordStatus.Active);
                if (existingStation.Result != null)
                    return new StationResponse { Message = "Station Already Existed", Status = OperationStatus.ERROR };
                if (_stationRepository.Add(station))
                    return new StationResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new StationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new StationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<StationResponse> BulkInsertion(List<StationRequest> requests)
        {
            try
            {
                using (var uow = new AppUnitOfWork(_appDbTransaction.GetDbContext()))
                {
                    RepositoryBaseWork<Station> stationRepo = new RepositoryBaseWork<Station>(uow);
                    using (var transaction = uow.BeginTrainsaction())
                    {
                        try
                        {
                            foreach (var request in requests)
                            {
                                var station = _mapper.Map<Station>(request);
                                station.StartDate = DateTime.Now;
                                station.EndDate = DateTime.MaxValue;
                                station.RegisteredDate = DateTime.Now;
                                station.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                station.RecordStatus = RecordStatus.Active;
                                station.IsReadOnly = false;
                                stationRepo.Add(station);
                            }
                            if (await uow.SaveChangesAsync()>0)
                            {
                                transaction.Commit();
                                return new StationResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                            }
                            transaction.Rollback();
                            return new StationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return new StationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                    }

                }
            }
            catch (Exception e)
            {
                return new StationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var station = await _stationRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (station == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    station.RecordStatus = RecordStatus.Deleted;
                    if (_stationRepository.Update(station))
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

        public StationsResponse GetAll()
        {
            try
            {
                var result = new StationsResponse();
                var stations = _stationRepository.Where(x => x.RecordStatus == RecordStatus.Active);
                foreach (var station in stations)
                {
                    var country = _countryRepository.Find(station.CountryId);
                    var stationDOT = _mapper.Map<StationDTO>(station);
                    stationDOT.Country = _mapper.Map<CountryDTO>(country);
                    result.Response.Add(stationDOT);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;

            }
            catch (Exception ex)
            {
                return new StationsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public StationResponse GetById(long id)
        {
            try
            {
                var result = new StationResponse();
                var station = _stationRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (station == null)
                    return new StationResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var country = _countryRepository.Find(station.CountryId);
                var stationDOT = _mapper.Map<StationDTO>(station);
                stationDOT.Country = _mapper.Map<CountryDTO>(country);
                result.Response = stationDOT;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new StationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<StationResponse> Update(StationRequest request)
        {
            var station = _stationRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (station == null)
                return new StationResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                station.CityName = request.CityName;
                station.CityCode = request.CityCode;
                station.CountryId = request.CountryId;
                station.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                station.LastUpdateDate = DateTime.UtcNow;
                if (_stationRepository.Update(station))
                {
                    return new StationResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new StationResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new StationResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
