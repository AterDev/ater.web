using System.Net;
using System.Net.Mail;

namespace Application.Services;

/// <summary>
/// smtp 邮件发送服务
/// </summary>
public static class SmtpService
{
    public static SmtpClient Create(string host, int port, bool useSSL)
    {
        // set smtp client
        return new SmtpClient(host, port)
        {
            UseDefaultCredentials = false,
            EnableSsl = useSSL,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential("username", "password")
        };
    }
    public static SmtpClient SetCredentials(this SmtpClient client, string username, string password)
    {
        // set credentials
        client.Credentials = new NetworkCredential(username, password);
        return client;
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="client"></param>
    /// <param name="fromName"></param>
    /// <param name="fromEmail"></param>
    /// <param name="toEmail"></param>
    /// <param name="subject"></param>
    /// <param name="htmlMessage"></param>
    /// <returns></returns>
    public static Task SendEmailAsync(this SmtpClient client, string fromName, string fromEmail, string toEmail, string subject, string htmlMessage)
    {
        return client.SendMailAsync(
            new MailMessage(
                new MailAddress(fromEmail, fromName),
                new MailAddress(toEmail))
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            });
    }
}
