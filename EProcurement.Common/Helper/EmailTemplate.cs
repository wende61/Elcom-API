using EProcurement.Common.IHelper;
using EProcurement.Common.ResponseModel.MasterData;
using EProcurement.Common.ResponseModel.Operational;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Common.Helper
{
    public class EmailTemplate : IEmailTemplate<HotelAccommodationDTO,PurchaseRequisitionDTO, PersonDTO, PersonDTO> 
    {
        public readonly IEmailSender _emailSender;
        public EmailTemplate(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public async Task<bool> HotelAccommedationAssign(PersonDTO assigner, PersonDTO assignedTo, HotelAccommodationDTO assignment)
        {
            try
            {
                var to = new string[] { assignedTo.Email };
                var cc = new string[] { assigner.Email };
                var message = @"Dear" + assignedTo.FirstName + " " + assignedTo.MiddleName + @"
                            You have assigned for the below project by " + assigner.FirstName + " " + assigner.MiddleName + @"
                            •	Request Name:" + assignment.RequestName + @"
                            •	Originating Section:" + assignment.OriginatingSection + @"
                            •	Hotel Service Type:" + assignment.HotelServiceType + @"
                            •	Station Code:" + assignment.Station.CityName + @"
                            •	Requested Date:" + assignment.Requester + @"
                            •	Requested By:" + assignment.Requester + @"
                        You may get the detail information about the project under “My Project” option on your E-procurement portal page (web portal URL) and you may start the process accordingly
                        Best Regards
                        ";
                await _emailSender.SendEmailAsync(message, "Hotel Accommodation ", to, cc,null);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> PurchaseRequisitionAssign(PersonDTO assigner, PersonDTO assignedTo, PurchaseRequisitionDTO assignment)
        {
            try
            {
                var to = new string[] { assignedTo.Email };
                var cc = new string[] { assigner.Email };
                var message = @"Dear" + assignedTo.FirstName + " " + assignedTo.MiddleName + @"
                            You have assigned for the below project by " + assigner.FirstName + " " + assigner.MiddleName + @"
                            •	Goods/Service Name: " + assignment.RequestedGood + @"
                            •	Project Group: " + assignment.PurchaseGroup.Group+ @"
                            •	Cost Center:" + assignment.CostCenter.CostCenterName  + @"
                            •	Requested Date:" + assignment.RequestDate  + @"
                            •	Requested By:" + assignment.Requester  + @"
                        You may get the detail information about the project under “My Project” option on your E-procurement portal page (web portal URL) and you may start the process accordingly
                        Best Regards
                        ";
                await _emailSender.SendEmailAsync(message, "Purchase Requisition ", to, cc, null);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
