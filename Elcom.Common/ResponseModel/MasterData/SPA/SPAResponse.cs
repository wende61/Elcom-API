using CargoProrationAPI.Common;
using CargoProrationMicroservice.Models.ConfigurationModels;
using CargoProrationMicroservice.Models.DBModels.Master;
using CargoProrationMicroservice.Models.DBModels.Master.LocationMaster;
using CargoProrationMicroservice.Models.DBModels.Master.SPA_Master;
using CargoProrationMicroservice.Models.DBModels.Master.SPAMaster;
using CargoProrationMicroservice.Models.Enums;
using CargoProrationMicroservice.ResponseModel.Master.SPA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CargoProrationMicroservice.ResponseModel.Master.SAP
{
    public class SPAsResponse : OperationStatusResponse
    {
        public List<SPARes> SPAsRes { get; set; }
        public  SPAsResponse()
        {
            SPAsRes = new List<SPARes>();
        }
    }
    public class SPAResponse : OperationStatusResponse
    {
        public SPARes SPARes { get; set; }
        public SPAResponse()
        {
            SPARes = new SPARes();
        }
    }
    public class SPARes
    {
        public long Id { get; set; }
        [Display(Name = "Carrier")]
        public long CarrierCode { get; set; }
        [Display(Name = "SPA No.")]
        public int SPANumber { get; set; }
        public string Description { get; set; }
        [Display(Name = "Reference No.")]
        public string ReferenceNumber { get; set; }
        [Display(Name = "New Priority")]
        public int Priority { get; set; }
        [Display(Name = "Sales/Uplift")]
        public SalesUplift SalesUplift { get; set; }
        public AWBIssueBy AWBIssuedBy { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        [Display(Name = "Issue Country")]
        public long? IssueCityCode { get; set; }
        [Display(Name = "MultiLateral")]
        public bool IsMultiLateral { get; set; }
        [Display(Name = "Must be Joint Carriage")]
        public bool IsJointCarriage { get; set; }
        [Display(Name = "3rd Carrier Can Participate")]
        public bool IsThirdCarrierCanParticipate { get; set; }
        [Display(Name = "ISC Applicable")]
        public bool IsISCApplicable { get; set; }
        [Display(Name = "ISC Percentage")]
        public double ISCPercentage { get; set; }
        [Display(Name = "Min. MPA/SPA")]
        public bool MinSPAorMPA { get; set; }
        [Display(Name = "Wgt. Break Check")]
        public bool WgtBreakCheck { get; set; }
        [Display(Name = "Billing Agreement Exist")]
        public bool IsBillingAgreementExist { get; set; }
        public MOP MOP { get; set; }
        [Display(Name = "Wgt. Applicable for freight")]
        public WeghitForFreight WgtApplicableforfreight { get; set; }
        [Display(Name = "Wgt. Applicable for othercharge")]
        public WeghitForFreight WgtApplicableforothercharge { get; set; }
        [Display(Name = "Old Priority")]
        public int OldPriority { get; set; }
        public List<SPAEmbargoPeriodRes> embargoPeriodRes { get; set; }
        public List<SPAMultilateralCarrierRes> SPAMultilateralCarrierRes { get; set; }
        public List<SPARateClassRes> RateClasses { get; set; }
        public List<SPAWeightBreaksRes> WeightBreaks { get; set; }
        public List<SPACommoditiesRes> commodities { get; set; }
        public List<SPARevenueShareRes> RevenueShares { get; set; }
        public List<SPAOtherchargesRes> Othercharges { get; set; }
        public List<SPAFlightsRes> Flights { get; set; }
        public List<SPAAgentRes> Agents { get; set; }
        public List<SPARoutingRes> Routings { get; set; }
        public List<AWBTypesRes> AWBTypes { get; set; }
        public List<SPAQuestionsRes> Questions { get; set; }
    }
}
