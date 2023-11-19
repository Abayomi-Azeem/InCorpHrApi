using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Abstractions.Notification
{
    public interface IEmailService
    {
        bool SendNewMemberEmail(string firstName, string email);
        bool SendVerificationEmail(string firstName, string email);
        bool SendSuccessEmail(string firstName, string email, string stage, string nextLogInDate);
        void SendFailureMail(string firstName, string email, string stage);
    }
}
