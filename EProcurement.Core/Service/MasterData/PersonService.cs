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
using System.Threading.Tasks;

namespace EProcurement.Core.Service.MasterData
{
    public class PersonService : ICrud<PersonResponse, PersonsResponse, PersonRequest>, IBulkInsertion<PersonResponse, PersonRequest>
    {
        private readonly IRepositoryBase<Person> _personRepository;
        private readonly IRepositoryBase<CostCenter> _costCenterRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbTransactionContext _appDbTransaction;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public PersonService(IRepositoryBase<Person> personRepository, ILoggerManager logger, IHttpContextAccessor httpContextAccessor, IMapper mapper,IAppDbTransactionContext appDbTransaction, IRepositoryBase<CostCenter> costCenterRepository)
        {
            _httpContextAccessor = httpContextAccessor; 
            _personRepository = personRepository;
            _costCenterRepository = costCenterRepository;
            _appDbTransaction = appDbTransaction;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PersonResponse> Create(PersonRequest request)
        {
            try
            {
                var person = _mapper.Map<Person>(request);
                person.StartDate = DateTime.Now;
                person.EndDate = DateTime.MaxValue;
                person.RegisteredDate = DateTime.Now;
                person.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                person.RecordStatus = RecordStatus.Active;
                person.IsReadOnly = false;
                var existingSupplier = _personRepository.FirstOrDefaultAsync(x => x.FirstName == person.FirstName && x.MiddleName == person.MiddleName && x.LastName == person.LastName && x.RecordStatus == RecordStatus.Active);
                if (existingSupplier.Result != null)
                    return new PersonResponse { Message = "Person/User Already Existed", Status = OperationStatus.ERROR };
                if (_personRepository.Add(person))
                    return new PersonResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new PersonResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new PersonResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<PersonResponse> BulkInsertion(List<PersonRequest> requests)
        {
            try
            {
                using (var uow = new AppUnitOfWork(_appDbTransaction.GetDbContext()))
                {
                    RepositoryBaseWork<Person> personRepo = new RepositoryBaseWork<Person>(uow);
                    using (var transaction=uow.BeginTrainsaction())
                    {
                        try
                        {
                            foreach (var request in requests)
                            {
                                var person = _mapper.Map<Person>(request);
                                person.StartDate = DateTime.Now;
                                person.EndDate = DateTime.MaxValue;
                                person.RegisteredDate = DateTime.Now;
                                person.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                person.RecordStatus = RecordStatus.Active;
                                person.IsReadOnly = false;
                                personRepo.Add(person);
                            }
                            if (await uow.SaveChangesAsync()>0)
                            {
                                transaction.Commit();
                                return new PersonResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                            }
                            transaction.Rollback();
                            return new PersonResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            return new PersonResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new PersonResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var person = await _personRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (person == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    person.RecordStatus = RecordStatus.Deleted;
                    if (_personRepository.Update(person))
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

        public PersonsResponse GetAll()
        {
            try
            {
                var result = new PersonsResponse();
                var Persons = _personRepository.Where(x => x.RecordStatus == RecordStatus.Active);
                foreach (var person in Persons)
                {
                    var costCenter = _costCenterRepository.Find(person.CostCenterId);
                    var personDOT = _mapper.Map<PersonDTO>(person);
                    personDOT.CostCenter = _mapper.Map<CostCenterDTO>(costCenter);
                    result.Response.Add(personDOT);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;

            }
            catch (Exception ex)
            {
                return new PersonsResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public  PersonResponse GetById(long id)
        {
            try
            {
                var result = new PersonResponse();
                var person = _personRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).FirstOrDefault();
                if (person == null)
                    return new PersonResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var costCenter = _costCenterRepository.Find(person.CostCenterId);
                var personDOT = _mapper.Map<PersonDTO>(person);
                personDOT.CostCenter = _mapper.Map<CostCenterDTO>(costCenter);
                result.Response = personDOT;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new PersonResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }

        public async Task<PersonResponse> Update(PersonRequest request)
        {

            var person = _personRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (person == null)
                return new PersonResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                person.FirstName = request.FirstName;
                person.EmployeeId = request.EmployeeId;
                person.MiddleName = request.MiddleName;
                person.LastName = request.LastName;
                person.Email = request.Email;
                person.ExtensionNumber = request.ExtensionNumber;
                person.Position = request.Position;
                person.CostCenterId = request.CostCenterId;
                person.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                person.LastUpdateDate = DateTime.UtcNow;
                if (_personRepository.Update(person))
                {
                    return new PersonResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new PersonResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new PersonResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
