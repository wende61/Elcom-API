using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.RequestModel.Operational
{
    public class FinancialEvaluationRequest
    {
        public long Id { get; set; }
        public string EvaluationName { get; set; }
        public double FinancialEvaluationValue { get; set; }
        public long ProjectId { get; set; }
        public AwardFactor AwardFactor { get; set; }
        public List<FinancialCriteriaGroupRequest> FinancialCriteriaGroups { get; set; }
    }
    public class FinancialEvaluationUpdateRequest
    {
        public long Id { get; set; }
        public string EvaluationName { get; set; }
        public double FinancialEvaluationValue { get; set; }
        public long ProjectId { get; set; }

        public AwardFactor AwardFactor { get; set; }
    }
    public class FinancialCriteriaGroupRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Sum { get; set; }
        public long FinancialEvaluationId { get; set; }
        public List<FinancialCriteriaRequest> FinancialCriterias { get; set; }
    } 
    public class FinancialCriteriaGroupUpdateRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Sum { get; set; }
        public long FinancialEvaluationId { get; set; }
    }
    public class FinancialCriteriaRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public long FinancialCriteriaGroupId { get; set; }
        public List<FinancialCriteriaItemRequest> FinancialCriteriaItems { get; set; }
    }
    public class FinancialCriteriaUpdateRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public long FinancialCriteriaGroupId { get; set; }
    }
    public class FinancialCriteriaItemRequest
    {
        public long Id { get; set; }
        public string FiledName { get; set; }
        public string DataType { get; set; }
        public string Value { get; set; }
        public long FinancialCriteriaId { get; set; }
    }
}
