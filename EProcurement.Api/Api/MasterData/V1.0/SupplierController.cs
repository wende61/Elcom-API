using EProcurement.Common;
using EProcurement.Common.RequestModel.MasterData;
using EProcurement.Common.ResponseModel;
using EProcurement.Common.ResponseModel.MasterData;
using EProcurement.Core.Interface.MasterData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EProcurement.Api.Api.MasterData.V1._0
{
    [ApiController]
    [Route("api/V1.0/[controller]")]
    public class SupplierController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<SupplierResponse, SuppliersResponse, SupplierRequest> _supplierCrud;
        private readonly ISupplierRegistration<SupplierRegistrationRequest, SupplierResponse> _supplierRegistraton;
        private readonly IBulkInsertion<SupplierResponse, SupplierRequest> _suppliers;
        private readonly ISupplier _supplier;
        public SupplierController(
            ICrud<SupplierResponse, SuppliersResponse, SupplierRequest> supplierCrud, ISupplierRegistration<SupplierRegistrationRequest, SupplierResponse> supplierRegistraton,
            IBulkInsertion<SupplierResponse, SupplierRequest> suppliers, ISupplier supplier,
            ILoggerManager logger)
        {
            _logger = logger;
            _supplierCrud = supplierCrud;
            _suppliers = suppliers;
            _supplier = supplier;
            _supplierRegistraton = supplierRegistraton;
        }
        [HttpGet(nameof(GetAll))]
        public SuppliersResponse GetAll()
        {
            return _supplierCrud.GetAll();
        }
        [HttpGet(nameof(GetForInvitation))]
        public SuppliersResponse GetForInvitation()
        {
            return _supplier.GetForInvitation();
        }
        [HttpGet(nameof(GetById))]
        public SupplierResponse GetById(long id)
        {
            return _supplierCrud.GetById(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<SupplierResponse> Update(SupplierRequest request)
        {
            return await _supplierCrud.Update(request);
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<SupplierResponse>> Create([FromBody] SupplierRequest request)
        {
            var result = await _supplierCrud.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpPost(nameof(Register))]
        public async Task<ActionResult<SupplierResponse>> Register([FromBody] SupplierRegistrationRequest request)
        {
            var result = await _supplierRegistraton.RegisterAsync(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpPost(nameof(BulkInsertion))]
        public async Task<ActionResult<SupplierResponse>> BulkInsertion([FromBody] SupplierRequests request)
        {
            var result = await _suppliers.BulkInsertion(request.Requests);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _supplierCrud.Delete(id);
        }
    }
}
