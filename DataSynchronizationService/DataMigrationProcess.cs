using DataSynchronizationService.DAL.Sql.QueryCommands;
using DataSynchronizationService.DAL.Sql.StoredProcedures;
using appConfig = DataSynchronizationService.Properties.AppConfiguration;

namespace DataSynchronizationService
{
    public static class DataMigrationProcess
    {
        public static void Run()
        {
            DataTransferUsersTable();
            DataTransferUserProfilesTable();
        }

        private static void DataTransferUsersTable()
        {
            appConfig.Log.Info($"Синхронизация таблицы Users: получение данных");
            // Получаем данные из БД данных
            var usersTable = new GetUsersTableRecords().Execute();
            if (usersTable.Count == 0)
            {
                appConfig.Log.Info($"Синхронизация таблицы Users: новых данных нет, возврат");
                return;
            }

            appConfig.Log.Info($"Синхронизация таблицы Users: мердж данных");
            // Загружаем данные в БД статистики
            new MergeUsersTable(usersTable).Execute();
            appConfig.Log.Info($"Синхронизация таблицы Users: сброс флага синхронизации");
            // Update DataBD set IsNeedSynchronization = 0
            new UpdateUsersTableSynchronizationBit(usersTable).Execute(useMainConnStr: true);
        }

        private static void DataTransferUserProfilesTable()
        {
            appConfig.Log.Info($"Синхронизация таблицы UserProfiles: получение данных");
            // Получаем данные из БД данных
            var profilesTable = new GetUserProfileTableRecords().Execute();
            if (profilesTable.Count == 0)
            {
                appConfig.Log.Info($"Синхронизация таблицы UserProfiles: новых данных нет, возврат");
                return;
            }

            appConfig.Log.Info($"Синхронизация таблицы UserProfiles: мердж данных");
            // Загружаем данные в БД статистики
            new MergeUserProfilesTable(profilesTable).Execute();
            appConfig.Log.Info($"Синхронизация таблицы UserProfiles: сброс флага синхронизации");
            // Update DataBD set IsNeedSynchronization = 0
            new UpdateUserProfilesTableSynchronizationBit(profilesTable).Execute(useMainConnStr: true);
        }
    }
}
