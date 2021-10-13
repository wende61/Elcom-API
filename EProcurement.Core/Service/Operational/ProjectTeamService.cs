using AutoMapper;
using EProcurement.DataObjects.Models.Operational;
using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.MasterData;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.Core.Interface.Operational;
using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.MasterData;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EProcurement.Core.Service.Operational
{
    public class ProjectTeamService : IProjectTeam<ProjectTeamResponse, ProjectTeamsResponse, ProjectTeamRequest>
    {

        private readonly IRepositoryBase<ProjectTeam> _projectTeamRepository;
        private readonly IRepositoryBase<Project> _projectRepository;
        private readonly IRepositoryBase<Person> _personRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbTransactionContext _appTransaction;
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public ProjectTeamService(IRepositoryBase<ProjectTeam> projectTeamRepository, IHttpContextAccessor httpContextAccessor, IAppDbTransactionContext appTransaction,
            IUserService userService, ILoggerManager logger, IMapper mapper, IRepositoryBase<Project> projectRepository, IRepositoryBase<Person> personRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _projectTeamRepository = projectTeamRepository;
            _projectRepository = projectRepository;
            _personRepository = personRepository;
            _appTransaction = appTransaction;
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ProjectTeamResponse> Create(ProjectTeamRequest request)
        {
            try
            {
                var project = _projectRepository.Find(request.Requests.FirstOrDefault().ProjectId.Value);
                if (project.IsBECMandatory == true)
                {
                    using (var uow = new AppUnitOfWork(_appTransaction.GetDbContext()))
                    {
                        var userName = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                        RepositoryBaseWork<ProjectTeam> projectTeamRepository = new RepositoryBaseWork<ProjectTeam>(uow);
                        using (var transaction = uow.BeginTrainsaction())
                        {
                            try
                            {
                                foreach (var pt in request.Requests)
                                {
                                    var projectTeam = _mapper.Map<ProjectTeam>(pt);
                                    projectTeam.StartDate = DateTime.Now;
                                    projectTeam.EndDate = DateTime.MaxValue;
                                    projectTeam.RegisteredDate = DateTime.Now;
                                    projectTeam.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                    projectTeam.RecordStatus = RecordStatus.Active;
                                    projectTeam.IsReadOnly = false;
                                    projectTeamRepository.Add(projectTeam);
                                }
                                if (await uow.SaveChangesAsync() > 0)
                                {
                                    transaction.Commit();
                                    return new ProjectTeamResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                                }
                                transaction.Rollback();
                                return new ProjectTeamResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR, BusinessErrorCode = "Unable to create Approvers" };
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                return new ProjectTeamResponse { Message = Resources.OperationEndWithError, Status = OperationStatus.ERROR, BusinessErrorCode = "Unable to save Request " };
                            }

                        }
                    }
                }
                else
                {
                    return new ProjectTeamResponse { Message = "The project Doesn't require BEC ", Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                return new ProjectTeamResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var projectTeam = await _projectTeamRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (projectTeam == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    if (_projectTeamRepository.Remove(projectTeam))
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
        public ProjectTeamsResponse GetByProjectId(long Id)
        {
            try
            {
                var result = new ProjectTeamsResponse();
                var projectTeams = _projectTeamRepository.Where(x => x.ProjectId == Id && x.RecordStatus == RecordStatus.Active).ToList();
                if (projectTeams == null)
                    return new ProjectTeamsResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                foreach (var pt in projectTeams)
                {
                    var project = _projectRepository.Find(pt.ProjectId.Value);
                    var person = _personRepository.Find(pt.PersonId.Value);
                    var projectTeamDTO = _mapper.Map<ProjectTeamResponseDTO>(pt);
                    projectTeamDTO.PersonDTO = _mapper.Map<PersonDTO>(person);
                    projectTeamDTO.ProjectDTO = _mapper.Map<ProjectDTO>(project);
                    result.Response.Add(projectTeamDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new ProjectTeamsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public ProjectTeamResponse GetById(long id)
        {
            try
            {
                var result = new ProjectTeamResponse();
                var projectTeam = _projectTeamRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (projectTeam == null)
                    return new ProjectTeamResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var project = _projectRepository.Find(projectTeam.ProjectId.Value);
                var person = _personRepository.Find(projectTeam.PersonId.Value);
                var projectTeamDTO = _mapper.Map<ProjectTeamResponseDTO>(projectTeam);
                projectTeamDTO.PersonDTO = _mapper.Map<PersonDTO>(person);
                projectTeamDTO.ProjectDTO = _mapper.Map<ProjectDTO>(project);
                result.Response = projectTeamDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new ProjectTeamResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
    }
}
