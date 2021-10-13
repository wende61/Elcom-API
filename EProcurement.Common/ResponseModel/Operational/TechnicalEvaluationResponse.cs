using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.ResponseModel.Operational
{
    public class TechnicalEvaluationResponse:OperationStatusResponse
    {
        public TechnicalEvaluationDTO Response { get; set; }
        public TechnicalEvaluationResponse()
        {
            Response = new TechnicalEvaluationDTO();
        }

    } 
    public class TechnicalEvaluationsResponse:OperationStatusResponse
    {
        public List<TechnicalEvaluationDTO> Response { get; set; }
        public TechnicalEvaluationsResponse()
        {
            Response = new List<TechnicalEvaluationDTO>();
        }

    }
    public class CriterionResponse : OperationStatusResponse
    {
        public CriterionDTO Response { get; set; }
        public CriterionResponse()
        {
            Response = new CriterionDTO();
        }
    }
    public class CriterionsResponse : OperationStatusResponse
    {
        public List<CriterionDTO> Response { get; set; }
        public CriterionsResponse()
        {
            Response = new List<CriterionDTO>();
        }
    }
    public class CriteriaGroupResponse :OperationStatusResponse
    {
        public CriteriaGroupDTO Response { get; set; }
        public CriteriaGroupResponse()
        {
            Response = new CriteriaGroupDTO();
        }

    }
    public class CriteriaGroupsResponse :OperationStatusResponse
    {
        public List<CriteriaGroupDTO> Response { get; set; }
        public CriteriaGroupsResponse()
        {
            Response = new List<CriteriaGroupDTO>();
        }

    }
    public class TechnicalEvaluationDTO 
    {
        public long Id { get; set; }
        public string EvaluationName { get; set; }
        public List<CriteriaGroupDTO> CriteriaGroup { get; set; }
        public double CutOffPoint { get; set; }
        public double TechnicalEvaluationValue { get; set; }
        public double ProjectId { get; set; }
    }
    public class CriteriaGroupDTO
    {
        public long Id { get; set; }
        public string GroupName { get; set; }
        public double Sum { get; set; }
        public double TechnicalEvaluationId { get; set; }
        public List<CriterionDTO> Criteria { get; set; }
    }
    public class CriterionDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public MeasurmentTypes Measurment { get; set; }
        public string Value { get; set; }
        public Necessity Necessity { get; set; }
        public long CriteriaGroupId { get; set; }
    }
}
