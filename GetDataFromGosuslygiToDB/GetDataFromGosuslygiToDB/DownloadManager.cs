using System;
using System.IO;
using System.Net;
using System.Text;

namespace GetDataFromGosuslygiToDB
{
    internal class DownloadManager
    {
        public string DownloadedFileName { get; private set; }

        public void DownloadFileFromUrl(string Url)
        {
            var request = WebRequest.CreateHttp(Url);
            request.Method = "GET";
            request.Timeout = 3000;
            request.UserAgent = "Mozilla/5.0";

            try
            {
                var response = request.GetResponse();
                var buffer = new StringBuilder();

                using (var stream = response.GetResponseStream())
                {
                    using (var w = File.OpenWrite("Gosulygi_Reestr_YK.zip"))
                    {
                        stream.CopyTo(w);
                    }
                }

                DownloadedFileName = "Gosulygi_Reestr_YK.zip";
                response.Close();
            }
            catch (WebException e)
            {
                throw new Exception($"Ошибка при попытке скачать файл с сайта госуслуг!\n Проверьте ссылку на скачивание в конфиг файле утилиты!\n {e.Message}");
            }
        }
    }
}