using Application.Common.DTOs.Mail;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Shared
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
        Task SendAsync(MailRequest request, string attachmentName, System.IO.Stream stream);
    }
}