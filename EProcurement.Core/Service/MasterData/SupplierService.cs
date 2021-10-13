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
using EProcurement.Common.ResponseModel;
using Microsoft.EntityFrameworkCore;
using EProcurement.DataObjects.Models.MasterData;

namespace EProcurement.Core.Service.MasterData
{
    public class SupplierService : ICrud<SupplierResponse, SuppliersResponse, SupplierRequest> , IBulkInsertion<SupplierResponse,SupplierRequest>, ISupplierRegistration<SupplierRegistrationRequest, SupplierResponse>, ISupplier
    {
        private readonly IRepositoryBase<Supplier> _supplierRepository;
        private readonly IRepositoryBase<VendorType> _vendorTypeRepository;
        private readonly IRepositoryBase<Country> _countryRepository;
        private readonly IRepositoryBase<BusinessCategory> _bcRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbTransactionContext _appTransaction;
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public SupplierService(IRepositoryBase<Supplier> supplierRepository, ILoggerManager logger, IHttpContextAccessor httpContextAccessor, IMapper mapper,IAppDbTransactionContext appTransaction,
            IRepositoryBase<VendorType> vendorTypeRepository, IUserService userService,
            IRepositoryBase<Country> countryRepository,
            IRepositoryBase<BusinessCategory> sbcRepository
            )
        {
            _vendorTypeRepository = vendorTypeRepository;
            _httpContextAccessor = httpContextAccessor;
            _supplierRepository = supplierRepository;
            _countryRepository = countryRepository;
            _appTransaction = appTransaction;
            _bcRepository = sbcRepository;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<SupplierResponse> Create(SupplierRequest request)
        {
            try
            {
                var supplier = _mapper.Map<Supplier>(request);
                supplier.StartDate = DateTime.Now;
                supplier.EndDate = DateTime.MaxValue;
                supplier.RegisteredDate = DateTime.Now;
                supplier.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                supplier.RecordStatus = RecordStatus.Active;
                supplier.IsReadOnly = false;
                var existingSupplier = _supplierRepository.FirstOrDefaultAsync(x => x.CompanyName == supplier.CompanyName && x.Website == supplier.Website && x.RecordStatus == RecordStatus.Active);
                if (existingSupplier.Result != null)
                    return new SupplierResponse { Message = "Supplier Already Existed", Status = OperationStatus.ERROR };
                if (_supplierRepository.Add(supplier))
                    return new SupplierResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                else
                    return new SupplierResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new SupplierResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<SupplierResponse> BulkInsertion(List<SupplierRequest> requests)
        {
            try
            {
                using (var uow= new AppUnitOfWork(_appTransaction.GetDbContext()))
                {
                    RepositoryBaseWork<Supplier> supplierRepo = new RepositoryBaseWork<Supplier>(uow);
                    using (var transaction= uow.BeginTrainsaction())
                    {
                        try
                        {
                            foreach (var request in requests)
                            {
                                var supplier = _mapper.Map<Supplier>(request);
                                supplier.StartDate = DateTime.Now;
                                supplier.EndDate = DateTime.MaxValue;
                                supplier.RegisteredDate = DateTime.Now;
                                supplier.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                                supplier.RecordStatus = RecordStatus.Active;
                                supplier.IsReadOnly = false;
                                supplierRepo.Add(supplier);
                            }
                            if (await uow.SaveChangesAsync()>0)
                            {
                                transaction.Commit();
                                return new SupplierResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
                            }
                            transaction.Rollback();
                            return new SupplierResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return new SupplierResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new SupplierResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> Delete(long id)
        {
            try
            {
                var supplier = await _supplierRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (supplier == null)
                    return new OperationStatusResponse { Message = Resources.RecordDoesNotExist, Status = OperationStatus.ERROR };

                else
                {
                    supplier.RecordStatus = RecordStatus.Deleted;
                    if (_supplierRepository.Update(supplier))
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
        public SuppliersResponse GetAll()
        {
            try
            {
                var result = new SuppliersResponse();
                var suppliers = _supplierRepository.Where(x => x.RecordStatus == RecordStatus.Active).Include(X=>X.VendorType);
                foreach (var supplier in suppliers)
                {
                    Country country = new Country();
                    VendorType vendorType = new VendorType();
                    BusinessCategory supplyBusinessCategory = new BusinessCategory();
                    if (supplier.CountryId != null)
                         country = _countryRepository.Find(supplier.CountryId);
                    if (supplier.VendorTypeId != null)
                        vendorType = _vendorTypeRepository.Find(supplier.VendorTypeId.Value);
                  
                    var supplierDTO = _mapper.Map<SupplierDTO>(supplier);
                    supplierDTO.VendorType = _mapper.Map<VendorTypeDTO>(vendorType);
                    supplierDTO.Country = _mapper.Map<CountryDTO>(country);
                    result.Response.Add(supplierDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;

            }
            catch (Exception ex)
            {
                return new SuppliersResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public SupplierResponse GetById(long id)
        {
            try
            {
                var result = new SupplierResponse();
                var supplier = _supplierRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active)
                    .Include(x => x.VendorType)
                    .Include(x => x.SupplierBusinessCategories)
                    .Include(x => x.Country)
                    .FirstOrDefault();
                if (supplier == null)
                    return new SupplierResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
                var supplierDTO = _mapper.Map<SupplierDTO>(supplier);
                supplierDTO.BusinessCategories = new List<BusinessCategoryDTO>();
                foreach (var sbc in supplier.SupplierBusinessCategories)
                {
                    var businessCategory = _bcRepository.Where(x => x.Id == sbc.BusinessCategoryId).FirstOrDefault();
                    var businessCategoryDTO = _mapper.Map<BusinessCategoryDTO>(businessCategory);
                    supplierDTO.BusinessCategories.Add(businessCategoryDTO);
                }
                result.Response = supplierDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;
            }
            catch (Exception ex)
            {
                return new SupplierResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
        public async Task<SupplierResponse> Update(SupplierRequest request)
        {

            var supllier = _supplierRepository.Where(c => c.Id == request.Id).FirstOrDefault();
            if (supllier == null)
                return new SupplierResponse { Status = OperationStatus.ERROR, Message = Resources.RecordDoesNotExist };
            try
            {
                supllier.CompanyName = request.CompanyName;
                supllier.ContactEmail = request.ContactEmail;
                supllier.ContactPhoneNumber = request.ContactPhoneNumber;
                supllier.ContactTelNumber = request.ContactTelNumber;
                supllier.ContactPerson = request.ContactPerson;
                supllier.ZipCode = request.ZipCode;
                supllier.Website = request.Website;
                supllier.CountryId = request.CountryId;
                supllier.City = request.City;
                supllier.Address = request.Address;
                supllier.SupplyCategoryDescription = request.SupplyCategoryDescription;
                supllier.VendorTypeId = request.VendorTypeId;
                supllier.StarType = request.StarType;
                supllier.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                supllier.LastUpdateDate = DateTime.UtcNow;
                if (_supplierRepository.Update(supllier))
                {
                    return new SupplierResponse
                    {
                        Message = Resources.OperationSucessfullyCompleted,
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new SupplierResponse
                    {
                        Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new SupplierResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }


        public async Task<SupplierResponse> RegisterAsync(SupplierRegistrationRequest request)
        {

            return new SupplierResponse { Message = "Supplier Already Existed", Status = OperationStatus.ERROR };


            //try
            //{
            //    using (var uow = new AppUnitOfWork(_appTransaction.GetDbContext()))
            //    {
            //        RepositoryBaseWork<Supplier> supplierRepo = new RepositoryBaseWork<Supplier>(uow);
            //        using (var transaction = uow.BeginTrainsaction())
            //        {
            //            try
            //            {
            //                var supplier = _mapper.Map<Supplier>(request);
            //                supplier.SupplierBusinessCategories = new List<SupplierBusinessCategory>();
            //                supplier.StartDate = DateTime.Now;
            //                supplier.EndDate = DateTime.MaxValue;
            //                supplier.RegisteredDate = DateTime.Now;
            //                supplier.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
            //                supplier.RecordStatus = RecordStatus.Active;
            //                supplier.IsReadOnly = false;
            //                foreach (var businessCategoryId in request.SupplyBusinessCategoryIds)
            //                {
            //                    var supplierBussinesCategory = new SupplierBusinessCategory();
            //                    supplierBussinesCategory.SupplierId = supplier.Id;
            //                    supplierBussinesCategory.BusinessCategoryId = businessCategoryId;
            //                    supplier.SupplierBusinessCategories.Add(supplierBussinesCategory);
            //                }
            //                var existingSupplier = _supplierRepository.FirstOrDefaultAsync(x => x.CompanyName == supplier.CompanyName && x.Website == supplier.Website && x.RecordStatus == RecordStatus.Active);
            //                if (existingSupplier.Result != null)
            //                    return new SupplierResponse { Message = "Supplier Already Existed", Status = OperationStatus.ERROR };
            //                if (supplierRepo.Add(supplier))
            //                {
            //                    request.UserRequest.PersonId = null;
            //                    request.UserRequest.SupplierId = supplier.Id;
            //                    if (_userService.Create(request.UserRequest).Result.Status == OperationStatus.SUCCESS)
            //                    {
            //                        if (await uow.SaveChangesAsync() > 0)
            //                        {
            //                            transaction.Commit();
            //                            return new SupplierResponse { Message = Resources.OperationSucessfullyCompleted, Status = OperationStatus.SUCCESS };
            //                        }
            //                        transaction.Rollback();
            //                        return new SupplierResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            //                   // }
            //                    transaction.Rollback();
            //                    return new SupplierResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            //                }
            //                transaction.Rollback();
            //                return new SupplierResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };

            //            }
            //            catch (Exception ex)
            //            {
            //                return new SupplierResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            //            }
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    return new SupplierResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            //}
        }
        public SuppliersResponse GetForInvitation()
        {
            try
            {
                var result = new SuppliersResponse();
                var suppliers = _supplierRepository.Where(x => x.RecordStatus == RecordStatus.Active)
                    .Include(x => x.VendorType)
                    .Include(x => x.SupplierBusinessCategories)
                    .Include(x => x.Country);
                foreach (var supplier in suppliers)
                {
                    var categories = "";
                    foreach (var sbc in supplier.SupplierBusinessCategories)
                    {
                        var businessCategory = _bcRepository.Find(sbc.BusinessCategoryId);
                        categories=categories+ " ["+businessCategory.Category+"]";
                    }
                    var supplierDTO = _mapper.Map<SupplierDTO>(supplier);
                    supplierDTO.SupplyCategoryDescription = categories;
                    result.Response.Add(supplierDTO);
                }
                result.Status = OperationStatus.SUCCESS;
                result.Message = Resources.OperationSucessfullyCompleted;
                return result;

            }
            catch (Exception ex)
            {
                return new SuppliersResponse { Message = Resources.ErrorHasOccuredWhileProcessingYourRequest, Status = OperationStatus.ERROR };
            }
        }
    }
}
