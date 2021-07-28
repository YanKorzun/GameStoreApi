using System.Threading.Tasks;

namespace GameStore.BL.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string emailToSend, string subject, string message);
    }
}