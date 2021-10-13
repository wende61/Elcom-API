using CargoProrationMicroservice.Models.ConfigurationModels;
using CargoProrationMicroservice.Models.DBModels.Master.ProvisioMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoProrationMicroservice.ResponseModel.Master.Provisio
{
    public class QuestionsResponse : OperationStatusResponse
    {
        public List<QuestionRes> QuestionReses { get; set; }
        public QuestionsResponse()
        {
            QuestionReses = new List<QuestionRes>();
        }
    }
    public class QuestionResponse : OperationStatusResponse
    {
        public QuestionRes QuestionRes { get; set; }
        public QuestionResponse()
        {
            QuestionRes = new QuestionRes();
        }
    }
    public class QuestionRes
    {
        public long Id { get; set; }
        public string Question { get; set; }
        public bool IsApplicable { get; set; }
        public long ProvisioId { get; set; }

    }
}
