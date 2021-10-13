using AutoMapper;
using EProcurement.DataObjects.Models.MasterData;
using EProcurement.Common;
using EProcurement.Common.RequestModel.MasterData;
using EProcurement.Common.ResponseModel.MasterData;
using EProcurement.Core.Interface.MasterData;
using EProcurement.DataObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Service.MasterData
{
    public class ProcurementSectionService : ICrud<ProcurementSectionResponse, ProcurementSectionsResponse, ProcurementSectionRequest>,IProcurementSection<ProcurementSectionsResponse>
    {
        private readonly IRepositoryBase<ProcurementSection> _procurementSectionRepository;
        private readonly IRepositoryBase<RequirmentPeriod> _rpRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public ProcurementSectionService(IRepositoryBase<ProcurementSection> procurementSectionRepository, IHttpContextAccessor httpContextAccessor, ILoggerManager logger, IMapper mapper, IRepositoryBase<RequirmentPeriod> rpRepository)
        {
            _procurementSectionRepository = procurementSectionRepository;
            _httpContextAccessor = httpContextAccessor;
            _rpRepository = rpRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ProcurementSectionResponse> Create(ProcurementSectionRequest request)
        {
            try
            {
                var procurementSection = _mapper.Map<ProcurementSection>(request);
                procurementSection.StartDate = DateTime.Now;
                procurementSection.EndDate = DateTime.MaxValue;
                procurementSection.RegisteredDate = DateTime.Now;
                procurementSection.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                procurementSection.RecordStatus = RecordStatus.Active;
                procurementSection.IsReadOnly = false;
                var existingPS = _procurementSectionRepository.FirstOrDefaultAsync(x => x.Section == procurementSection.Section && x.RequirmentPeriodId == procurementSection.RequirmentPeriodId && x.RecordStatus == RecordStatus.Active);
                if (existingPS.Result != null)
                    return new ProcurementSectionResponse { Message = "Procurmnet Section Already Existed", Status = OperationStatus.ERROR };
                if (_procurementSectionRepository.Add(procurementSection))
                    return new ProcurementSectionResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new ProcurementSectionResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new ProcurementSectionResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var procurementSection = await _procurementSectionRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (procurementSection == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    procurementSection.RecordStatus = RecordStatus.Deleted;
                    if (_procurementSectionRepository.Update(procurementSection))
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

        public ProcurementSectionsResponse GetAll()
        {
            try
            {
                var result = new ProcurementSectionsResponse();
                var procurementSections = _procurementSectionRepository.Where(x => x.RecordStatus == RecordStatus.Active);
                foreach (var procurementSection in procurementSections)
                {
                    var requirmentPeriod = _rpRepository.Find(procurementSection.RequirmentPeriodId);
                    var procurementSectionDTO = _mapper.Map<ProcurementSectionDTO>(procurementSection);
                    procurementSectionDTO.RequirmentPeriod = _mapper.Map<RequirmentPeriodDTO>(requirmentPeriod);
                    result.Response.Add(procurementSectionDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new ProcurementSectionsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public ProcurementSectionResponse GetById(long id)
        {
            try
            {
                var result = new ProcurementSectionResponse();
                var procurementSection = _procurementSectionRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (procurementSection == null)
                    return new ProcurementSectionResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var requirmentPeriod = _rpRepository.Find(procurementSection.RequirmentPeriodId);
                var procurementSectionDTO = _mapper.Map<ProcurementSectionDTO>(procurementSection);
                procurementSectionDTO.RequirmentPeriod = _mapper.Map<RequirmentPeriodDTO>(requirmentPeriod);
                result.Response = procurementSectionDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new ProcurementSectionResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public ProcurementSectionsResponse GetByRequirementPeriodId(long id)
        {
            try
            {
                var result = new ProcurementSectionsResponse();
                var procurementSections = _procurementSectionRepository.Where(x =>x.RequirmentPeriodId==id && x.RecordStatus == RecordStatus.Active);
                foreach (var procurementSection in procurementSections)
                {
                    var requirmentPeriod = _rpRepository.Find(procurementSection.RequirmentPeriodId);
                    var procurementSectionDTO = _mapper.Map<ProcurementSectionDTO>(procurementSection);
                    procurementSectionDTO.RequirmentPeriod = _mapper.Map<RequirmentPeriodDTO>(requirmentPeriod);
                    result.Response.Add(procurementSectionDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new ProcurementSectionsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<ProcurementSectionResponse> Update(ProcurementSectionRequest request)
        {
            var procurementSection = _procurementSectionRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (procurementSection == null)
                return new ProcurementSectionResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                procurementSection.Section = request.Section;
                procurementSection.Description = request.Description;
                procurementSection.RequirmentPeriodId = request.RequirmentPeriodId;
                procurementSection.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                procurementSection.LastUpdateDate = DateTime.UtcNow;
                if (_procurementSectionRepository.Update(procurementSection))
                {
                    return new ProcurementSectionResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new ProcurementSectionResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new ProcurementSectionResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
