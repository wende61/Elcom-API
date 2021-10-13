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
    public class CountryService : ICrud<CountryResponse, CountriesResponse, CountryRequest>
    {

        private readonly IRepositoryBase<Country> _countryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CountryService(IRepositoryBase<Country> countryRepository, ILoggerManager logger, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _countryRepository = countryRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CountryResponse> Create(CountryRequest request)
        {
            try
            {
                var country = _mapper.Map<Country>(request);
                country.StartDate = DateTime.Now;
                country.EndDate = DateTime.MaxValue;
                country.RegisteredDate = DateTime.Now;
                country.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                country.RecordStatus = RecordStatus.Active;
                country.IsReadOnly = false;
                var existingSupplier = _countryRepository.FirstOrDefaultAsync(x => x.ShortName == country.ShortName && x.CountryName == country.CountryName && x.RecordStatus == RecordStatus.Active);
                if (existingSupplier.Result != null)
                    return new CountryResponse { Message = "Country Already Existed", Status = OperationStatus.ERROR };
                if (_countryRepository.Add(country))
                    return new CountryResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new CountryResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new CountryResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var country = await _countryRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (country == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    country.RecordStatus = RecordStatus.Deleted;
                    if (_countryRepository.Update(country))
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
        public CountriesResponse GetAll()
        {
            try
            {
                var result = new CountriesResponse();
                var countries = _countryRepository.Where(x => x.RecordStatus == RecordStatus.Active);
                foreach (var country in countries)
                {
                    var countryDOT = _mapper.Map<CountryDTO>(country);
                    result.Response.Add(countryDOT);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;

            }
            catch (Exception ex)
            {
                return new CountriesResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public CountryResponse GetById(long id)
        {
            try
            {
                var result = new CountryResponse();
                var country = _countryRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (country == null)
                    return new CountryResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var countryDOT = _mapper.Map<CountryDTO>(country);
                result.Response = countryDOT;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new CountryResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<CountryResponse> Update(CountryRequest request)
        {
            var country = _countryRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (country == null)
                return new CountryResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                country.ShortName = request.ShortName;
                country.CountryName = request.CountryName;
                country.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                country.LastUpdateDate = DateTime.UtcNow;
                if (_countryRepository.Update(country))
                {
                    return new CountryResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new CountryResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new CountryResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
