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
    public class HotelEvaluationService : IHotelEvaluation<HotelAccommodationCriteriaRequest, HotelAccommodationCriteriaResponse, HotelAccommodationCriteriasResponse>
    {
        private readonly IRepositoryBase<HotelAccommodationCriteria> _hotelAccommodationCriteriaRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbTransactionContext _appDbTransaction;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public HotelEvaluationService(IRepositoryBase<HotelAccommodationCriteria> hotelAccommodationCriteriaRepository, IHttpContextAccessor httpContextAccessor, IAppDbTransactionContext appDbTransaction, ILoggerManager logger, IMapper mapper)
        {
            _hotelAccommodationCriteriaRepository = hotelAccommodationCriteriaRepository;
            _httpContextAccessor = httpContextAccessor;
            _appDbTransaction = appDbTransaction;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<HotelAccommodationCriteriaResponse> Create(HotelAccommodationCriteriaRequest request)
        {
            try
            {
                var hotelAccommodationCriteria = _mapper.Map<HotelAccommodationCriteria>(request);
                hotelAccommodationCriteria.StartDate = DateTime.Now;
                hotelAccommodationCriteria.EndDate = DateTime.MaxValue;
                hotelAccommodationCriteria.RegisteredDate = DateTime.Now;
                hotelAccommodationCriteria.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                hotelAccommodationCriteria.RecordStatus = RecordStatus.Active;
                hotelAccommodationCriteria.IsReadOnly = false;
                hotelAccommodationCriteria.IsReadOnly = false;
                if (_hotelAccommodationCriteriaRepository.Add(hotelAccommodationCriteria))
                    return new HotelAccommodationCriteriaResponse {
                        Response = _mapper.Map<HotelAccommodationCriteriaDTO>(hotelAccommodationCriteria),
                        Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new HotelAccommodationCriteriaResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new HotelAccommodationCriteriaResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var hotelAccommodationCriteria = await _hotelAccommodationCriteriaRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (hotelAccommodationCriteria == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
                else
                {
                    if (_hotelAccommodationCriteriaRepository.Remove(hotelAccommodationCriteria))
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

        public  HotelAccommodationCriteriaResponse GetById(long id)
        {          
            try
            {
                var result = new HotelAccommodationCriteriaResponse();
                var hotelAccommodationCriteria = _hotelAccommodationCriteriaRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active)
                    .Include(x=>x.Project).FirstOrDefault();
                if(hotelAccommodationCriteria==null)
                    return new HotelAccommodationCriteriaResponse { Status = OperationStatus.EMPTY, Message = Resources.RecordDoesNotExist };
                var hotelAccommodationCriteriaDTO = _mapper.Map<HotelAccommodationCriteriaDTO>(hotelAccommodationCriteria);
                result.Response = hotelAccommodationCriteriaDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new HotelAccommodationCriteriaResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public HotelAccommodationCriteriaResponse GetByParentId(long id)
        {
            try
            {
                var result = new HotelAccommodationCriteriaResponse();
                var hotelAccommodationCriteria = _hotelAccommodationCriteriaRepository.Where(x => x.ProjectId == id && x.RecordStatus == RecordStatus.Active)
                    .Include(x => x.Project).FirstOrDefault();
                if (hotelAccommodationCriteria == null)
                    return new HotelAccommodationCriteriaResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };

                var hotelAccommodationCriteriaDTO = _mapper.Map<HotelAccommodationCriteriaDTO>(hotelAccommodationCriteria);
                result.Response = hotelAccommodationCriteriaDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;

            }
            catch (Exception ex)
            {
                return new HotelAccommodationCriteriaResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<HotelAccommodationCriteriaResponse> Update(HotelAccommodationCriteriaRequest request)
        {
            var hotelAccommodationCriteria = _hotelAccommodationCriteriaRepository.Where(x => x.Id == request.Id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
            if (hotelAccommodationCriteria==null)
                return new HotelAccommodationCriteriaResponse { Status = OperationStatus.EMPTY, Message = Resources.RecordDoesNotExist };
            try
            {
                hotelAccommodationCriteria.DailyRoomNumber = request.DailyRoomNumber;
                hotelAccommodationCriteria.WeeklyFrequency = request.WeeklyFrequency;
                hotelAccommodationCriteria.YearlyFrequency = request.YearlyFrequency;
                hotelAccommodationCriteria.ProjectId = request.ProjectId;
                if (_hotelAccommodationCriteriaRepository.Update(hotelAccommodationCriteria))
                {
                    return new HotelAccommodationCriteriaResponse
                    {
                        Response= _mapper.Map<HotelAccommodationCriteriaDTO>(hotelAccommodationCriteria),
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new HotelAccommodationCriteriaResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }


            }
            catch (Exception ex)
            {
                return new HotelAccommodationCriteriaResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
