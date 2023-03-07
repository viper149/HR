namespace DenimERP.ServiceInterfaces.BaseInterfaces
{
    public interface IPDL_EMAIL_SENDER<out T>
    {
        T SendEmailAsync(string toEmail, string subject = "", string htmlMessage = "", bool isHtmlBody = false);
        T SendMultiEmailAsync(string toEmails, string ccEmails, string bccEmails, string subject = "", string htmlMessage = "", bool isHtmlBody = false);
    }
}
