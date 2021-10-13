using EProcurement.Common;
using EProcurement.Core.Interface.Helper;
using EProcurement.DataObjects;
using EProcurement.DataObjects.Models.MasterData;
using EProcurement.DataObjects.Models.Operational;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Service.Helper
{
    public class EmailTemplateService :IEmailTemplate
    {
        private readonly IEmailSender _emailSender;
        private readonly IRepositoryBase<Person> _personRepository;
        private readonly IRepositoryBase<Supplier> _supplierRepository;
        public EmailTemplateService(IEmailSender emailSender, IRepositoryBase<Person> personRepository, IRepositoryBase<Supplier> supplierRepository)
        {
            _emailSender = emailSender;
            _personRepository = personRepository;
            _supplierRepository = supplierRepository;
        }
        public async Task<OperationStatusResponse> InviteTender(Project project, List<long> suppliersId)
        {
            try
            {
                var carbonCopies = new List<string>();
                carbonCopies.Add(_personRepository.Find(project.AssignedPerson.Value).Email);
                foreach (var team in project.ProjectTeams)
                {
                    carbonCopies.Add(_personRepository.Find(team.PersonId.Value).Email);
                }

                foreach (var supplierId in suppliersId)
                {
                    var supplier = _supplierRepository.Find(supplierId);
                    var recipent = new string[] { supplier.ContactEmail };
                    var message = @"
To:" + supplier.CompanyName + @" Alazar computer Center
Dear Sir/ Madam

Ethiopian Airlines Group hereby invites you to participate in its bid project for " + project.ProjectName + @"
Hence please go through Ethiopian Airlines Group E - Procurement site( https://www.eProcurment.ethiopianairlines.com) and confirm your interest to participate.

Wish you Luck.
Best regards                
";
                   //await _emailSender.SendEmailAsync(message, "Tender Invitation", recipent, (string[])carbonCopies.ToArray(), null); ;
                }
                return new OperationStatusResponse
                {
                    Message = Resources.OperationSucessfullyCompleted,
                    Status = OperationStatus.SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse
                {
                    Message = Resources.ErrorHasOccuredWhileProcessingYourRequest,
                    Status = OperationStatus.ERROR
                };
            }
        }

    }
}
