using EProcurement.Common;
using EProcurement.Common.RequestModel.MasterData;
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
    public class PersonController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<PersonResponse, PersonsResponse, PersonRequest> _person;
        private readonly IBulkInsertion<PersonResponse, PersonRequest> _persons;
        public PersonController(
            ICrud<PersonResponse, PersonsResponse, PersonRequest> person,
             IBulkInsertion<PersonResponse, PersonRequest> persons,
            ILoggerManager logger)
        {
            _logger = logger;
            _person = person;
            _persons = persons;
        }
        [HttpGet(nameof(GetAll))]
        public PersonsResponse GetAll()
        {
            return _person.GetAll();
        }
        [HttpGet(nameof(GetById))]
        public PersonResponse GetById(long id)
        {
            return _person.GetById(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<PersonResponse> Update(PersonRequest request)
        {
            return await _person.Update(request);
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<PersonResponse>> Create([FromBody] PersonRequest request)
        {
            var result = await _person.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpPost(nameof(BulkInsertion))]
        public async Task<ActionResult<PersonResponse>> BulkInsertion([FromBody] PersonRequests request)
        {
            var result = await _persons.BulkInsertion(request.Requests);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _person.Delete(id);
        }
    }
}
