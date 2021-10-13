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
    public class ProcurementSectionController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly ICrud<ProcurementSectionResponse, ProcurementSectionsResponse, ProcurementSectionRequest> _crud;
        private readonly IProcurementSection<ProcurementSectionsResponse> _procurementSection;
        public ProcurementSectionController(
         ICrud<ProcurementSectionResponse, ProcurementSectionsResponse, ProcurementSectionRequest> crud, 
        ILoggerManager logger, IProcurementSection<ProcurementSectionsResponse> procurementSection)
        {
            _logger = logger;
            _crud = crud;
            _procurementSection = procurementSection;
        }

        [HttpGet(nameof(GetAll))]
        public ProcurementSectionsResponse GetAll()
        {
            return _crud.GetAll();
        }  
        [HttpGet(nameof(GetByRequirementPeriodId))]
        public ProcurementSectionsResponse GetByRequirementPeriodId(long id)
        {
            return _procurementSection.GetByRequirementPeriodId(id);
        }
        [HttpGet(nameof(GetById))]
        public ProcurementSectionResponse GetById(long id)
        {
            return _crud.GetById(id);
        }
        [HttpPut(nameof(Update))]
        public async Task<ProcurementSectionResponse> Update(ProcurementSectionRequest request)
        {
            return await _crud.Update(request);
        }
        [HttpPost(nameof(Create))]
        public async Task<ActionResult<ProcurementSectionResponse>> Create([FromBody] ProcurementSectionRequest request)
        {
            var result = await _crud.Create(request);
            if (result.Status == OperationStatus.SUCCESS)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        [HttpDelete(nameof(Delete))]
        public async Task<OperationStatusResponse> Delete(long id)
        {
            return await _crud.Delete(id);
        }
    }
}
