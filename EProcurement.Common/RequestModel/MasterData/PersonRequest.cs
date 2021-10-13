using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Common.RequestModel.MasterData
{
    public class PersonRequest
    {
        public long Id { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ExtensionNumber { get; set; }
        public string Position { get; set; }
        public long CostCenterId { get; set; }
    }
    public class PersonRequests
    {
        public List<PersonRequest> Requests { get; set; }
    }
}
