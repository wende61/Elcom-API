using AutoMapper;
using EProcurement.DataObjects.Models.Operational;
using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Operational;
using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.MasterData;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Service.Operational
{
    public class RequestForDocumentService : IRequestForDocument<RequestForDocResponse, RequestForDocsResponse, RequestForDocRequest>
    {
        private readonly IRepositoryBase<RequestForDocument> _rfdRepository;
        private readonly IRepositoryBase<Project> _projectRepository;
        private readonly IRepositoryBase<ProjectTeam> _projectTeamRepository;
        private readonly IRepositoryBase<RequestForDocAttachment> _rfdaRepository;
        private readonly IRepositoryBase<RequestForDocumentApproval> _rfdApprovalRepository;
        private readonly IRepositoryBase<Person> _personRepository;
        private readonly IAppDbTransactionContext _appTransaction;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly FileSettings fileSettings;
        public RequestForDocumentService(IRepositoryBase<RequestForDocument> rfdRepository, IRepositoryBase<Project> projectRepository
           , IRepositoryBase<ProjectTeam> projectTeamRepository, IRepositoryBase<RequestForDocAttachment> rfdaRepository, IConfiguration config,
            IHttpContextAccessor httpContextAccessor,IUserService userService, ILoggerManager logger, IMapper mapper, IRepositoryBase<Person> personRepository,
            IAppDbTransactionContext appTransaction, IRepositoryBase<RequestForDocumentApproval> rfdaApprovalRepository)
        {
            _rfdApprovalRepository = rfdaApprovalRepository;
            _projectTeamRepository = projectTeamRepository;
            _httpContextAccessor = httpContextAccessor;
            _projectRepository = projectRepository;
            _personRepository = personRepository;
            _appTransaction = appTransaction;
            _rfdaRepository = rfdaRepository;
            _rfdRepository = rfdRepository;
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
            var fileSettingsSection = config.GetSection("FileSettings");
            fileSettings = fileSettingsSection.Get<FileSettings>();
        }

        public RequestForDocResponse Approve(DocumentApprovalRequest request)
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                var requestForDocument = _rfdRepository.Where(x => x.Id == request.RequestForDocumentId && x.RecordStatus == RecordStatus.Active).Include(x=>x.Attachements).Include(X=>X.Approvers).FirstOrDefault();
                var rfdApprovals = _rfdApprovalRepository.Where(x => x.RequestForDocumentId == request.RequestForDocumentId);
                var forApproval = rfdApprovals.Where(x => x.Approver == person.Id).FirstOrDefault();
                if (requestForDocument==null || rfdApprovals==null || forApproval==null)
                    return new RequestForDocResponse { Status = OperationStatus.EMPTY, Message = Resources.RecordDoesNotExist };
                forApproval.ApprovalStatus = request.ApprovalStatus;
                if (_rfdApprovalRepository.Update(forApproval))
                {
                    bool approved = true;
                    foreach (var approval in rfdApprovals)
                    {
                        if (approval.ApprovalStatus!=ApprovalStatus.Approved)
                        {
                            approved = false;
                            break;
                        }
                    }
                    if (approved)
                    {
                        requestForDocument.ApprovalStatus = ApprovalStatus.Approved;
                        if (_rfdRepository.Update(requestForDocument))
                        {
                            return new RequestForDocResponse
                            {
                                Response = _mapper.Map<RequestForDocDTO>(requestForDocument),
                                Message = Resources.OperationSucessfullyCompleted,
                                Status = OperationStatus.SUCCESS
                            };
                        }
                    }
                    return new RequestForDocResponse
                    {
                        Response = _mapper.Map<RequestForDocDTO>(requestForDocument),
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                return new RequestForDocResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest };
            }
            catch (Exception ex)
            {
                return new RequestForDocResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest };
            }
        }
        public async Task<RequestForDocResponse> Create(RequestForDocRequest request)
        {
            try
            {
                var project = _projectRepository.Where(x => x.Id == request.ProjectId && x.RecordStatus == RecordStatus.Active)
                    .Include(x=>x.PurchaseRequisition)
                    .Include(x=>x.HotelAccommodation)
                    .Include(x=>x.ProjectTeams)
                    .FirstOrDefault();
                var projectTeam = _projectTeamRepository.Where(x => x.ProjectId == request.ProjectId).Cast<ProjectTeam>().ToList();
                using (var uow = new AppUnitOfWork(_appTransaction.GetDbContext()))
                {
                    RepositoryBaseWork<RequestForDocument> requestForDocumentRepo = new RepositoryBaseWork<RequestForDocument>(uow);
                    using (var transaction = uow.BeginTrainsaction())
                    {
                        var requestForDocument = _mapper.Map<RequestForDocument>(request);
                        requestForDocument.Attachements = new List<RequestForDocAttachment>();
                        requestForDocument.Approvers = new List<RequestForDocumentApproval>();
                        requestForDocument.StartDate = DateTime.Now;
                        requestForDocument.EndDate = DateTime.MaxValue;
                        requestForDocument.RegisteredDate = DateTime.Now;
                        requestForDocument.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                        requestForDocument.RecordStatus = RecordStatus.Active;
                        requestForDocument.IsReadOnly = false;
                        requestForDocument.ApprovalStatus = ApprovalStatus.Pending;
                        if (project.RequestType == RequestType.HotelAccommodationRequest && project.IsBECMandatory == true && projectTeam.Count == 0)
                            requestForDocument.ApprovalStatus = ApprovalStatus.Approved;
                        foreach (var attachemet in request.Attachements)
                        {
                            var response = FileUploader.Upload(attachemet, fileSettings.StoredFilesPath, "1", fileSettings.FileSizeLimit);
                            if (response.Result)
                            {
                                var rfdAttachement = new RequestForDocAttachment();
                                rfdAttachement.AttachementPath = response.Path;
                                rfdAttachement.RequestForDocumentationId = requestForDocument.Id;
                                requestForDocument.Attachements.Add(rfdAttachement);
                            }
                        }
                        foreach (var approver in projectTeam)
                        {                            
                            var documentApprover = new RequestForDocumentApproval();
                            documentApprover.Approver = approver.PersonId.Value;
                            documentApprover.RequestForDocumentId = requestForDocument.Id;
                            documentApprover.ApprovalStatus = ApprovalStatus.Pending;
                            requestForDocument.Approvers.Add(documentApprover);
                        }
                        requestForDocumentRepo.Add(requestForDocument);
                        if (await uow.SaveChangesAsync() > 0)
                        {
                            transaction.Commit();
                            return new RequestForDocResponse
                            {
                                Response = _mapper.Map<RequestForDocDTO>(requestForDocument),
                                Message = Resources.OperationSucessfullyCompleted,
                                Status = OperationStatus.SUCCESS
                            };
                        }
                        transaction.Rollback();
                        return new RequestForDocResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                    }
                }
                return new RequestForDocResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new RequestForDocResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public RequestForDocsResponse GetAll()
        {
            var result = new RequestForDocsResponse();
            var requestForDocs = _rfdRepository.Where(x => x.RecordStatus == RecordStatus.Active).ToList();
            if (requestForDocs==null)
                return new RequestForDocsResponse { Status = OperationStatus.EMPTY, Message = Resources.RecordDoesNotExist };

            foreach (var rfd in requestForDocs)
            {
                var project = _projectRepository.Find(rfd.ProjectId);
                var rfda = _rfdaRepository.Where(x => x.RequestForDocumentationId == rfd.Id);
                var rfdApprovals = _rfdApprovalRepository.Where(x => x.RequestForDocumentId == rfd.Id);
                var rfdDOT = _mapper.Map<RequestForDocDTO>(rfd);
                rfdDOT.Approvers = new List<RequestForDocumentApprovalDTO>();
                rfdDOT.Attachements = new List<DocumentAttachementDTO>();
                rfdDOT.Project = _mapper.Map<ProjectDTO>(project);
                foreach (var approval in rfdApprovals)
                {
                    var approvalDTO = _mapper.Map<RequestForDocumentApprovalDTO>(approval);
                    rfdDOT.Approvers.Add(approvalDTO);
                }
                foreach (var attachment in rfda)
                {
                    var attachementDTO = _mapper.Map<DocumentAttachementDTO>(attachment);
                    rfdDOT.Attachements.Add(attachementDTO);
                }
                result.Response.Add(rfdDOT);
            }
            result.Status = OperationStatus.SUCCESS;
            result.Message = Resources.OperationSucessfullyCompleted;
            return result;
        }
        public RequestForDocResponse GetById(long id)
        {
            try
            {
                var result = new RequestForDocResponse();
                var requestForDocument = _rfdRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (requestForDocument == null)
                    return new RequestForDocResponse { Status = OperationStatus.EMPTY, Message = Resources.RecordDoesNotExist };
                var project = _projectRepository.Find(requestForDocument.ProjectId);
                var rfdAttachements = _rfdaRepository.Where(x => x.RequestForDocumentationId == requestForDocument.Id);
                var rfdApprovals = _rfdApprovalRepository.Where(x => x.RequestForDocumentId == requestForDocument.Id);
                var rfdDOT = _mapper.Map<RequestForDocDTO>(requestForDocument);
                rfdDOT.Approvers = new List<RequestForDocumentApprovalDTO>();
                rfdDOT.Attachements = new List<DocumentAttachementDTO>();
                rfdDOT.Project = _mapper.Map<ProjectDTO>(project);
                foreach (var approval in rfdApprovals)
                {
                    var approvalDTO = _mapper.Map<RequestForDocumentApprovalDTO>(approval);
                    rfdDOT.Approvers.Add(approvalDTO);
                }
                foreach (var attachment in rfdAttachements)
                {
                    var attachementDTO = _mapper.Map<DocumentAttachementDTO>(attachment);
                    rfdDOT.Attachements.Add(attachementDTO);
                }
                result.Response = rfdDOT;
                return result;
            }
            catch (Exception)
            {
                return new RequestForDocResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest };
            }
        }
        public RequestForDocResponse GetByProjectId(long id)
        {
            try
            {
                var result = new RequestForDocResponse();
                var requestForDocument = _rfdRepository.Where(x => x.ProjectId == id && x.RecordStatus == RecordStatus.Active)
                    .Include(x=>x.Approvers)
                    .FirstOrDefault();
                if (requestForDocument==null)
                    return new RequestForDocResponse { Status = OperationStatus.EMPTY, Message = Resources.RecordDoesNotExist };
                var project = _projectRepository.Find(requestForDocument.ProjectId);
                var rfdAttachements= _rfdaRepository.Where(x => x.RequestForDocumentationId == requestForDocument.Id);
                var rfdApprovals = _rfdApprovalRepository.Where(x => x.RequestForDocumentId == requestForDocument.Id).Include(x=>x.Person);
                var rfdDOT = _mapper.Map<RequestForDocDTO>(requestForDocument);
                rfdDOT.Approvers = new List<RequestForDocumentApprovalDTO>();
                rfdDOT.Attachements = new List<DocumentAttachementDTO>();
                rfdDOT.Project = _mapper.Map<ProjectDTO>(project);
                foreach (var approval in rfdApprovals)
                {
                    var approvalDTO = _mapper.Map<RequestForDocumentApprovalDTO>(approval);
                    rfdDOT.Approvers.Add(approvalDTO);
                }
                foreach (var attachment in rfdAttachements)
                {
                    var attachementDTO = _mapper.Map<DocumentAttachementDTO>(attachment);
                    rfdDOT.Attachements.Add(attachementDTO);
                }
                result.Response=rfdDOT;
                return result;
            }
            catch (Exception ex)
            {
                return new RequestForDocResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest };
            }
        }
    }
}
