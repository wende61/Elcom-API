using EProcurement.DataObjects.Models.Operational;
using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Operational;
using EProcurement.DataObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using EProcurement.DataObjects.Models.MasterData;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using EProcurement.Core.Interface.Helper;
using Microsoft.EntityFrameworkCore;
using CargoProrationAPI.DataObjects.Models.Operational;
using EProcurement.Common.ResponseModel;

namespace EProcurement.Core.Service.Operational
{
    public class TenderInvitationService : ITenderInvitation<TenderInvitationResponse, TenderInvitationRequest>
    {
        private readonly IMapper _mapper;
        private readonly IUserService _user;
        private readonly ILoggerManager _logger;
        private readonly IEmailTemplate _emailTemplate;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepositoryBase<Project> _projectRepository;
        private readonly IRepositoryBase<Supplier> _supplierRepository;
        private readonly IRepositoryBase<Person> _personRepository;
        private readonly IAppDbTransactionContext _appDbTransactionContext;
        private readonly IRepositoryBase<TenderInvitation> _tenderInvitationRepository;
        private readonly IFileHelper _fileHelper;
        private readonly IRepositoryBase<SupplierTenderInvitation> _supplierTenderInvitationRepository;
        private readonly IRepositoryBase<ShortListApproval> _shorListApprovalRepository;
        private readonly IRepositoryBase<ShortListedSupplier> _shorListedSupplierRepository;
        private readonly IRepositoryBase<SuppliersTenderProposal> _proposalRepository;
        public TenderInvitationService(IRepositoryBase<Project> projectRepository,
           IFileHelper fileHelper,
             IRepositoryBase<SuppliersTenderProposal> proposalRepository,
        IRepositoryBase<SupplierTenderInvitation> supplierTenderInvitationRepository,
            IRepositoryBase<TenderInvitation> tenderInvitationRepository,
            IRepositoryBase<ShortListApproval> shorListApprovalRepository,
            IRepositoryBase<ShortListedSupplier> shorListedSupplierRepository,
            IAppDbTransactionContext appDbTransactionContext,
            IRepositoryBase<Supplier> supplierRepository,
            IHttpContextAccessor httpContextAccessor,
            IEmailTemplate emailTemplate,
            ILoggerManager logger,
            IUserService user,
            IMapper mapper)
        {
            _proposalRepository = proposalRepository;
            _supplierTenderInvitationRepository = supplierTenderInvitationRepository;
            _fileHelper = fileHelper;
            _projectRepository = projectRepository;
            _tenderInvitationRepository = tenderInvitationRepository;
            _appDbTransactionContext = appDbTransactionContext;
            _supplierRepository = supplierRepository;
            _httpContextAccessor = httpContextAccessor;
            _emailTemplate = emailTemplate;
            _shorListApprovalRepository = shorListApprovalRepository;
            _shorListedSupplierRepository = shorListedSupplierRepository;
            _user = user;
            _logger = logger;
            _mapper = mapper;
        }
        public PostedBidsResponse GetPostedBids()
        {
            try
            {
                var result = new PostedBidsResponse();
                var tenders = _tenderInvitationRepository
                    .Where(x => x.RecordStatus == RecordStatus.Active && x.ResponseDueDate.AddDays(10) > DateTime.UtcNow && x.Project.ProjectProcessType == ProjectProcessType.OpenBid)
                    .Include(x => x.Project)
                    .ToList();
                if (tenders == null)
                    return new PostedBidsResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.EMPTY };

                foreach (var tender in tenders)
                {
                    var bidPost = new PostedBid();
                    bidPost.Id = tender.Id;
                    bidPost.ProjectName = tender.Project.ProjectName;
                    bidPost.ProjectCode = tender.Project.ProjectCode;
                    bidPost.Description = tender.Description;
                    bidPost.Deadline = tender.ResponseDueDate;
                    bidPost.PublishedDate = tender.RegisteredDate;
                    if (DateTime.UtcNow <= tender.ResponseDueDate)
                        bidPost.BidStatus = BidStatus.Open;
                    else
                        bidPost.BidStatus = BidStatus.Closed;
                    result.Response.Add(bidPost);
                }
                result.Message = Resources.OperationSucessfullyCompleted;
                result.Status = OperationStatus.SUCCESS;
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public PostedBidsResponse GetOpenBidsInternal()
        {
            try
            {
                var result = new PostedBidsResponse();
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var supplier = _user.GetSupplier(userName);
                var supplierTenders = _supplierTenderInvitationRepository.Where(x => x.RecordStatus == RecordStatus.Active && x.SupplierId == supplier.Id && x.IsInvited != true).ToList();
                var tenders = _tenderInvitationRepository
                   .Where(x => x.RecordStatus == RecordStatus.Active && x.ResponseDueDate.AddDays(10) > DateTime.UtcNow && x.Project.ProjectProcessType == ProjectProcessType.OpenBid)
                   .Include(x => x.Project)
                   .ToList();
                if (tenders == null)
                    return new PostedBidsResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.EMPTY };

                foreach (var tender in tenders)
                {
                    var interestedBids = supplierTenders.Where(x => x.TenderInvitationId == tender.Id).Count();
                    var bidPost = new PostedBid();
                    bidPost.Id = tender.Id;
                    bidPost.ProjectName = tender.Project.ProjectName;
                    bidPost.ProjectCode = tender.Project.ProjectCode;
                    bidPost.Description = tender.Description;
                    bidPost.Deadline = tender.ResponseDueDate;
                    bidPost.PublishedDate = tender.RegisteredDate;
                    bidPost.BidInterest = (interestedBids == 0 ? BidInterest.Pending : BidInterest.Interested);
                    if (DateTime.UtcNow <= tender.ResponseDueDate)
                        bidPost.BidStatus = BidStatus.Open;
                    else
                        bidPost.BidStatus = BidStatus.Closed;
                    result.Response.Add(bidPost);
                }
                result.Message = Resources.OperationSucessfullyCompleted;
                result.Status = OperationStatus.SUCCESS;
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<TenderInvitationResponse> Invite(TenderInvitationRequest request)
        {

            try
            {
                var teamEmails = new List<string>();
                var project = _projectRepository.Where(x => x.Id == request.ProjectId && x.RecordStatus == RecordStatus.Active).Include(x => x.ProjectTeams).FirstOrDefault();
                if (project == null)
                    return new TenderInvitationResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
                try
                {
                    using (var uow = new AppUnitOfWork(_appDbTransactionContext.GetDbContext()))
                    {
                        RepositoryBaseWork<TenderInvitation> tenderInvitationRepo = new RepositoryBaseWork<TenderInvitation>(uow);
                        using (var transaction = uow.BeginTrainsaction())
                        {
                            try
                            {
                                var tenderInvitation = _mapper.Map<TenderInvitation>(request);
                                tenderInvitation.Suppliers = new List<SupplierTenderInvitation>();
                                tenderInvitation.StartDate = DateTime.Now;
                                tenderInvitation.EndDate = DateTime.MaxValue;
                                tenderInvitation.RegisteredDate = DateTime.Now;
                                tenderInvitation.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                tenderInvitation.RecordStatus = RecordStatus.Active;
                                tenderInvitation.IsReadOnly = false;
                                if (project.ProjectProcessType != ProjectProcessType.OpenBid)
                                {
                                    foreach (var supplier in request.Suppliers)
                                    {
                                        var supplierTenderInvitation = new SupplierTenderInvitation();
                                        supplierTenderInvitation.SupplierId = supplier;
                                        supplierTenderInvitation.TenderInvitationId = tenderInvitation.Id;
                                        supplierTenderInvitation.IsInvited = true;
                                        supplierTenderInvitation.StartDate = DateTime.Now;
                                        supplierTenderInvitation.EndDate = DateTime.MaxValue;
                                        supplierTenderInvitation.RegisteredDate = DateTime.Now;
                                        supplierTenderInvitation.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                        supplierTenderInvitation.RecordStatus = RecordStatus.Active;
                                        supplierTenderInvitation.BidInterest = BidInterest.Pending;
                                        supplierTenderInvitation.IsReadOnly = false;
                                        tenderInvitation.Suppliers.Add(supplierTenderInvitation);
                                    }
                                }
                                if (tenderInvitationRepo.Add(tenderInvitation))
                                {
                                    if (_emailTemplate.InviteTender(project, request.Suppliers.ToList()).Result.Status == OperationStatus.SUCCESS)
                                    {
                                        if (await uow.SaveChangesAsync() > 0)
                                        {
                                            transaction.Commit();
                                            return new TenderInvitationResponse
                                            {
                                                Response = _mapper.Map<TenderInvitationDTO>(tenderInvitation),
                                                Message = Resources.OperationSucessfullyCompleted,
                                                Status = OperationStatus.SUCCESS
                                            };
                                        }
                                        transaction.Rollback();
                                        return new TenderInvitationResponse
                                        {
                                            Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                                            Status = OperationStatus.ERROR
                                        };

                                    }
                                }
                                transaction.Rollback();
                                return new TenderInvitationResponse
                                {
                                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                                    Status = OperationStatus.ERROR
                                };
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                return new TenderInvitationResponse
                                {
                                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                                    Status = OperationStatus.ERROR
                                };
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    return new TenderInvitationResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new TenderInvitationResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<OperationStatusResponse> ExpressInterestOnInvitationdBid(BidInterestRequest request) 
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var supplier = _user.GetSupplier(userName);
                var SupplierTenderInviation = _supplierTenderInvitationRepository.Where(x => x.Id == request.InvitationId && x.RecordStatus == RecordStatus.Active)
                    .Include(x => x.TenderInvitation)
                    .Include(x => x.TenderInvitation.Project)
                    .FirstOrDefault();
                if (SupplierTenderInviation == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.EMPTY };
                if (SupplierTenderInviation.TenderInvitation.Project.ProjectProcessType != ProjectProcessType.OpenBid)
                {
                    SupplierTenderInviation.BidInterest = request.BidInterest;
                    SupplierTenderInviation.Remark = request.Remark;
                    if (_supplierTenderInvitationRepository.Update(SupplierTenderInviation))
                    {
                        return new OperationStatusResponse
                        {
                            Message = Resources.OperationSucessfullyCompleted,
                            Status = OperationStatus.SUCCESS
                        };
                    }
                }
                return new OperationStatusResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
        public SupplierTenderInvitationResponse ShowBidInterests(long Id)
        {
            try
            {

                var result = new SupplierTenderInvitationResponse();
                var supplierInvitations = _supplierTenderInvitationRepository.Where(x => x.TenderInvitation.ProjectId == Id && x.RecordStatus == RecordStatus.Active)
                    .Include(x => x.Supplier)
                    .Include(x => x.TenderInvitation)
                    .Include(x => x.TenderInvitation.Project)
                    .ToList();
                if (supplierInvitations.Count() == 0)
                    return new SupplierTenderInvitationResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.EMPTY };
                foreach (var supplierInvitation in supplierInvitations)
                {
                    var supplierInvitationDTO = _mapper.Map<SupplierTenderInvitationDTO>(supplierInvitation);
                    result.Response.Add(supplierInvitationDTO);
                }
                result.TotalInvitedSupplier = result.Response.Count();
                result.TotalInterstedSupplier = result.Response.Where(x => x.BidInterest == BidInterest.Interested).Count();
                result.TotalPendingSupplier = result.TotalInvitedSupplier - result.TotalInterstedSupplier;
                result.Message = Resources.OperationSucessfullyCompleted;
                result.Status = OperationStatus.SUCCESS;
                return result;
            }
            catch (Exception ex)
            {
                return new SupplierTenderInvitationResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
        public PostedBidsResponse GetInvitationBids()
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var result = new PostedBidsResponse();
                var supplier = _user.GetSupplier(userName);
                var supplierTenders = _supplierTenderInvitationRepository.Where(x => x.RecordStatus == RecordStatus.Active && x.SupplierId == supplier.Id && x.IsInvited == true)
                    .Include(x => x.TenderInvitation)
                    .Include(x => x.TenderInvitation.Project)
                    .ToList();
                if (supplierTenders == null)
                    return new PostedBidsResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.EMPTY };
                foreach (var supplierTender in supplierTenders)
                {
                    var bidPost = new PostedBid();
                    bidPost.Id = supplierTender.Id;
                    bidPost.ProjectName = supplierTender.TenderInvitation.Project.ProjectName;
                    bidPost.ProjectCode = supplierTender.TenderInvitation.Project.ProjectCode;
                    bidPost.Description = supplierTender.TenderInvitation.Description;
                    bidPost.Deadline = supplierTender.TenderInvitation.ResponseDueDate;
                    bidPost.PublishedDate = supplierTender.TenderInvitation.RegisteredDate;
                    bidPost.BidInterest = supplierTender.BidInterest;
                    if (DateTime.UtcNow <= supplierTender.TenderInvitation.ResponseDueDate)
                        bidPost.BidStatus = BidStatus.Open;
                    else
                        bidPost.BidStatus = BidStatus.Closed;
                    result.Response.Add(bidPost);
                }
                result.Message = Resources.OperationSucessfullyCompleted;
                result.Status = OperationStatus.SUCCESS;
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<OperationStatusResponse> ExpressInterestOnOpenBid(BidInterestRequest request)
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var supplier = _user.GetSupplier(userName);
                var tenderInviation = _tenderInvitationRepository.Where(x => x.Id == request.InvitationId && x.RecordStatus == RecordStatus.Active)
                    .Include(x => x.Project)
                    .FirstOrDefault();
                if (tenderInviation == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.EMPTY };

                if (tenderInviation.Project.ProjectProcessType == ProjectProcessType.OpenBid || tenderInviation.Project.ProjectProcessType == ProjectProcessType.TwoStageBidding)
                {
                    var supplierInvitation = new SupplierTenderInvitation();
                    supplierInvitation.SupplierId = supplier.Id;
                    supplierInvitation.TenderInvitationId = request.InvitationId;
                    supplierInvitation.BidInterest = request.BidInterest;
                    supplierInvitation.IsInvited = false;
                    supplierInvitation.ResponseDate = DateTime.Now;
                    supplierInvitation.Remark = request.Remark;
                    supplierInvitation.StartDate = DateTime.Now;
                    supplierInvitation.EndDate = DateTime.MaxValue;
                    supplierInvitation.RegisteredDate = DateTime.Now;
                    supplierInvitation.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                    supplierInvitation.RecordStatus = RecordStatus.Active;
                    supplierInvitation.IsReadOnly = false;
                    if (_supplierTenderInvitationRepository.Add(supplierInvitation))
                    {
                        return new OperationStatusResponse
                        {
                            Message = Resources.OperationSucessfullyCompleted,
                            Status = OperationStatus.SUCCESS
                        };
                    }
                }
                return new OperationStatusResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<SupplierTenderInvitationResponse> ShortListSupplier(ShortListRequest request)
        {
            try
            {

                using (var uow = new AppUnitOfWork(_appDbTransactionContext.GetDbContext()))
                {
                    var shortListApprovalRepo = new RepositoryBaseWork<ShortListApproval>(uow);
                    try
                    {
                        using (var transaction = uow.BeginTrainsaction())
                        {
                            var shortListApproval = new ShortListApproval();
                            shortListApproval.ApprovalStatus = ApprovalStatus.Pending;
                            shortListApproval.TenderInvitationId = request.TenderInvitationId;
                            shortListApproval.StartDate = DateTime.Now;
                            shortListApproval.EndDate = DateTime.MaxValue;
                            shortListApproval.RegisteredDate = DateTime.Now;
                            shortListApproval.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                            shortListApproval.RecordStatus = RecordStatus.Active;
                            shortListApproval.IsReadOnly = false;
                            foreach (var sup in request.SupplierInvitationIds)
                            {
                                var shortListedSupplier = new ShortListedSupplier();
                                shortListedSupplier.ShortListApprovalId = shortListApproval.Id;
                                shortListedSupplier.SupplierTenderInvitationId = sup;
                                shortListedSupplier.StartDate = DateTime.Now;
                                shortListedSupplier.EndDate = DateTime.MaxValue;
                                shortListedSupplier.RegisteredDate = DateTime.Now;
                                shortListedSupplier.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                shortListedSupplier.RecordStatus = RecordStatus.Active;
                                shortListedSupplier.IsReadOnly = false;
                                shortListApproval.ShortListedSupplier.Add(shortListedSupplier);
                            }
                            foreach (var approver in request.Approvers)
                            {
                                var shortListApprover = new ShortListApprover();
                                shortListApprover.Approver = approver;
                                shortListApprover.ShortListApprovalId = shortListApproval.Id;
                                shortListApprover.ApprovalStatus = ApprovalStatus.Pending;
                                shortListApprover.IsApprover = true;
                                shortListApproval.ShortListApprover.Add(shortListApprover);
                            }
                            foreach (var carbonCopy in request.CarbonCopies)
                            {
                                var shortListApprover = new ShortListApprover();
                                shortListApprover.Approver = carbonCopy;
                                shortListApprover.ShortListApprovalId = shortListApproval.Id;
                                shortListApprover.ApprovalStatus = ApprovalStatus.Pending;
                                shortListApprover.IsApprover = false;
                                shortListApproval.ShortListApprover.Add(shortListApprover);
                            }
                            if (shortListApprovalRepo.Add(shortListApproval))
                            {
                                if (await uow.SaveChangesAsync() > 0)
                                {
                                    transaction.Commit();
                                    return new SupplierTenderInvitationResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                                }
                                transaction.Rollback();
                                return new SupplierTenderInvitationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                            }
                            transaction.Rollback();
                            return new SupplierTenderInvitationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new SupplierTenderInvitationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                    }
                }
            }
            catch (Exception ex)
            {
                return new SupplierTenderInvitationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public ShortListedResponses GetShortListedSuppliers(long projectId)
        {
            try
            {
                var result = new ShortListedResponses();
                var suppliers = _shorListedSupplierRepository
                    .Where(x => x.ShortListApproval.TenderInvitation.ProjectId == projectId && x.SupplierTenderInvitation.ShortListed == true && x.RecordStatus == RecordStatus.Active)
                    .Include(x => x.SupplierTenderInvitation)
                    .Include(x => x.SupplierTenderInvitation.Supplier)
                    .Include(x => x.SupplierTenderInvitation.TenderInvitation)
                    .ToList();
                if (suppliers.Count() == 0)
                    return new ShortListedResponses { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };
                foreach (var supplier in suppliers)
                {
                    var shorList = new ShortListedResponse();
                    shorList.Supplier = _mapper.Map<SupplierDTO>(supplier.SupplierTenderInvitation.Supplier);
                    shorList.ResponseDate = supplier.SupplierTenderInvitation.ResponseDate;
                    result.ShortListResponses.Add(shorList);
                }
                result.InvitationId = suppliers.Select(x => x.SupplierTenderInvitation.TenderInvitationId).FirstOrDefault();
                result.Message = Resources.OperationSucessfullyCompleted;
                result.Status = OperationStatus.SUCCESS;
                return result;
            }
            catch (Exception ex)
            {
                return new ShortListedResponses { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<SupplierTenderInvitationResponse> FloatRequestDocument(FloatRequest request)
        {
            try
            {
                using (var uow = new AppUnitOfWork(_appDbTransactionContext.GetDbContext()))
                {
                    var documentfloatationRepo = new RepositoryBaseWork<TenderInvitationFloat>(uow);
                    using (var transaction = uow.BeginTrainsaction())
                    {
                        foreach (var file in request.FileNames)
                        {
                            var documentFloatation = new TenderInvitationFloat();
                            documentFloatation.FileName = file;
                            documentFloatation.TenderInvitationId = request.TenderInvitationId;
                            documentfloatationRepo.Add(documentFloatation);
                        }
                        if (await uow.SaveChangesAsync() > 0)
                        {
                            transaction.Commit();
                            return new SupplierTenderInvitationResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                        }
                        transaction.Rollback();
                        return new SupplierTenderInvitationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                    }
                }
            }
            catch (Exception ex)
            {
                return new SupplierTenderInvitationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }

        }

        public ProposalResponses GetProposals()
        {
            try
            {
                var result = new ProposalResponses();
                var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var supplier = _user.GetSupplier(userName);
                var flotations = _supplierTenderInvitationRepository
                    .Where(x => x.SupplierId == supplier.Id /*&& x.ShortListed == true*/ && x.RecordStatus == RecordStatus.Active)
                    .Include(x => x.TenderInvitation)
                    .Include(x => x.TenderInvitation.TenderFloatations)
                    .Include(x => x.TenderInvitation.Project)
                    .Include(x => x.TenderInvitation.Project.RequestForDocument)
                    .ToList();
                foreach (var flotation in flotations)
                {
                    var proposal = new ProposalDTO();
                    proposal.Id = flotation.Id;
                    proposal.ProjectTitle = flotation.TenderInvitation?.Project?.ProjectName;
                    proposal.ProjectCode = flotation.TenderInvitation?.Project.ProjectCode;
                    proposal.PublishDate = flotation.TenderInvitation?.TenderFloatations?.FirstOrDefault()?.RequestDate;
                    proposal.Deadline = flotation.TenderInvitation?.Project?.BidClosingDate.Value;
                    proposal.Attachements = flotation.TenderInvitation.TenderFloatations?.Select(X => X.FileName).ToList();
                    proposal.DetailSubject = flotation.TenderInvitation?.Project?.RequestForDocument?.Subject;
                    proposal.DetailSubject = flotation.TenderInvitation?.Project?.RequestForDocument.Body;
                    result.Response.Add(proposal);
                }
                result.Message = Resources.OperationSucessfullyCompleted;
                result.Status = OperationStatus.SUCCESS;
                return result;
            }
            catch (Exception ex)
            {
                return new ProposalResponses { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest };
            }
        }
        public async Task<OperationStatusResponse> SubmitProposal(SubmitProposalRequest request)
        {
            try
            {
                using (var uow = new AppUnitOfWork(_appDbTransactionContext.GetDbContext()))
                {
                    var propsalRepo = new RepositoryBaseWork<SuppliersTenderProposal>(uow);
                    using (var transaction = uow.BeginTrainsaction())
                    {
                        var proposal = new SuppliersTenderProposal();
                        proposal.SupplierTenderInvitationId = request.ProposalId;
                        proposal.SubmitionDate = DateTime.Now;
                        if (request.FinancialAttachements == null || request.TechnicalAttachements == null)
                            return new OperationStatusResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { "Both Financial and technical attachement are mandatory." } };    
                        foreach (var financial in request.FinancialAttachements)
                        {
                            var fileResult = _fileHelper.Upload(financial);
                            if (fileResult.Result.Status == OperationStatus.SUCCESS)
                            {
                                var attachment = new SuppliersProposalAttachment();
                                attachment.FilePath = fileResult.Result.TrustedName;
                                attachment.DocumentType = AttachementType.Financial;
                                attachment.Seen = false;
                                attachment.SeenDate = DateTime.MinValue;
                                attachment.SuppliersTenderProposalId = proposal.Id;
                                proposal.Attachments.Add(attachment);
                            }
                            else
                            {
                                return fileResult.Result;
                            }
                        }
                        foreach (var technical in request.TechnicalAttachements)
                        {
                            var fileResult = _fileHelper.Upload(technical);
                            if (fileResult.Result.Status == OperationStatus.SUCCESS)
                            {
                                var attachment = new SuppliersProposalAttachment();
                                attachment.FilePath = fileResult.Result.TrustedName;
                                attachment.DocumentType = AttachementType.Technical;
                                attachment.Seen = false;
                                attachment.SeenDate = DateTime.MinValue;
                                attachment.SuppliersTenderProposalId = proposal.Id;
                                proposal.Attachments.Add(attachment);
                            }
                            else
                            {
                                return fileResult.Result;
                            }
                        }
                        propsalRepo.Add(proposal);
                        if (await uow.SaveChangesAsync() > 0)
                        {
                            transaction.Commit();
                            return new TechnicalEvaluationResponse
                            {
                                Message = Resources.OperationSucessfullyCompleted,
                                Status = OperationStatus.SUCCESS
                            };
                        }
                        transaction.Rollback();
                        return new TechnicalEvaluationResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                     
                    }
                    return new OperationStatusResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public SubmitedPropsalResponse GetSubmitedProposal(long Id)
        {
            try
            {
                var result = new SubmitedPropsalResponse();
                if (Id == 0)
                    return new SubmitedPropsalResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { "Unable to get submited Proposal" } };
                var propsalResponses = _proposalRepository.Where(x => x.SupplierTenderInvitation.TenderInvitation.ProjectId == Id )
                     .Include(x => x.Attachments)
                     .Include(x => x.SupplierTenderInvitation.Supplier)
                     .Include(x => x.SupplierTenderInvitation.TenderInvitation.Project).ToList();
                if (propsalResponses.Count()==0)
                    return new SubmitedPropsalResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { "0 supplier Response." } };
                foreach (var proposalResponse in propsalResponses)
                {
                    var proposal = new SubmitedProposalDTO();
                    proposal.Id = proposalResponse.Id;
                    proposal.SupplierName = proposalResponse.SupplierTenderInvitation.Supplier.CompanyName;
                    proposal.SubmitionDate = proposalResponse.SubmitionDate;
                    foreach (var attachment in proposalResponse.Attachments)
                    {
                        if (attachment.DocumentType== AttachementType.Financial)
                        {
                            proposal.FinancialAttachements.Add(attachment.FilePath);

                        }
                        if (attachment.DocumentType== AttachementType.Technical)
                        {
                            proposal.TechnicalAttachements.Add(attachment.FilePath);

                        }
                        result.Response.Add(proposal);
                    }
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;  
            }
            catch (Exception ex)
            {
                return new SubmitedPropsalResponse { Status = OperationStatus.ERROR, Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, MessageList = new List<string> { ex.Message } };
            }
        }
    }

}
