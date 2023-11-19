using InCorpApp.Application.Abstractions.Notification;
using InCorpApp.Contracts.Notification;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace InCorpApp.Infrastructure.Notification
{
    public sealed class EmailService: IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
        {
            _settings = settings.Value ?? throw new ArgumentNullException($"{nameof(EmailSettings)} cannot be null");
            _logger = logger;
        }

        public bool SendNewMemberEmail(string firstName, string email)
        {
            try
            {
                MailAddress to = new MailAddress(email);
                MailAddress from = new MailAddress(_settings.Sender.ToString());
                var mail = new MailMessage(from, to);
                mail.Subject = "Welcome";

                mail.Body = NewMemberTemplate.Replace("{firstName}", firstName);
                mail.IsBodyHtml = true;

                SendEmail(mail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("[SendNewMemberEmail] - Exception - {0}\n {1}", ex.Message, ex.StackTrace);
                return false;
            }

        }
        
        public bool SendVerificationEmail(string firstName, string email)
        {
            try
            {
                MailAddress to = new MailAddress(email);
                MailAddress from = new MailAddress(_settings.Sender.ToString());
                var mail = new MailMessage(from, to);
                mail.Subject = "Account Verified";

                mail.Body = VerificationTemplate.Replace("{firstName}", firstName);
                mail.IsBodyHtml = true;

                SendEmail(mail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("[SendVerificationEmail] - Exception - {0}\n {1}", ex.Message, ex.StackTrace);
                return false;
            }
        }

        public bool SendSuccessEmail(string firstName, string email, string stage, string nextLogInDate)
        {
            try
            {
                MailAddress to = new MailAddress(email);
                MailAddress from = new MailAddress(_settings.Sender.ToString());
                var mail = new MailMessage(from, to);
                mail.Subject = "Stage Passed";

                mail.Body = VerificationTemplate.Replace("{firstName}", firstName)
                                                .Replace("{passedStage}", stage)
                                                .Replace("{nextStageDate}", nextLogInDate);
                mail.IsBodyHtml = true;

                SendEmail(mail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("[SendVerificationEmail] - Exception - {0}\n {1}", ex.Message, ex.StackTrace);
                return false;
            }
        }

        public void SendFailureMail(string firstName, string email, string stage)
        {
            try
            {
                MailAddress to = new MailAddress(email);
                MailAddress from = new MailAddress(_settings.Sender.ToString());
                var mail = new MailMessage(from, to);
                mail.Subject = "Stage Failed";

                mail.Body = VerificationTemplate.Replace("{firstName}", firstName)
                                                .Replace("{stage}", stage);
                mail.IsBodyHtml = true;

                SendEmail(mail);
            }
            catch (Exception ex)
            {
                _logger.LogError("[SendVerificationEmail] - Exception - {0}\n {1}", ex.Message, ex.StackTrace);
            }
        }

        public void SendEmail(MailMessage mail)
        {
            using (var smtpClient = new SmtpClient(_settings.Host, _settings.Port))
                {
                    smtpClient.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mail);
                    smtpClient.Dispose();

                }
        }

        private static string NewMemberTemplate = @"<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""UTF-8"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <title>InCorp Welcomes You</title>
  </head>

  <body>
    <div class=""main-container""><p>Hi {firstName}.</p>
             <p>Welcome To InCorp.</p> 
            <p> ...</p>
         
    </div>
  </body>
</html>
";
        private static string VerificationTemplate = @"<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""UTF-8"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <title>Account Verified</title>
  </head>

  <body>
    <div class=""main-container""><p>Hi {firstName}.</p>
             <p>Your Account has been verified</p> 
            <p>You can now login and create jobs for your company</p>
         
    </div>
  </body>
</html>
";
        private static string SuccessMailTemplate = @"<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""UTF-8"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <title>Stage Passed</title>
  </head>

  <body>
    <div class=""main-container""><p>Hi {firstName}.</p>
             <p>Congratulations!! You passed the {passedStage} </p> 
            <p>Kindly Log in on {nextStageDate} to see the details for the next stage</p>
         
    </div>
  </body>
</html>
";

        private static string FailureMailTemplate = @"<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""UTF-8"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <title>Stage Failed</title>
  </head>

  <body>
    <div class=""main-container""><p>Hi {firstName}.</p>
             <p>We deeply regret that you did not meet the required points to pass {stage} </p> 
            <p>Thank you for your time</p>
         
    </div>
  </body>
</html>
";
    }
}
