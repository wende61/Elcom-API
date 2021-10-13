using AutoMapper;
using EProcurement.DataObjects.Models.Operational;
using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.MasterData;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Helper;
using EProcurement.Core.Interface.Operational;
using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.MasterData;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EProcurement.Core.Service.Operational
{
    public class HotelAccommodationService : IHotelAccommodation<HotelAccommodationResponse, HotelAccommodationsResponse, HotelAccommodationRequest>, IRequest<RejectRequest, AssignRequest, SelfAssignRequest, HotelAccommodationResponse>
    {
        private readonly IRepositoryBase<HotelAccommodation> _haRepository;
        private readonly IRepositoryBase<Country> _countryRepository;
        private readonly IRepositoryBase<Station> _stationRepository;
        private readonly IRepositoryBase<CostCenter> _costCenterRepository;
        private readonly IRepositoryBase<Person> _personRepository;
        private readonly IRepositoryBase<Project> _projectRepository;
        private readonly IRepositoryBase<HARDelegateTeam> _harDelegateTeamRepository;
        private readonly IRepositoryBase<HARApprover> _harApproversRepository;
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IAppDbTransactionContext _appTransaction;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerManager _logger;
        private readonly IApprovalDocument _approvalDocument;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly FileSettings fileSettings;
        public HotelAccommodationService(IRepositoryBase<HotelAccommodation> harRepository, IRepositoryBase<HARDelegateTeam> harDelegateTeamRepository, IConfiguration config,
             IRepositoryBase<HARApprover> harApproversRepository, IRepositoryBase<User> userRepository, ILoggerManager logger, IHttpContextAccessor httpContextAccessor, IMapper mapper,
             IAppDbTransactionContext appTransaction, IUserService userService, IRepositoryBase<Project> projectRepository, IRepositoryBase<Country> countryRepository,
          IRepositoryBase<Station> stationRepository, IRepositoryBase<CostCenter> costCenterRepository, IRepositoryBase<Person> personRepository, IApprovalDocument approvalDocument

            )
        {
            _haRepository = harRepository;
            _harDelegateTeamRepository = harDelegateTeamRepository;
            _harApproversRepository = harApproversRepository;
            _httpContextAccessor = httpContextAccessor;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _appTransaction = appTransaction;
            _countryRepository = countryRepository;
            _stationRepository = stationRepository;
            _approvalDocument = approvalDocument;
            _costCenterRepository = costCenterRepository;
            _personRepository = personRepository;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
            var fileSettingsSection = config.GetSection("FileSettings");
            fileSettings = fileSettingsSection.Get<FileSettings>();
        }

        public async Task<HotelAccommodationResponse> Assign(AssignRequest request)
        {
            var hotelAccomodation = _haRepository.Where(c => c.Id == request.RequestId).FirstOrDefault();
            if (hotelAccomodation == null)
                return new HotelAccommodationResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                hotelAccomodation.AssignedTo = request.PersonId;
                if (hotelAccomodation.PRStatus == PRStatus.UnAssigned || hotelAccomodation.PRStatus == null)
                {
                    hotelAccomodation.AssignedTo = request.PersonId;
                    hotelAccomodation.AssignRemark = request.Remark;
                    hotelAccomodation.PRStatus = PRStatus.Assigned;
                }
                else if (hotelAccomodation.PRStatus == PRStatus.Assigned)
                {
                    var project = _projectRepository.Where(x => x.HotelAccommodationId == hotelAccomodation.Id && x.AssignedPerson != null && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                    if (project != null)
                    {
                        project.AssignedPerson = request.PersonId;
                        _projectRepository.Update(project);
                    }
                    hotelAccomodation.AssignedTo = request.PersonId;
                    hotelAccomodation.PRStatus = PRStatus.ReAssigned;
                    hotelAccomodation.ReAssignRemark = request.Remark;
                }
                hotelAccomodation.AssignedBy = person.Id;
                hotelAccomodation.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                hotelAccomodation.LastUpdateDate = DateTime.UtcNow;
                if (_haRepository.Update(hotelAccomodation))
                {
                    return new HotelAccommodationResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new HotelAccommodationResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new HotelAccommodationResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<HotelAccommodationResponse> Create(HotelAccommodationRequest request, List<HotelARApproversRequest> approvers, List<HotelARDelegateTeamRequest> delegateTeams)
        {
            try
            {
                var response = FileUploader.Upload(request.SpecificationFile, fileSettings.StoredFilesPath, "1", fileSettings.FileSizeLimit);
                if (response.Result)
                {
                    using (var uow = new AppUnitOfWork(_appTransaction.GetDbContext()))
                    {
                        var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                        RepositoryBaseWork<HotelAccommodation> harRepo = new RepositoryBaseWork<HotelAccommodation>(uow);
                        RepositoryBaseWork<HARApprover> harApproverRepo = new RepositoryBaseWork<HARApprover>(uow);
                        RepositoryBaseWork<HARDelegateTeam> harDelegateTeamRepo = new RepositoryBaseWork<HARDelegateTeam>(uow);
                        using (var transaction = uow.BeginTrainsaction())
                        {
                            try
                            {
                                var person = _userService.GetPerson(userName);
                                var newHotelAccommodation = _mapper.Map<HotelAccommodation>(request);
                                newHotelAccommodation.Approvers = new List<HARApprover>();
                                newHotelAccommodation.DelegateTeams = new List<HARDelegateTeam>();                                //
                                newHotelAccommodation.AttachementPath = response.Path;
                                newHotelAccommodation.ApprovalStatus = ApprovalStatus.Pending;
                                newHotelAccommodation.PRStatus = PRStatus.UnAssigned; // requested by
                                newHotelAccommodation.RequestDate = DateTime.Now;
                                newHotelAccommodation.RequestedBy = person.Id;
                                newHotelAccommodation.RegisteredBy = userName;
                                newHotelAccommodation.StartDate = DateTime.Now;
                                newHotelAccommodation.EndDate = DateTime.MaxValue;
                                newHotelAccommodation.RegisteredDate = DateTime.Now;
                                newHotelAccommodation.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                newHotelAccommodation.RecordStatus = RecordStatus.Active;
                                foreach (var delegateTeam in delegateTeams)
                                {
                                    var harDelegateTeam = new HARDelegateTeam();
                                    harDelegateTeam.PersonId = delegateTeam.PersonId;
                                    harDelegateTeam.HotelAccommodationId = newHotelAccommodation.Id;
                                    harDelegateTeam.StartDate = DateTime.Now;
                                    harDelegateTeam.EndDate = DateTime.MaxValue;
                                    harDelegateTeam.RegisteredDate = DateTime.Now;
                                    harDelegateTeam.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                    harDelegateTeam.RecordStatus = RecordStatus.Active;
                                    newHotelAccommodation.DelegateTeams.Add(harDelegateTeam);
                                }
                                foreach (var approver in approvers)
                                {
                                    var harApprover = new HARApprover();
                                    harApprover.PersonId = approver.PersonId;
                                    harApprover.Order = approver.Order;
                                    harApprover.HotelAccommodationId = newHotelAccommodation.Id;
                                    harApprover.ApprovalStatus = ApprovalStatus.Pending;
                                    harApprover.StartDate = DateTime.Now;
                                    harApprover.EndDate = DateTime.MaxValue;
                                    harApprover.RegisteredDate = DateTime.Now;
                                    harApprover.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                    harApprover.RecordStatus = RecordStatus.Active;
                                    newHotelAccommodation.Approvers.Add(harApprover);
                                }
                                harRepo.Add(newHotelAccommodation);
                                if (await uow.SaveChangesAsync() > 0)
                                {
                                    transaction.Commit();
                                    return new HotelAccommodationResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                                    //if (_approvalDocument.GenerateHotelAccommodationApproval(request, response.Path, person.Id))
                                    //{
                                    //    transaction.Commit();
                                    //    return new HotelAccommodationResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                                    //}
                                    //transaction.Rollback();
                                    //return new HotelAccommodationResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR, BusinessErrorCode = "Unable to create Approvers" };
                                }
                                transaction.Rollback();
                                return new HotelAccommodationResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR, BusinessErrorCode = "Unable to create Approvers" };
                            }
                            catch (Exception ex)
                            {
                                return new HotelAccommodationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                            }
                        }
                    }
                }
                return new HotelAccommodationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception)
            {
                return new HotelAccommodationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public HotelAccommodationsResponse GetAll()
        {
            try
            {
                var result = new HotelAccommodationsResponse();
                var hotelAccommodations = _haRepository.Where(x => x.RecordStatus == RecordStatus.Active)
                     .OrderByDescending(x => x.RegisteredDate)
                    .Include(x => x.Country)
                    .Include(x => x.Station)
                    .Include(x => x.CostCenter)
                    .Include(x => x.AssignedAgent)
                    .Include(x => x.Assigner)
                    .Include(x => x.Rejector)
                    .Include(x => x.Requester)
                    .ToList();
                foreach (var hotelAccommodation in hotelAccommodations)
                {
                    var hotelAccommodationDTO = _mapper.Map<HotelAccommodationDTO>(hotelAccommodation);
                    hotelAccommodationDTO.Approvers = new List<HotelARApproversDTO>();
                    hotelAccommodationDTO.DelegateTeams = new List<HotelARDelegateTeamDTO>();
                    var delegateTeams = _harDelegateTeamRepository.Where(x => x.HotelAccommodationId == hotelAccommodation.Id && x.RecordStatus == RecordStatus.Active).Include(x => x.Person).ToList();
                    var approvers = _harApproversRepository.Where(x => x.HotelAccommodationId == hotelAccommodation.Id && x.RecordStatus == RecordStatus.Active).Include(x => x.Person).ToList();
                    foreach (var delegateTeam in delegateTeams)
                    {
                        var delegateDTO = _mapper.Map<HotelARDelegateTeamDTO>(delegateTeam);
                        hotelAccommodationDTO.DelegateTeams.Add(delegateDTO);
                    }
                    foreach (var approver in approvers)
                    {
                        var approverDTO = _mapper.Map<HotelARApproversDTO>(approver);
                        hotelAccommodationDTO.Approvers.Add(approverDTO);
                    }
                    result.Status = OperationStatus.SUCCESS;
                    result.Message = Resources.OperationSucessfullyCompleted;
                    result.Responses.Add(hotelAccommodationDTO);
                }
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public HotelAccommodationResponse GetById(long id)
        {
            try
            {
                var result = new HotelAccommodationResponse();
                var hotelAccommodation = _haRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active)
                     .Include(x => x.Country)
                    .Include(x => x.Station)
                    .Include(x => x.CostCenter)
                    .Include(x => x.AssignedAgent)
                    .Include(x => x.Assigner)
                    .Include(x => x.Rejector)
                    .Include(x => x.Requester).FirstOrDefault();
                if (hotelAccommodation == null)
                    return new HotelAccommodationResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var hotelAccommodationDTO = _mapper.Map<HotelAccommodationDTO>(hotelAccommodation);
                hotelAccommodationDTO.AttachementPath = new System.Uri(fileSettings.StoredFilesPath + "\\" + hotelAccommodation.AttachementPath).AbsoluteUri;
                hotelAccommodationDTO.Approvers = new List<HotelARApproversDTO>();
                hotelAccommodationDTO.DelegateTeams = new List<HotelARDelegateTeamDTO>();
                var delegateTeams = _harDelegateTeamRepository.Where(x => x.HotelAccommodationId == hotelAccommodation.Id && x.RecordStatus == RecordStatus.Active).Include(x => x.Person).ToList();
                var approvers = _harApproversRepository.Where(x => x.HotelAccommodationId == hotelAccommodation.Id && x.RecordStatus == RecordStatus.Active).Include(x => x.Person).ToList();
                foreach (var delegateTeam in delegateTeams)
                {
                    var delegateDTO = _mapper.Map<HotelARDelegateTeamDTO>(delegateTeam);
                    hotelAccommodationDTO.DelegateTeams.Add(delegateDTO);
                }
                foreach (var approver in approvers)
                {
                    var approverDTO = _mapper.Map<HotelARApproversDTO>(approver);
                    hotelAccommodationDTO.Approvers.Add(approverDTO);
                }
                result.Response = hotelAccommodationDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception)
            {
                return new HotelAccommodationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public HotelAccommodationsResponse GetMyAssignedHotelAccomodation()
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                var result = new HotelAccommodationsResponse();
                var hotelAccommodations = _haRepository.Where(x => x.AssignedTo == person.Id && x.RecordStatus == RecordStatus.Active).Include(x => x.Country)
                       .OrderByDescending(x => x.RegisteredDate)
                    .Include(x => x.Station)
                    .Include(x => x.CostCenter)
                    .Include(x => x.AssignedAgent)
                    .Include(x => x.Assigner)
                    .Include(x => x.Rejector)
                    .Include(x => x.Requester).ToList();
                foreach (var hotelAccommodation in hotelAccommodations)
                {
                    var hotelAccommodationDTO = _mapper.Map<HotelAccommodationDTO>(hotelAccommodation);

                    hotelAccommodationDTO.Approvers = new List<HotelARApproversDTO>();
                    hotelAccommodationDTO.DelegateTeams = new List<HotelARDelegateTeamDTO>();
                    var delegateTeams = _harDelegateTeamRepository.Where(x => x.HotelAccommodationId == hotelAccommodation.Id && x.RecordStatus == RecordStatus.Active).Include(x => x.Person).ToList();
                    var approvers = _harApproversRepository.Where(x => x.HotelAccommodationId == hotelAccommodation.Id && x.RecordStatus == RecordStatus.Active).Include(x => x.Person).ToList();
                    foreach (var delegateTeam in delegateTeams)
                    {
                        var delegateDTO = _mapper.Map<HotelARDelegateTeamDTO>(delegateTeam);
                        hotelAccommodationDTO.DelegateTeams.Add(delegateDTO);
                    }
                    foreach (var approver in approvers)
                    {
                        var approverDTO = _mapper.Map<HotelARApproversDTO>(approver);
                        hotelAccommodationDTO.Approvers.Add(approverDTO);
                    }
                    result.Responses.Add(hotelAccommodationDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new HotelAccommodationsResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };

            }
        }
        public HotelAccommodationsResponse GetMyPurchaseRequsition()
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                var result = new HotelAccommodationsResponse();
                var hotelAccommodations = _haRepository.Where(x => x.RequestedBy == person.Id && x.RecordStatus == RecordStatus.Active)
                       .OrderByDescending(x => x.RegisteredDate)
                    .Include(x => x.Country)
                    .Include(x => x.Station)
                    .Include(x => x.CostCenter)
                    .Include(x => x.AssignedAgent)
                    .Include(x => x.Assigner)
                    .Include(x => x.Rejector)
                    .Include(x => x.Requester).ToList();
                foreach (var hotelAccommodation in hotelAccommodations)
                {
                    var hotelAccommodationDTO = _mapper.Map<HotelAccommodationDTO>(hotelAccommodation);

                    hotelAccommodationDTO.Approvers = new List<HotelARApproversDTO>();
                    hotelAccommodationDTO.DelegateTeams = new List<HotelARDelegateTeamDTO>();
                    var delegateTeams = _harDelegateTeamRepository.Where(x => x.HotelAccommodationId == hotelAccommodation.Id && x.RecordStatus == RecordStatus.Active).Include(x => x.Person).ToList();
                    var approvers = _harApproversRepository.Where(x => x.HotelAccommodationId == hotelAccommodation.Id && x.RecordStatus == RecordStatus.Active).Include(x => x.Person).ToList();
                    foreach (var delegateTeam in delegateTeams)
                    {
                        var delegateDTO = _mapper.Map<HotelARDelegateTeamDTO>(delegateTeam);
                        hotelAccommodationDTO.DelegateTeams.Add(delegateDTO);
                    }
                    foreach (var approver in approvers)
                    {
                        var approverDTO = _mapper.Map<HotelARApproversDTO>(approver);
                        hotelAccommodationDTO.Approvers.Add(approverDTO);
                    }
                    result.Responses.Add(hotelAccommodationDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<HotelAccommodationResponse> Reject(RejectRequest request)
        {
            var hotelAccomodation = _haRepository.Where(c => c.Id == request.RequestId).FirstOrDefault();
            if (hotelAccomodation == null)
                return new HotelAccommodationResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                hotelAccomodation.RejectionRemark = request.Remark;
                hotelAccomodation.PRStatus = PRStatus.Rejected;
                hotelAccomodation.AssignedBy = person.Id;
                hotelAccomodation.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                hotelAccomodation.LastUpdateDate = DateTime.UtcNow;
                if (_haRepository.Update(hotelAccomodation))
                {
                    return new HotelAccommodationResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new HotelAccommodationResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new HotelAccommodationResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<HotelAccommodationResponse> SelfAssign(SelfAssignRequest request)
        {
            var hotelAccomodation = _haRepository.Where(c => c.Id == request.RequestId).FirstOrDefault();
            if (hotelAccomodation == null)
                return new HotelAccommodationResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                hotelAccomodation.AssignedTo = person.Id;
                hotelAccomodation.PRStatus = PRStatus.Assigned;
                hotelAccomodation.AssignedBy = person.Id;
                hotelAccomodation.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                hotelAccomodation.LastUpdateDate = DateTime.UtcNow;
                if (_haRepository.Update(hotelAccomodation))
                {
                    return new HotelAccommodationResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new HotelAccommodationResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new HotelAccommodationResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<HotelAccommodationResponse> UnAssign(long id)
        {
            var hotelAccomodation = _haRepository.Where(c => c.Id == id).FirstOrDefault();
            if (hotelAccomodation == null)
                return new HotelAccommodationResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                hotelAccomodation.AssignedTo = null;
                hotelAccomodation.PRStatus = PRStatus.UnAssigned;
                hotelAccomodation.AssignedBy = person.Id;
                hotelAccomodation.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                hotelAccomodation.LastUpdateDate = DateTime.UtcNow;
                if (_haRepository.Update(hotelAccomodation))
                {
                    return new HotelAccommodationResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new HotelAccommodationResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new HotelAccommodationResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<HotelAccommodationResponse> Update(HotelAccommodationRequest request)
        {
            var hotelAR = _haRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (hotelAR == null)
                return new HotelAccommodationResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                hotelAR.RequestName = request.RequestName;
                hotelAR.HotelServiceType = request.HotelServiceType;
                hotelAR.OriginatingSection = request.OriginatingSection;
                hotelAR.CostCenterId = request.CostCenterId;
                hotelAR.StationId = request.StationId;
                hotelAR.CountryId = request.CountryId;
                hotelAR.City = request.City;
                hotelAR.ContractExpiredate = request.ContractExpiredate;
                hotelAR.Commencementdate = request.Commencementdate;
                hotelAR.RequestDate = request.RequestDate;
                hotelAR.CrewPattern = request.CrewPattern;
                hotelAR.ExpectedDeliveryDate = request.ExpectedDeliveryDate;
                hotelAR.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                hotelAR.LastUpdateDate = DateTime.UtcNow;
                if (_haRepository.Update(hotelAR))
                {
                    return new HotelAccommodationResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new HotelAccommodationResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new HotelAccommodationResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
