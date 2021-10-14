using CargoProrationMicroservice.Models.ConfigurationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CargoProrationMicroservice.ResponseModel.Master.SPA
{
    public class SPAQuestionsResponse : OperationStatusResponse
    {
        public List<SPAQuestionsRes> Questions { get; set; }
        public SPAQuestionsResponse()
        {
            Questions = new List<SPAQuestionsRes>();
        }
    }
    public class SPAQuestionResponse : OperationStatusResponse
    {
        public SPAQuestionsRes Question { get; set; }
        public SPAQuestionResponse()
        {
            Question = new SPAQuestionsRes();
        }
    }
    public class SPAQuestionsRes
    {
        public long Id { get; set; }
        public string Question { get; set; }
        public bool IsApplicable { get; set; }
        public long SPAId { get; set; }       
    }
}
