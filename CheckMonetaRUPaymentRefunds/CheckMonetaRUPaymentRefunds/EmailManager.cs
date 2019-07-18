using System;
using System.Net.Mail;

namespace CheckMonetaRUPaymentRefunds
{
    class EmailManager
    {
        private int _port;      // 25
        private string _host;   //smtp.yandex.ru
        private string _mailUsr;
        private string _mailPswrd;


        public EmailManager(int port, string host, string mailUsr, string mailPswrd)
        {
            _port = port;
            _host = host;
            _mailUsr = mailUsr;
            _mailPswrd = mailPswrd;
        }

        public void SendEmail(string emailList, string subject, string body)
        {
            var client = new SmtpClient(_host, _port)
            {
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential(_mailUsr, _mailPswrd),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            foreach (var address in emailList.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries))
            {
                var mail = new MailMessage("noreply@insoc.ru", address, subject, body);
                mail.IsBodyHtml = true;

                client.Send(mail);
            }
        }
    }
}
