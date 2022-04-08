using appConfig = DataSynchronizationService.Properties.AppConfiguration;
using System;

namespace DataSynchronizationService
{
    class Program
    {
        static void Main(string[] args)
        {
            // Получаем ConnectionString и SMTP
            appConfig.Init();
            var SMTP = new SMTP.SMTPService(appConfig.MailServer);

            try
            {
                appConfig.Log.Info($"Синхронизация данных");
                // Запускаем процесс миграции данных
                DataMigrationProcess.Run();
            }
            catch (Exception exception)
            {
                appConfig.Log.Error($"Произошла ошибка:\n{exception}");
                SMTP.SendErrorMessage(exception);
            }
        }
    }

}
