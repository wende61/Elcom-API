using System;
using System.Threading.Tasks;

namespace Elcom.Common.Helper
{
    public class EmailTemplate 
    {
        public readonly IEmailSender _emailSender;
        public EmailTemplate(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public async Task<bool> HotelAccommedationAssign()
        {
            try
            {
                var to = new string[] { "assignedTo.Email" };
                var cc = new string[] { "assigner.Email" };
                var message = @"Dear" + "Name" + " " + "Middel Name" + @"
                            You have assigned for the below project by " + "assigner.FirstName" + " " + "assigner.MiddleName" + @"
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
       
    }
}
