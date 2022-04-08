using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using log4net;
using log4net.Config;

namespace DataSynchronizationService.Properties
{
    public static class AppConfiguration
    {
        public static string MainConnectionString;
        public static string StatisticConnectionString;
        public static MailServer MailServer;
        public static ILog Log;
            
        public static void Init()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true);
            var config = builder.Build();

            MainConnectionString = config["MainConnectionString"];
            StatisticConnectionString = config["StatisticConnectionString"];

            int.TryParse(config["MailSenderConfig:Port"], out int port);
            bool.TryParse(config["MailSenderConfig:UseEncryption"], out bool useEncryption);

            MailServer = new MailServer(
                Uri: config["MailSenderConfig:Uri"],
                Port: port,
                Login: config["MailSenderConfig:Login"],
                Password: config["MailSenderConfig:Password"],
                Address: config["MailSenderConfig:Address"],
                Name: config["MailSenderConfig:Name"],
                UseEncryption: useEncryption
            );

            var log4NetConfigDirectory = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var log4NetConfigFilePath = Path.Combine(log4NetConfigDirectory, "log4net.xml");
            XmlConfigurator.ConfigureAndWatch(new FileInfo(log4NetConfigFilePath));
            Log = LogManager.GetLogger("error-logger");
        }
    }
}
