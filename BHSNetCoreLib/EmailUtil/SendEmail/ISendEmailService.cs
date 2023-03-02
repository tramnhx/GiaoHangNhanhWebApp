using System.Threading.Tasks;

namespace BHSNetCoreLib.EmailUtil.SendEmail
{
    public interface ISendEmailService
    {
        Task SendEmail(EmailContent emailContent);
    }
}
