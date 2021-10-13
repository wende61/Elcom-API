using AutoMapper;
using EProcurement.DataObjects.Models.Operational;
using EProcurement.Common;
using EProcurement.Common.Model;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Operational;
using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.MasterData;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EProcurement.Common.ResponseModel.MasterData;

namespace EProcurement.Core.Service.Operational
{
    public class ProjectService : IProject<ProjectInitiationResponse, ProjectInitiationsResponse, ProjectInitiationRequest>
    {
        private readonly IRepositoryBase<Person> _personRepository;
        private readonly IRepositoryBase<Project> _projectRepository;
        private readonly IRepositoryBase<ProjectTeam> _projectTeamRepository;
        private readonly IRepositoryBase<HotelAccommodation> _hotelRepository;
        private readonly IRepositoryBase<PurchaseRequisition> _prRepository;
        private readonly IRepositoryBase<RequestForDocument> _documentRepository;
        private readonly IRepositoryBase<TechnicalEvaluation> _technicalEvaluationRepository;
        private readonly IRepositoryBase<FinancialEvaluation> _financialEvaluationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;
        private readonly IServiceUtility _serviceUtility;
        private readonly IMapper _mapper;
        public ProjectService(IRepositoryBase<Project> projectRepository, IHttpContextAccessor httpContextAccessor,
            ILoggerManager logger, IMapper mapper, IRepositoryBase<HotelAccommodation> hotelRepository, IRepositoryBase<RequestForDocument> documentRepository,
            IRepositoryBase<PurchaseRequisition> prRepository, IUserService userService, IServiceUtility serviceUtility, IRepositoryBase<ProjectTeam> projectTeamRepository,
            IRepositoryBase<TechnicalEvaluation> technicalEvaluationRepository , IRepositoryBase<FinancialEvaluation> financialEvaluationRepository,IRepositoryBase<Person> personRepository)
        {
            _financialEvaluationRepository = financialEvaluationRepository;
            _technicalEvaluationRepository = technicalEvaluationRepository;
            _personRepository = personRepository;
            _httpContextAccessor = httpContextAccessor;
            _projectTeamRepository = projectTeamRepository;
            _documentRepository = documentRepository;
            _projectRepository = projectRepository;
            _hotelRepository = hotelRepository;
            _userService = userService;
            _prRepository = prRepository;
            _serviceUtility = serviceUtility;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ProjectSourcingId> GeneratePurchaseProjectCode(ProjectProcessType processType)
        {
            try
            {
                var sourcePrefix = _serviceUtility.GetProjectSourcePrefix(processType);
                var source = _projectRepository
                    .Where(x => x.RecordStatus == RecordStatus.Active && x.RequestType == RequestType.PurchaseRequest && x.SourcingId != null && x.ProjectProcessType == processType)
                    .Cast<Project>()
                    .OrderByDescending(x => x.SourcingId)
                    .FirstOrDefault();
                if (source != null)
                {
                    var newSourceid = source.SourcingId++;
                    var result = sourcePrefix + newSourceid.ToString().PadLeft(4, '0');
                    return new ProjectSourcingId
                    {
                        SourceId = newSourceid,
                        ProjectCode = result
                    };
                }
                else
                {
                    return new ProjectSourcingId
                    {
                        SourceId = 1,
                        ProjectCode = sourcePrefix + "00001"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ProjectSourcingId
                {
                    SourceId = 0,
                    ProjectCode = "UnAssigned"
                };
            }
        }
        public async Task<ProjectSourcingId> GenerateHotelProjectCode()
        {
            try
            {
                var hotelSource = _projectRepository
                    .Where(x => x.RecordStatus == RecordStatus.Active && x.RequestType == RequestType.HotelAccommodationRequest && x.SourcingId != null)
                    .Cast<Project>()
                    .OrderByDescending(x => x.SourcingId)
                    .FirstOrDefault();
                if (hotelSource != null)
                {
                    var newSourceid = hotelSource.SourcingId++;
                    var result = "ETHS" + newSourceid.ToString().PadLeft(4, '0');
                    return new ProjectSourcingId
                    {
                        SourceId = newSourceid,
                        ProjectCode = result
                    };
                }
                else
                {
                    return new ProjectSourcingId
                    {
                        SourceId = 1,
                        ProjectCode = "ETHS00001"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ProjectSourcingId
                {
                    SourceId = 0,
                    ProjectCode = "UnAssigned"
                };
            }
        }
        public ProjectInitiationsResponse GetAll()
        {
            try
            {
                var result = new ProjectInitiationsResponse();
                var projects = _projectRepository.Where(x => x.RecordStatus == RecordStatus.Active);
                foreach (var project in projects)
                {
                    if (project.RequestType == RequestType.HotelAccommodationRequest)
                    {
                        var hotelA = _hotelRepository.Find(project.HotelAccommodationId.Value);
                        var projectDTO = _mapper.Map<ProjectDTO>(project);
                        projectDTO.HotelAccommodation = _mapper.Map<HotelAccommodationDTO>(hotelA);
                        projectDTO.PurchaseRequisition = null;
                        result.Responses.Add(projectDTO);
                    }
                    else
                    {
                        var purchaseR = _prRepository.Find(project.PurchaseRequisitionId.Value);
                        var projectDTO = _mapper.Map<ProjectDTO>(project);
                        projectDTO.PurchaseRequisition = _mapper.Map<PurchaseRequisitionDTO>(purchaseR);
                        projectDTO.HotelAccommodation = null;
                        result.Responses.Add(projectDTO);
                    }

                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;

            }
            catch (Exception ex)
            {
                return new ProjectInitiationsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public ProjectInitiationsResponse GetAllPurchaseProjects()
        {
            try
            {
                var result = new ProjectInitiationsResponse();
                var projects = _projectRepository.Where(x => x.RecordStatus == RecordStatus.Active && x.RequestType == RequestType.PurchaseRequest);
                foreach (var project in projects)
                {
                    var purchaseR = _prRepository.Find(project.PurchaseRequisitionId.Value);
                    var projectDTO = _mapper.Map<ProjectDTO>(project);
                    projectDTO.PurchaseRequisition = _mapper.Map<PurchaseRequisitionDTO>(purchaseR);
                    projectDTO.HotelAccommodation = null;
                    result.Responses.Add(projectDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new ProjectInitiationsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public ProjectInitiationsResponse GetAllHotelAccommodationProjects()
        {
            try
            {
                var result = new ProjectInitiationsResponse();
                var projects = _projectRepository.Where(x => x.RecordStatus == RecordStatus.Active && x.RequestType == RequestType.HotelAccommodationRequest);
                foreach (var project in projects)
                {
                    var hotelA = _hotelRepository.Find(project.HotelAccommodationId.Value);
                    var projectDTO = _mapper.Map<ProjectDTO>(project);
                    projectDTO.HotelAccommodation = _mapper.Map<HotelAccommodationDTO>(hotelA);
                    projectDTO.PurchaseRequisition = null;
                    result.Responses.Add(projectDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;

            }
            catch (Exception ex)
            {
                return new ProjectInitiationsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public ProjectInitiationsResponse GetMyProjects()
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                var result = new ProjectInitiationsResponse();
                var projects = _projectRepository.Where(x => x.AssignedPerson == person.Id && x.RecordStatus == RecordStatus.Active).ToList();
                foreach (var project in projects)
                {
                    if (project.RequestType == RequestType.HotelAccommodationRequest)
                    {
                        var hotelA = _hotelRepository.Find(project.HotelAccommodationId.Value);
                        var projectDTO = _mapper.Map<ProjectDTO>(project);
                        projectDTO.HotelAccommodation = _mapper.Map<HotelAccommodationDTO>(hotelA);
                        projectDTO.PurchaseRequisition = null;
                        result.Responses.Add(projectDTO);
                    }
                    else
                    {
                        var purchaseR = _prRepository.Find(project.PurchaseRequisitionId.Value);
                        var projectDTO = _mapper.Map<ProjectDTO>(project);
                        projectDTO.PurchaseRequisition = _mapper.Map<PurchaseRequisitionDTO>(purchaseR);
                        projectDTO.HotelAccommodation = null;
                        result.Responses.Add(projectDTO);
                    }
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new ProjectInitiationsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public ProjectInitiationsResponse GetMyHotelAccommodationProjects()
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                var result = new ProjectInitiationsResponse();
                var projects = _projectRepository.Where(x => x.AssignedPerson ==person.Id && x.RecordStatus == RecordStatus.Active && x.RequestType == RequestType.HotelAccommodationRequest).ToList();
                foreach (var project in projects)
                {
                    var hotelA = _hotelRepository.Find(project.HotelAccommodationId.Value);
                    var projectDTO = _mapper.Map<ProjectDTO>(project);
                    projectDTO.HotelAccommodation = _mapper.Map<HotelAccommodationDTO>(hotelA);
                    projectDTO.PurchaseRequisition = null;
                    result.Responses.Add(projectDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new ProjectInitiationsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public ProjectInitiationsResponse GetMyPurchaseProjects()
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                var result = new ProjectInitiationsResponse();
                var projects = _projectRepository.Where(x => x.AssignedPerson == person.Id && x.RecordStatus == RecordStatus.Active && x.RequestType == RequestType.PurchaseRequest).ToList();
                foreach (var project in projects)
                {
                    var purchaseR = _prRepository.Find(project.PurchaseRequisitionId.Value);
                    var projectDTO = _mapper.Map<ProjectDTO>(project);
                    projectDTO.PurchaseRequisition = _mapper.Map<PurchaseRequisitionDTO>(purchaseR);
                    projectDTO.HotelAccommodation = null;
                    result.Responses.Add(projectDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new ProjectInitiationsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<ProjectInitiationResponse> Initiate(ProjectInitiationRequest request)
        {
            try
            {
                var project = _mapper.Map<Project>(request);
                if (request.RequestType == RequestType.HotelAccommodationRequest)
                {
                    var previousProject = _projectRepository.Where(x => x.RecordStatus == RecordStatus.Active && x.HotelAccommodationId == request.HotelAccommodationId).FirstOrDefault();
                    if (previousProject != null)
                        return new ProjectInitiationResponse { Message = "Project has been initialized for this request.", Status = OperationStatus.ERROR };
                    var personId = _hotelRepository.Where(x => x.Id == request.HotelAccommodationId).FirstOrDefault().AssignedTo;
                    var sourceId = await GenerateHotelProjectCode();
                    project.IsBECMandatory = true;
                    project.HotelAccommodationId = request.HotelAccommodationId;
                    project.SourcingId = sourceId.SourceId;
                    project.PurchaseRequisitionId = null;
                    project.ProjectCode = sourceId.ProjectCode;
                    project.AssignedPerson = personId;
                }
                else
                {
                    var previousProject = _projectRepository.Where(x => x.RecordStatus == RecordStatus.Active && x.PurchaseRequisitionId == request.PurchaseRequisitionId).FirstOrDefault();
                    if (previousProject != null)
                        return new ProjectInitiationResponse { Message = "Project has been initialized for this request.", Status = OperationStatus.ERROR };
                    var personId = _prRepository.Where(x => x.Id == request.PurchaseRequisitionId).FirstOrDefault().AssignedTo;
                    project.PurchaseRequisitionId = request.PurchaseRequisitionId;
                    project.HotelAccommodationId = null;
                    project.ProjectCode = "";
                    project.AssignedPerson = personId;
                }
                project.StartDate = DateTime.Now;
                project.EndDate = DateTime.MaxValue;
                project.RegisteredDate = DateTime.Now;
                project.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                project.RecordStatus = RecordStatus.Active;
                project.IsReadOnly = false;
                if (_projectRepository.Add(project))
                    return new ProjectInitiationResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new ProjectInitiationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new ProjectInitiationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<ProjectInitiationResponse> AssignProcessType(PurchaseProcessTypeRequisition request)
        {
            var project = _projectRepository.Where(c => c.Id == request.ProjectId).FirstOrDefault();
            if (project == null)
                return new ProjectInitiationResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                var source = await GeneratePurchaseProjectCode(request.ProjectProcessType);
                project.ProjectProcessType = request.ProjectProcessType;
                project.SourcingId = source.SourceId;
                project.ProjectCode = source.ProjectCode;
                if (_projectRepository.Update(project))
                {
                    return new ProjectInitiationResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new ProjectInitiationResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new ProjectInitiationResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
        public ProjectInitiationResponse GetById(long id)
        {
            try
            {
                var result = new ProjectInitiationResponse();
                var project = _projectRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).Include(x=>x.ProjectTeams)
                    .Include(x=>x.PurchaseRequisition)
                    .Include(x=>x.PurchaseRequisition.CostCenter)
                    .Include(x=>x.HotelAccommodation)
                    .Include(x=>x.HotelAccommodation.CostCenter)
                    .Include(x=>x.Person)
                    .FirstOrDefault();
                if (project == null)
                    return new ProjectInitiationResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                if (project.RequestType == RequestType.HotelAccommodationRequest)
                {
                    var hotelA = _hotelRepository.Find(project.HotelAccommodationId.Value);
                    var projectDTO = _mapper.Map<ProjectDTO>(project);
                    //projectDTO.HotelAccommodation = _mapper.Map<HotelAccommodationDTO>(hotelA);
                    projectDTO.PurchaseRequisition = null;
                    result.Response = projectDTO;
                }
                else
                {
                    var purchaseR = _prRepository.Where(x=>x.Id==project.PurchaseRequisitionId.Value).Include(X=>X.DelegateTeam).FirstOrDefault();
                    var projectDTO = _mapper.Map<ProjectDTO>(project);
                    //projectDTO.PurchaseRequsition = _mapper.Map<PurchaseRequisitionDTO>(purchaseR);
                    //projectDTO.PurchaseRequsition.DelegateTeam = new List<PRDelegateTeamDTO>();
                    //foreach (var  delegat in purchaseR.DelegateTeam)
                    //{
                    //    var delegateMember = new PRDelegateTeamDTO();
                    //    var person = _personRepository.Find(delegat.PersonId.Value);
                    //    delegateMember.Id = delegat.Id;
                    //    delegateMember.Person = _mapper.Map<PersonDTO>(person);
                    //    projectDTO.PurchaseRequsition.DelegateTeam.Add(delegateMember);
                    //}
                    //projectDTO.HotelAccommodation = null;
                    result.Response = projectDTO;
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new ProjectInitiationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public ProjectOverviewResponse GetProjectOverview(long id)
        {
            var result = new ProjectOverviewResponse();
            var projectOverview = new ProjectOverviewDTO();
            projectOverview.BecTeamMembers = new List<BECTeamMember>();
            var project = _projectRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).Include(x => x.ProjectTeams).FirstOrDefault();
            if (project == null)
                return new ProjectOverviewResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
            var projectTeam = _projectTeamRepository.Where(x => x.ProjectId == project.Id && x.RecordStatus == RecordStatus.Active).Include(x => x.Person).ToList();
            var document = _documentRepository.Where(x => x.ProjectId == project.Id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
            var financialEvaluation = _financialEvaluationRepository.Where(x => x.ProjectId == project.Id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
            var technicalEvaluation = _technicalEvaluationRepository.Where(x => x.ProjectId == project.Id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
            //
            if (project.RequestType == RequestType.HotelAccommodationRequest)
            {
                var hotelAccommodation = _hotelRepository.Where(x => x.Id == project.HotelAccommodationId && x.RecordStatus == RecordStatus.Active).Include(X => X.CostCenter).FirstOrDefault();
                if (hotelAccommodation == null)
                    return new ProjectOverviewResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
                projectOverview.RequestedCostCenter = hotelAccommodation.CostCenter.CostCenterName;
            }
            else
            {
                var purchaseRequesition = _prRepository.Where(x => x.Id == project.PurchaseRequisitionId && x.RecordStatus == RecordStatus.Active).Include(X => X.CostCenter).FirstOrDefault();
                if (purchaseRequesition == null)
                    return new ProjectOverviewResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
                projectOverview.RequestedCostCenter = purchaseRequesition.CostCenter.CostCenterName;
            }
            projectOverview.ProjectName = project.ProjectName;
            projectOverview.ProjectReferenceCode = project.ProjectCode;
            projectOverview.PlannedCompletionDate = project.PlannedCompletionDate;
            projectOverview.PurchaseMethodType =((ProjectProcessType)project.ProjectProcessType).ToString();
            projectOverview.DocumentType = (document == null ? "" : ((RequestDocumentType)document.RequestDocumentType).ToString());
            projectOverview.TechnicalEvaluation = (technicalEvaluation == null ? "0" : technicalEvaluation.TechnicalEvaluationValue.ToString());
            projectOverview.FinancialEvaluation = (financialEvaluation == null ? "0" : financialEvaluation.FinancialEvaluationValue.ToString());
            foreach (var team in projectTeam)
            {
                BECTeamMember becTeamMember = new BECTeamMember();
                becTeamMember.Role = team.Role.ToString();
                becTeamMember.Name = team.Person.FirstName + " " + team.Person.MiddleName;
                projectOverview.BecTeamMembers.Add(becTeamMember);
            }
            result.Response = projectOverview;
            result.Status = OperationStatus.SUCCESS;
            result.Message = Resources.OperationSucessfullyCompleted;
            return result;
        }
        public ProjectInitiationResponse DefineBidClosing(BidClosingRequest request)
        {
            var project = _projectRepository.Where(x => x.Id == request.ProjectId && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
            if (project == null)
                return new ProjectInitiationResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
            try
            {
                project.BidClosingDate = request.BidClosingDate;
                project.TechnicalOpeningDate = request.TechnicalProposalOpeningDate;
                if (_projectRepository.Update(project))
                {
                    return new ProjectInitiationResponse
                    {
                        Response = _mapper.Map<ProjectDTO>(project),
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                return new ProjectInitiationResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
            catch (Exception ex)
            {
                return new ProjectInitiationResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}

