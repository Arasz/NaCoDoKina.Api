using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Services.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
