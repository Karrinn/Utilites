using System;
using System.Text;
using System.Net;
using System.Net.Mail;
using appConfig = DataSynchronizationService.Properties.AppConfiguration;

namespace DataSynchronizationService.SMTP
{
    public class SMTPService
    {
        private MailServer _mailServer;

        public SMTPService(MailServer mailServer)
        {
            _mailServer = mailServer;
        }

        public void SendErrorMessage(Exception exception)
        {
            var breakLine = "\n:::::\n";

            var builder = new StringBuilder("");
            builder.AppendLine($"exception.Message: {exception.Message}")
                   .AppendLine(breakLine)
                   .AppendLine($"exception.StackTrace: {exception.StackTrace}")
                   .AppendLine(breakLine);

            if (exception.InnerException != null)
            {
                exception = exception.InnerException;
                builder.AppendLine($"exception.Message: {exception.Message}")
                       .AppendLine(breakLine)
                       .AppendLine($"exception.StackTrace: {exception.StackTrace}")
                       .AppendLine(breakLine);
            }

            var subject = "Ошибка синхронизации данных для базы статистики";
            var body = builder.ToString();

            SendSingleMessage(subject, body);
        }

        public void SendSingleMessage(string subject, string body)
        {
            try
            {
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                using (var smtp = new SmtpClient(_mailServer.Uri, _mailServer.Port))
                {
                    var from = new MailAddress(_mailServer.Address, _mailServer.Name);
                    var to = new MailAddress("email_to_1@gmail.ru");
                    
                    #if DEBUG
                    to = new MailAddress("email_to_2@gmail.com");
                    #endif

                    var m = new MailMessage(from, to)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                        Priority = MailPriority.High
                    };

                    smtp.Credentials = new NetworkCredential(_mailServer.Login, _mailServer.Password);
                    smtp.EnableSsl = _mailServer.UseEncryption;
                    smtp.Send(m);
                }
            }
            catch (Exception ex)
            {
                appConfig.Log.Error($"Ошибка отправки емаил уведомления:\n{ex}");

            }
        }
    }

}
