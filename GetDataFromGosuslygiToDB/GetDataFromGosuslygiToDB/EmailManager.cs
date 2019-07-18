using System;
using System.Net.Mail;

namespace GetDataFromGosuslygiToDB
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
            foreach (var address in emailList.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries))
            {
                MailMessage mail = new MailMessage("noreply@insoc.ru", address, subject, body);

                SmtpClient client = new SmtpClient(_host, _port);
                client.Credentials = new System.Net.NetworkCredential(_mailUsr, _mailPswrd);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Send(mail);
            }
        }
    }
}
