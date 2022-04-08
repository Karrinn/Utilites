namespace DataSynchronizationService.DAL.Sql.QueryText
{
    public static partial class DataDBSqlQueryText
    {
        public static string GetVrachirfUsers => @$"
                SELECT TOP 10000
                     [Id]
                    ,[Email]
                    ,[RegistrationDate]
                    ,[LastVisitDate]
                    ,[Status]
                    ,[Profile]
                    ,[SocialId]
                    ,[SocialType]
                    ,[NotificationEmailSended]
                    ,[OldStatus]
                    ,[EmailChanged]
                    ,[EmailStatus]
                    ,[EmailConfirmed]
                    ,[EmailConfirmedStatus]
                    ,[DoctorConfirmed]
                    ,[DoctorConfirmedStatus]
                    ,[LastVisitFeedDate]
                    ,[IsReadOnly]
                    ,[IsUseRedesigned]
                    ,[PreregisteredStatus]
                    ,[LastRegistrationStepComplete]
                FROM dbo.[DB.Users]
                WHERE [IsNeedSynchronization] = 1";
    }
}
