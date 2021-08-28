using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Library.Data.Service {
    public static class MailService {
        private const string Email    = "librarybest01@gmail.com";
        private const string Password = "BestLibrary123";
        public static void SendMessage(string recipient, string subject, string body) {
            var client = new SmtpClient("smtp.gmail.com", 587) {
                Credentials = new NetworkCredential(Email, Password), EnableSsl = true
            };
            Task.Factory.StartNew(() => {
                client.Send(Email, recipient, subject, body);
            });
        }
    }
}