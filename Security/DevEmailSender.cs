using System;
using System.Net;
using System.Net.Mail;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.Security
{
    public class DevEmailSender : IPDL_EMAIL_SENDER<bool>
    {
        public bool SendEmailAsync(string toEmail, string subject = "", string htmlMessage = "", bool isHtmlBody = false)
        {
            try
            {
                var to = toEmail;
                var sub = subject;
                var body = htmlMessage;

                var mailMessage = new MailMessage();
                mailMessage.To.Add(to);
                mailMessage.From = new MailAddress("it.ho@pioneer-denim.com");
                mailMessage.Subject = sub;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isHtmlBody;

                var smtp = new SmtpClient("mail.pioneer-denim.com")
                {
                    Port = 587,
                    UseDefaultCredentials = true,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("it.ho@pioneer-denim.com", "PDL#It%ho%20")
                };

                ServicePointManager.ServerCertificateValidationCallback =
                    (s, certificate, chain, sslPolicyErrors) => true;
                smtp.Send(mailMessage);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool SendMultiEmailAsync(string toEmails, string ccEmails = null, string bccEmails = null, string subject = "", string htmlMessage = "", bool isHtmlBody = false)
        {
            try
            {
                var to = toEmails;
                var cc = ccEmails;
                var bcc = bccEmails;
                var sub = subject;
                var body = htmlMessage;

                var mailMessage = new MailMessage();

                foreach (var address in to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mailMessage.To.Add(address);
                }

                if (cc != null)
                    foreach (var address in cc.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mailMessage.CC.Add(address);
                    }

                if (bcc != null)
                    foreach (var address in bcc.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mailMessage.Bcc.Add(address);
                    }

                mailMessage.From = new MailAddress("it.ho@pioneer-denim.com");
                mailMessage.Subject = sub;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isHtmlBody;

                var smtp = new SmtpClient("mail.pioneer-denim.com")
                {
                    Port = 587,
                    UseDefaultCredentials = true,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("it.ho@pioneer-denim.com", "PDL#It%ho%20")
                };

                ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
                smtp.Send(mailMessage);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
