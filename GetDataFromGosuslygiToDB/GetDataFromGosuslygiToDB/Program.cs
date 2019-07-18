using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using GetDataFromGosuslygiToDB.Properties;

namespace GetDataFromGosuslygiToDB
{
    internal class Program
    {
        /// <summary>
        ///     Локальная папка, куда скачиваются заархивированный  .zip "реест реквизитов УК"
        /// </summary>
        public static string path2File = $@"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}";

        /// <summary>
        ///     Адресс откуда скачивается "реест реквизитов УК"
        /// </summary>
        public static string URL => Settings.Default.URL;

        /// <summary>
        ///     Путь к папке, на сервере db-srv.omnius.local, куда будет заливаться распакованный "реест реквизитов УК"
        ///     если такой папки нет, будет ошибка и файл не загрузиться
        /// </summary>
        public static string DestinationServerFileNamePath => Settings.Default.destinationServerFileNamePath;

        /// <summary>
        /// Список Email адресов куда будут отсылаться уведомления об ошибках
        /// </summary>
        public static string Emails => Settings.Default.emails;

        /// <summary>
        ///     Имя разархивированного файла "реест реквизитов УК"
        /// </summary>
        public static string UnzipedFileName { get; set; }

        public static string Host => Settings.Default.host;
        public static int Port => Settings.Default.port;
        public static string MailUser => Settings.Default.mailUser;
        public static string MailPassword => Settings.Default.mailPassword;
        public static string EmailList  => Settings.Default.emails;


        private static void Main(string[] args)
        {
            var connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            

            var dbm = new DbManager(connStr);
            var dm = new DownloadManager();
            var zm = new ZipManager();

            var success = false;
            var count = 2; // попытоки

            while (!success & (count > 0))
                try
                {
                    // качаем файл
                    dm.DownloadFileFromUrl(URL);
                    var downloadedFile = dm.DownloadedFileName;

                    // распаковываем файл
                    zm.ExtractZipFile(downloadedFile, null,
                        Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
                    UnzipedFileName = zm.UnzipedFileName;

                    var filePath = $@"{path2File}\{UnzipedFileName}";
                    if(IsFileChanged(filePath))
                        throw new Exception($"Структура файла изменилась! Поправьте процедуру которая анализирует файл на сервере!");

                    // отправляем на сервер
                    success = dbm.LoadFile2DB(DestinationServerFileNamePath + UnzipedFileName,
                        File.ReadAllBytes(filePath), out var outStr);

                    if (success)
                        File.AppendAllText("Gosulygi_Reestr_YK_log.txt", $"{Environment.NewLine}\n{DateTime.Now}: Выполненно успешно!");
                    else
                        throw new Exception(outStr);
                }
                catch (Exception e)
                {
                    var message = $"\n{DateTime.Now}: {e.Message}";
                    File.AppendAllText("Gosulygi_Reestr_YK_log.txt", message);
                    
                    var emailManager = new EmailManager(Port,Host,MailUser,MailPassword);
                    var subject = @"Ошибка сверки реквизитов УК!";

                    emailManager.SendEmail(EmailList, subject, message +$"\n\n\n---\nУтилита \"GosuslygiSyncUtility\\GetDataFromGosuslygiToDB\" на сервере ЛК (10.0.0.22)");

                    // в случае не удачи, пробуем еще раз, через 30 минут
                    Thread.Sleep(new TimeSpan(0, 30, 0));
                    success = false;
                    count--;
                }
        }

        /// <summary>
        /// Проверяем что в файле с госуслги не поменялся порядок столбцов,
        /// т.к. в случае изменений, в бд bulk insert csv файла может не корректно отработать
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsFileChanged(string filePath)
        {
            string line;
            using (var reader = new StreamReader(filePath, Encoding.GetEncoding("windows-1251")))
            {
                line = reader.ReadLine();
            }

            if (line.Replace(" ", "").ToLower() != Settings.Default.columnList.Replace(" ", "").ToLower())
            {
                Settings.Default.columnList = line;
                return true;
            }

            return false;
        }
    }
}