using AutoMapper;
using EProcurement.DataObjects.Models.Operational;
using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Helper;
using EProcurement.Core.Interface.Operational;
using EProcurement.DataObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using EProcurement.DataObjects.Models.MasterData;
using EProcurement.DataObjects.Models.MasterData;
using EProcurement.Common.ResponseModel.MasterData;
using Microsoft.EntityFrameworkCore;

namespace EProcurement.Core.Service.Operational
{
    public class PurchaseRequisitionService : IPurchaseRequisition<PurchaseRequisitionResponse, PurchaseRequisitionsResponse, PurchaseRequisitionRequest>, IRequest<RejectRequest,AssignRequest, SelfAssignRequest, PurchaseRequisitionResponse>
    {
        private readonly IRepositoryBase<PurchaseRequisition> _prRepository;
        private readonly IRepositoryBase<PRDelegateTeam> _prDelegateTeamRepository;
        private readonly IRepositoryBase<PRApprover> _prApproversRepository;
        private readonly IRepositoryBase<Project> _projectRepository;
        private readonly IRepositoryBase<PurchaseGroup> _pgRepository;
        private readonly IRepositoryBase<RequirmentPeriod> _rpRepository;
        private readonly IRepositoryBase<ProcurementSection> _psRepository;
        private readonly IRepositoryBase<CostCenter> _csRepository;
        private readonly IRepositoryBase<Person> _personRepository;
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IAppDbTransactionContext _appTransaction;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly FileSettings fileSettings;
        public PurchaseRequisitionService(IRepositoryBase<PurchaseRequisition> prRepository, IRepositoryBase<PRDelegateTeam> prDelegateTeamRepository,
             IRepositoryBase<PRApprover> prApproversRepository, ILoggerManager logger, IHttpContextAccessor httpContextAccessor, IMapper mapper,
             IAppDbTransactionContext appTransaction,  IUserService userService,IConfiguration config,
             IRepositoryBase<Person> personRepository, IRepositoryBase<CostCenter> csRepository, IRepositoryBase<ProcurementSection> psRepository,
             IRepositoryBase<RequirmentPeriod> rpRepository, IRepositoryBase<PurchaseGroup> pgRepository, IRepositoryBase<Project> projectRepository
           )
        {
            _prDelegateTeamRepository = prDelegateTeamRepository;
            _prApproversRepository = prApproversRepository;
            _httpContextAccessor = httpContextAccessor;
            _personRepository = personRepository;
            _appTransaction = appTransaction;
            _projectRepository = projectRepository;
            _pgRepository = pgRepository;
            _rpRepository = rpRepository;
            _psRepository = psRepository;
            _csRepository = csRepository;
            _prRepository = prRepository;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
            var fileSettingsSection = config.GetSection("FileSettings");
             fileSettings = fileSettingsSection.Get<FileSettings>();          
        }
        public async Task<PurchaseRequisitionResponse> Assign(AssignRequest request)
        {
            var purchaseRequest = _prRepository.Where(c => c.Id == request.RequestId).FirstOrDefault();
            if (purchaseRequest == null)
                return new PurchaseRequisitionResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                purchaseRequest.AssignedTo = request.PersonId;
                if (purchaseRequest.PRStatus == PRStatus.UnAssigned || purchaseRequest.PRStatus == null)
                {
                    purchaseRequest.AssignedTo = request.PersonId;
                    purchaseRequest.AssignRemark = request.Remark;
                    purchaseRequest.PRStatus = PRStatus.Assigned;
                }
                else if (purchaseRequest.PRStatus == PRStatus.Assigned)
                {
                    var project = _projectRepository.Where(x => x.PurchaseRequisitionId == purchaseRequest.Id && x.AssignedPerson != null && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                    if (project!=null)
                    {
                        project.AssignedPerson = request.PersonId;
                       _projectRepository.Update(project);
                       
                    }
                    purchaseRequest.AssignedTo = request.PersonId;
                    purchaseRequest.PRStatus = PRStatus.ReAssigned;
                    purchaseRequest.ReAssignRemark = request.Remark;
                }
                purchaseRequest.AssignedBy = person.Id;
                purchaseRequest.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                purchaseRequest.LastUpdateDate = DateTime.UtcNow;
                if (_prRepository.Update(purchaseRequest))
                {
                    return new PurchaseRequisitionResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new PurchaseRequisitionResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new PurchaseRequisitionResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<PurchaseRequisitionResponse> Create(PurchaseRequisitionRequest request, List<PRApproversRequest> approvers, List<PRDelegateTeamRequest> delegates)
        {
            try
            {
                var response = FileUploader.Upload(request.SpecificationFile, fileSettings.StoredFilesPath, "1", fileSettings.FileSizeLimit);
                if (response.Result)
                {
                    using (var uow = new AppUnitOfWork(_appTransaction.GetDbContext()))
                    {
                        var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");

                        RepositoryBaseWork<PurchaseRequisition> prRepo = new RepositoryBaseWork<PurchaseRequisition>(uow);
                        RepositoryBaseWork<PRApprover> prApproverRepo = new RepositoryBaseWork<PRApprover>(uow);
                        RepositoryBaseWork<PRDelegateTeam> prDelegateTeamRepo = new RepositoryBaseWork<PRDelegateTeam>(uow);
                        using (var transaction = uow.BeginTrainsaction())
                        {
                            try
                            {
                                var person = _userService.GetPerson(userName);
                                var newPurchseRequest = _mapper.Map<PurchaseRequisition>(request);
                                newPurchseRequest.DelegateTeam = new List<PRDelegateTeam>();
                                newPurchseRequest.Approvers = new List<PRApprover>();
                                newPurchseRequest.AttachementPath = response.Path;
                                newPurchseRequest.ApprovalStatus = ApprovalStatus.Pending;
                                newPurchseRequest.IsInitiated = false;
                                newPurchseRequest.PRStatus = PRStatus.UnAssigned;
                                newPurchseRequest.RequestedBy = person.Id;
                                newPurchseRequest.RequestDate = DateTime.Now;
                                newPurchseRequest.StartDate = DateTime.Now;
                                newPurchseRequest.EndDate = DateTime.MaxValue;
                                newPurchseRequest.RegisteredDate = DateTime.Now;
                                newPurchseRequest.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                newPurchseRequest.RecordStatus = RecordStatus.Active;
                                foreach (var delegateTeam in delegates)
                                {
                                    var prDelegateTeam = new PRDelegateTeam();
                                    prDelegateTeam.PersonId = delegateTeam.PersonId;
                                    prDelegateTeam.PurchaseRequisitionId = newPurchseRequest.Id;
                                    prDelegateTeam.StartDate = DateTime.Now;
                                    prDelegateTeam.EndDate = DateTime.MaxValue;
                                    prDelegateTeam.RegisteredDate = DateTime.Now;
                                    prDelegateTeam.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                    prDelegateTeam.RecordStatus = RecordStatus.Active;
                                    newPurchseRequest.DelegateTeam.Add(prDelegateTeam);
                                }
                                foreach (var approver in approvers)
                                {
                                    var prApprover = new PRApprover();
                                    prApprover.PersonId = approver.PersonId;
                                    prApprover.Order = approver.Order;
                                    prApprover.PurchaseRequisitionId = newPurchseRequest.Id;
                                    prApprover.ApprovalStatus = ApprovalStatus.Pending;
                                    prApprover.StartDate = DateTime.Now;
                                    prApprover.EndDate = DateTime.MaxValue;
                                    prApprover.RegisteredDate = DateTime.Now;
                                    prApprover.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                    prApprover.RecordStatus = RecordStatus.Active;
                                    newPurchseRequest.Approvers.Add(prApprover);
                                }
                                prRepo.Add(newPurchseRequest);
                                if (await uow.SaveChangesAsync() > 0)
                                {
                                    transaction.Commit();
                                    return new PurchaseRequisitionResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                                }
                                transaction.Rollback();
                                return new PurchaseRequisitionResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR, BusinessErrorCode = "Unable to create Approvers" };
                            }
                            catch (Exception ex)
                            {
                                return new PurchaseRequisitionResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                            }
                        }
                    }
                }
                return new PurchaseRequisitionResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new PurchaseRequisitionResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public PurchaseRequisitionsResponse GetAll()
        {
            try
            {           
                var result = new PurchaseRequisitionsResponse();
                var purchaseRequsitions = _prRepository.Where(x =>x.RecordStatus == RecordStatus.Active)
                    .OrderByDescending(x=>x.RegisteredDate)
                    .Include(x=>x.PurchaseGroup)
                    .Include(x=>x.RequirmentPeriod)
                    .Include(x=>x.ProcurementSection)
                    .Include(x=>x.CostCenter)
                    .Include(x=>x.Requester)
                    .Include(x=>x.Rejector)
                    .Include(x=>x.AssignedAgent)
                    .Include(x=>x.Assigner)
                    .ToList();
                foreach (var purchaseRequsition in purchaseRequsitions)
                {
                    var purchaseRequisitionDTO = _mapper.Map<PurchaseRequisitionDTO>(purchaseRequsition);
                    purchaseRequisitionDTO.DelegateTeam = new List<PRDelegateTeamDTO>();
                    purchaseRequisitionDTO.Approvers = new List<PRApproversDTO>();                   
                    var delegateTeams = _prDelegateTeamRepository.Where(x => x.PurchaseRequisitionId == purchaseRequsition.Id && x.RecordStatus == RecordStatus.Active).ToList();
                    var approvers = _prApproversRepository.Where(x => x.PurchaseRequisitionId == purchaseRequsition.Id && x.RecordStatus == RecordStatus.Active).OrderBy(x=>x.Order).ToList();
                    foreach (var delegateTeam in delegateTeams)
                    {
                        var delegateDTO = _mapper.Map<PRDelegateTeamDTO>(delegateTeam);
                        purchaseRequisitionDTO.DelegateTeam.Add(delegateDTO);
                    }
                    foreach (var approver in approvers)
                    {
                        var approverDTO = _mapper.Map<PRApproversDTO>(approver);
                        purchaseRequisitionDTO.Approvers.Add(approverDTO);
                    }
                  
                    result.Response.Add(purchaseRequisitionDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new PurchaseRequisitionsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public PurchaseRequisitionResponse GetById(long id)
        {
            try
            {
                var result = new PurchaseRequisitionResponse();
                var purchaseRequsition = _prRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active)
                    .Include(x => x.DelegateTeam).Include(x => x.Approvers)
                    .Include(x => x.PurchaseGroup) .Include(x => x.RequirmentPeriod)
                    .Include(x => x.ProcurementSection).Include(x => x.CostCenter)
                    .Include(x => x.Requester).Include(x => x.Requester.CostCenter).Include(x => x.Rejector)
                    .Include(x => x.AssignedAgent) .Include(x => x.Assigner)
                    .FirstOrDefault();
                if (purchaseRequsition == null)
                    return new PurchaseRequisitionResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var purchaseRequisitionDTO = _mapper.Map<PurchaseRequisitionDTO>(purchaseRequsition);
                if (!string.IsNullOrEmpty(purchaseRequsition.AttachementPath))
                    purchaseRequisitionDTO.AttachementPath= @"\\svhqdts01\Files\" + purchaseRequsition.AttachementPath;
                foreach (var delegateTeam in purchaseRequisitionDTO.DelegateTeam)
                {
                    var delegates = _prDelegateTeamRepository.Where(x => x.Id == delegateTeam.Id).Include(x => x.Person).FirstOrDefault();
                    delegateTeam.Person = _mapper.Map<PersonDTO>(delegates.Person);
                }
                foreach (var approver in purchaseRequisitionDTO.Approvers)
                {
                    var delegates = _prDelegateTeamRepository.Where(x=>x.Id==approver.Id).Include(x=>x.Person).FirstOrDefault();
                    approver.Person = _mapper.Map<PersonDTO>(delegates.Person);
                }
                result.Response = purchaseRequisitionDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new PurchaseRequisitionResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public PurchaseRequisitionsResponse GetMyAssignedPurchaseRequsition()
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                var result = new PurchaseRequisitionsResponse();
                var purchaseRequsitions = _prRepository.Where(x => x.AssignedTo == person.Id && x.RecordStatus == RecordStatus.Active)
                       .OrderByDescending(x => x.RegisteredDate)
                    .Include(x => x.DelegateTeam).Include(x => x.Approvers)
                    .Include(x => x.PurchaseGroup).Include(x => x.RequirmentPeriod)
                    .Include(x => x.ProcurementSection).Include(x => x.CostCenter)
                    .Include(x => x.Requester).Include(x => x.Rejector)
                    .Include(x => x.AssignedAgent).Include(x => x.Assigner).ToList();

                if (purchaseRequsitions.Count() == 0)
                    return new PurchaseRequisitionsResponse { Message = Resources.RecordDoesNotExist, Status=OperationStatus.EMPTY };
                foreach (var purchaseRequsition in purchaseRequsitions)
                {
                    var purchaseRequisitionDTO = _mapper.Map<PurchaseRequisitionDTO>(purchaseRequsition);
                    purchaseRequisitionDTO.DelegateTeam = new List<PRDelegateTeamDTO>();
                    purchaseRequisitionDTO.Approvers = new List<PRApproversDTO>();
                    var delegateTeams = _prDelegateTeamRepository.Where(x => x.PurchaseRequisitionId == purchaseRequsition.Id && x.RecordStatus == RecordStatus.Active).ToList();
                    var approvers = _prApproversRepository.Where(x => x.PurchaseRequisitionId == purchaseRequsition.Id && x.RecordStatus == RecordStatus.Active).OrderBy(x => x.Order).ToList();
                    foreach (var delegat in delegateTeams)
                    {
                        var delegateMember = new PRDelegateTeamDTO();
                        var member = _personRepository.Find(delegat.PersonId.Value);
                        delegateMember.Id = delegat.Id;
                        delegateMember.Person = _mapper.Map<PersonDTO>(member);
                        purchaseRequisitionDTO.DelegateTeam.Add(delegateMember);
                    }
                    foreach (var approver in approvers)
                    {
                        var approverDTO = _mapper.Map<PRApproversDTO>(approver);
                        purchaseRequisitionDTO.Approvers.Add(approverDTO);
                    }

                    result.Response.Add(purchaseRequisitionDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new PurchaseRequisitionsResponse 
                { 
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR 
                };
            }
        }
        public PurchaseRequisitionsResponse GetMyPurchaseRequsition()
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                var result = new PurchaseRequisitionsResponse();
                var purchaseRequsitions = _prRepository.Where(x => x.RequestedBy == person.Id && x.RecordStatus == RecordStatus.Active)
                    .OrderByDescending(x=>x.RegisteredDate)
                    .Include(x => x.PurchaseGroup)
                    .Include(x => x.RequirmentPeriod)
                    .Include(x => x.ProcurementSection)
                    .Include(x => x.CostCenter)
                    .Include(x => x.Requester)
                    .Include(x => x.Rejector)
                    .Include(x => x.AssignedAgent)
                    .Include(x => x.Assigner)
                    .ToList();


                foreach (var purchaseRequsition in purchaseRequsitions)
                {
                    var purchaseRequisitionDTO = _mapper.Map<PurchaseRequisitionDTO>(purchaseRequsition);                
                    purchaseRequisitionDTO.DelegateTeam = new List<PRDelegateTeamDTO>();
                    purchaseRequisitionDTO.Approvers = new List<PRApproversDTO>();
                    var delegateTeams = _prDelegateTeamRepository.Where(x => x.PurchaseRequisitionId == purchaseRequsition.Id && x.RecordStatus == RecordStatus.Active).ToList();
                    var approvers = _prApproversRepository.Where(x => x.PurchaseRequisitionId == purchaseRequsition.Id && x.RecordStatus == RecordStatus.Active).OrderBy(x => x.Order).ToList();
                    foreach (var delegat in delegateTeams)
                    {
                        var delegateMember = new PRDelegateTeamDTO();
                        var member = _personRepository.Find(delegat.PersonId.Value);
                        delegateMember.Id = delegat.Id;
                        delegateMember.Person = _mapper.Map<PersonDTO>(member);
                        purchaseRequisitionDTO.DelegateTeam.Add(delegateMember);
                    }
                    foreach (var approver in approvers)
                    {
                        var approverDTO = _mapper.Map<PRApproversDTO>(approver);
                        purchaseRequisitionDTO.Approvers.Add(approverDTO);
                    }

                    result.Response.Add(purchaseRequisitionDTO);
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
        public async Task<PurchaseRequisitionResponse> Reject(RejectRequest request)
        {
            var purchaseRequest = _prRepository.Where(c => c.Id == request.RequestId).FirstOrDefault();
            if (purchaseRequest == null)
                return new PurchaseRequisitionResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                purchaseRequest.PRStatus = PRStatus.Rejected;
                purchaseRequest.RejectionRemark = request.Remark;
                purchaseRequest.AssignedBy = person.Id;
                purchaseRequest.AssignedTo =  person.Id;
                purchaseRequest.PRStatus = PRStatus.UnAssigned;
                purchaseRequest.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                purchaseRequest.LastUpdateDate = DateTime.UtcNow;
                if (_prRepository.Update(purchaseRequest))
                {
                    return new PurchaseRequisitionResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new PurchaseRequisitionResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new PurchaseRequisitionResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<PurchaseRequisitionResponse> SelfAssign(SelfAssignRequest request)
        {
            var purchaseRequest = _prRepository.Where(c => c.Id == request.RequestId).FirstOrDefault();
            if (purchaseRequest == null)
                return new PurchaseRequisitionResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                purchaseRequest.AssignedTo = person.Id;
                purchaseRequest.PRStatus = PRStatus.Assigned;
                purchaseRequest.AssignedBy =  person.Id;
                purchaseRequest.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                purchaseRequest.LastUpdateDate = DateTime.UtcNow;
                if (_prRepository.Update(purchaseRequest))
                {
                    return new PurchaseRequisitionResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new PurchaseRequisitionResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new PurchaseRequisitionResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<PurchaseRequisitionResponse> UnAssign(long  id)
        {
            var purchaseRequest = _prRepository.Where(c => c.Id == id).FirstOrDefault();
            if (purchaseRequest == null)
                return new PurchaseRequisitionResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var person = _userService.GetPerson(userName);
                purchaseRequest.AssignedTo = null;
                purchaseRequest.PRStatus = PRStatus.UnAssigned;
                purchaseRequest.AssignedBy =  person.Id;
                purchaseRequest.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                purchaseRequest.LastUpdateDate = DateTime.UtcNow;
                if (_prRepository.Update(purchaseRequest))
                {
                    return new PurchaseRequisitionResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new PurchaseRequisitionResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new PurchaseRequisitionResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<PurchaseRequisitionResponse> Update(PurchaseRequisitionRequest request)
        {
            var purchaseRequest = _prRepository.Where(x => x.Id == request.Id).FirstOrDefault();
            if (purchaseRequest == null)
                return new PurchaseRequisitionResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                purchaseRequest.RequestedGood = request.RequestedGood;
                purchaseRequest.ApprovedBudgetAmmount = request.ApprovedBudgetAmmount;
                purchaseRequest.PurchaseType = request.PurchaseType;
                purchaseRequest.PurchaseGroupId = request.PurchaseGroupId;
                purchaseRequest.RequirmentPeriodId = request.RequirementPeriodId;
                purchaseRequest.ProcurementSectionId = request.ProcurementSectionId;
                purchaseRequest.Division = request.Division;
                purchaseRequest.CostCenterId = request.CostCenterId;
                purchaseRequest.Specification = request.Specification;
                purchaseRequest.ExpectedDeliveryDate = request.ExpectedDeliveryDate;
                purchaseRequest.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                purchaseRequest.LastUpdateDate = DateTime.UtcNow;
                if (_prRepository.Update(purchaseRequest))
                {
                    return new PurchaseRequisitionResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new PurchaseRequisitionResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception)
            {
                return new PurchaseRequisitionResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        } 
        public async Task<PurchaseRequisitionResponse> UpdateSpecification(UpdateSpecificationRequest request)
        {
            var purchaseRequest = _prRepository.Where(x => x.Id == request.Id).FirstOrDefault();
            if (purchaseRequest == null)
                return new PurchaseRequisitionResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                purchaseRequest.Specification = request.Specification;
                purchaseRequest.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                purchaseRequest.LastUpdateDate = DateTime.UtcNow;
                if (_prRepository.Update(purchaseRequest))
                {
                    return new PurchaseRequisitionResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new PurchaseRequisitionResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception)
            {
                return new PurchaseRequisitionResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}