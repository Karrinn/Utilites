using CheckMonetaRUPaymentRefunds.Properties;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CheckMonetaRUPaymentRefunds.FindAccountsList;
using Newtonsoft.Json;

namespace CheckMonetaRUPaymentRefunds
{
    class Program
    {
        /// <summary>
        /// Дата начала проверки
        /// </summary>
        public static DateTime DateFrom = string.IsNullOrEmpty(Settings.Default.dateFrom)
            ? DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0))
            : DateTime.Parse(Settings.Default.dateFrom);
        /// <summary>
        /// Дата конца проверки
        /// </summary>
        public static DateTime DateTo = string.IsNullOrEmpty(Settings.Default.dateTo)
            ? DateTime.Now
            : DateTime.Parse(Settings.Default.dateTo);
        /// <summary>
        ///  Логин от кабинета МонетаРУ
        /// </summary>
        public static string UserName = Settings.Default.Username;
        /// <summary>
        /// Пароль от кабинета МонетаРУ
        /// </summary>
        public static string Pswrd = Settings.Default.Password;

        // Почтовый сервер
        public static string Host => Settings.Default.host;
        public static int Port => Settings.Default.port;
        public static string MailUser => Settings.Default.mailUser;
        public static string MailPassword => Settings.Default.mailPassword;
        public static string EmailList => Settings.Default.emails;


        static void Main(string[] args)
        {

            var emailManager = new EmailManager(Port, Host, MailUser, MailPassword);
            var textFooter = $"\n\n\n---\nУтилита \"CheckMonetaRUPaymentRefunds\" на сервере ЛК (10.0.0.22)";

            try
            {
                var subject = "Проверка возврата средств в МонетаРУ!";

                var mr = new MonetaRuRequest();
                if (mr.FindOperationsListRequest(DateFrom, DateTo, username: UserName, password: Pswrd))
                {
                    var body = mr.ReportMessage;
                    emailManager.SendEmail(EmailList, subject, body + textFooter);
                }
                else
                {
                    var body = $"Операций со статусом \"refund\" не найдено.";
                    emailManager.SendEmail(EmailList, subject, body + textFooter);
                }
                File.AppendAllText("CheckMonetaRUPaymentRefunds.txt", $"\n{DateTime.Now} Выполненно.\n");
            }
            catch (Exception e)
            {
                var subject = "Ошибка проверки возврата средств в МонетаРУ!";

                emailManager.SendEmail(EmailList, subject, e.Message + textFooter);
                File.AppendAllText("CheckMonetaRUPaymentRefunds.txt", $"\n{DateTime.Now} Выполненно с ошибкой: {e.Message} \n");
            }

            // Мониторинг зависших платежей
            RunAsync().Wait();

        }

        private static async Task RunAsync()
        {
            FindAccountsListRequest findAccountsListRequest = new FindAccountsListRequest
            {
                Login = "",
                Password = ""
            };

            var findAccountsListResponse = await Requests.FindAccountsList(findAccountsListRequest);

            // если все ок, выходим
            if (findAccountsListResponse.Envelope.Body.FindAccountsListResponse.Account.Count == 0)
                return;

            var emailManager = new EmailManager(Port, Host, MailUser, MailPassword);
            var textFooter = $"\n\n\n---\nУтилита \"CheckMonetaRUPaymentRefunds\" на сервере ЛК (10.0.0.22)";
            var subject = "Мониторинг ЛС в кабинете МонетаРУ на наличие зависших платиежей";

            var body = new StringBuilder("Баланс счетов в кабинете МонетаРу\n\n");
            body.AppendLine("<div><p><table border='1' cellspacing='0' cellpadding='3'>");
            body.Append("<tr><th> Название счета </th><th> Баланс </th><th> Доступный баланс </th></tr>");

            foreach (var account in findAccountsListResponse.Envelope.Body.FindAccountsListResponse.Account)
            {
                if (account.AvailableBalance > 0 || account.Balance > 0)
                {
                    body.Append($"<tr><td> {account.Alias} </td><td> {account.Balance} </td><td> {account.AvailableBalance} </td></tr>");
                }
            }

            body.Append("</table><p></div>");
            body.AppendLine(textFooter);

            // Отсылаем уведомление если есть платежи
            emailManager.SendEmail(EmailList, subject, body.ToString().Normalize());
        }
    }
}
