using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdentityServer.Services;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Task.Run(() =>
        {
            Console.WriteLine("do nothing");
        });
    }
}
