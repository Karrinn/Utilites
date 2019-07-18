using Dapper;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace GetDataFromGosuslygiToDB
{
    internal class DbManager
    {
        private readonly string _sqlConn;

        public DbManager(string SqlConnectionString)
        {
            _sqlConn = SqlConnectionString;
        }

        /// <summary>
        ///     Метод заливает на сервер файл с помощью скалярной функции IE.IMP_File_Copy_Server
        /// </summary>
        /// <param name="FileNameFullPath">Полный путь на сервере с раширением файла, если в каталоге ошибка, файл не зальеться!</param>
        /// <param name="BinaryFile">Файл в бинарном формате</param>
        public bool LoadFile2DB(string fileNameFullPath, byte[] binaryFile, out string outStr)
        {
            try
            {
                if (string.IsNullOrEmpty(fileNameFullPath))
                {
                    outStr = $"Полный путь сохранения файла на сервере указан с ошибкой!\nПроверьте DestinationServerFileNamePath в файл-конфиге утилиты";
                    return false;
                }

                if (binaryFile == null)
                {
                    outStr = $"Передаваемый файл пуст!\nПроверьте скачиваемый файл по ссылке: {Properties.Settings.Default.URL}";
                    return false;
                }

                // вызывает ХП
                using (var connection = new SqlConnection(_sqlConn))
                {
                    connection.Open();

                    var affectedRows = connection.Execute($@"IE.IMP_File_Copy_Server",
                        new {Export = 0, FileName = fileNameFullPath, Content = binaryFile}, // параметры ф-ции
                        commandType: CommandType.StoredProcedure);

                    connection.Close();
                    outStr = null;
                    return true;
                }
            }
            catch (Exception e)
            {
                outStr = $"Ошибка при работе с БД!\n Проверьте настройки подключения к БД в конфиг-файле утилиты или проблемы с процедурой IE.IMP_File_Copy_Server!\n{e.Message}";
                return false;
            }
        }
    }
}