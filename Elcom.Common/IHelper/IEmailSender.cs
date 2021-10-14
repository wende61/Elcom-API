using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elcom.Common
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string message, string subject, string[] toAddress, string[] ccAddress, string[] attachements);   
    }
}
