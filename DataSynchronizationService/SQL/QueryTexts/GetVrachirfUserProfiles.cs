namespace DataSynchronizationService.DAL.Sql.QueryText
{
    public static partial class DataDBSqlQueryText
    {
        public static string GetVrachirfUserProfiles => @$"
                SELECT  TOP 10000
                     [profile].[Id]
                    ,[profile].[Name]
                    ,[profile].[SecondName]
                    ,[profile].[ThirdName]
                    ,[profile].[UseNickName]
                    ,[profile].[NickName]
                    ,[profile].[Gender]
                    ,[profile].[Birthday]
                    ,[profile].[Country]
                    ,[profile].[Region]
                    ,[profile].[City]
                    ,[profile].[Specialization]
                    ,[profile].[UseRegistrationEmail]
                    ,[profile].[WhoLookProfile]
                    ,[profile].[WhoWriteMessage]
                    ,[profile].[WhoLookContactInfo]
                    ,[profile].[WhoLookPersonalInfo]
                    ,[profile].[LookAdvertisment]
                    ,[profile].[NoticeNewMessage]
                    ,[profile].[NoticeAddCollegua]
                    ,[profile].[NoticePublicComment]
                    ,[profile].[NoticeReplyComment]
                    ,[profile].[NoticeColleagueTopic]
                    ,[profile].[NoticeColleagueBirthday]
                    ,[profile].[NoticeBirthday]
                    ,[profile].[NoticeHospital]
                    ,[profile].[NoticeGroupTopic]
                    ,[profile].[NoticeGroupsRequests]
                    ,[profile].[NoticeBlogTopic]
                    ,[profile].[NoticePeriodicallyIfAbsent]
                    ,[profile].[InviteCount]
                    ,[profile].[FirstSpecializationId]
                    ,S2.[Specialization] AS [SecondSpecializationId]
                    ,S3.[Specialization] AS [ThirdSpecializationId]
                    ,[profile].[Working]
                FROM dbo.[DB.User.Profiles] as [profile]
                LEFT JOIN (
	                SELECT  DISTINCT
			                S.[Profile] ,
			                S.[Specialization],
			                S.[Rank]
	                FROM dbo.[DB.User.Profiles.To.MedicalSpecializations] AS S
	                WHERE S.[Rank] = 2
	                ) S2 ON S2.[Profile] = [profile].[Id]
                LEFT JOIN (
	                SELECT  DISTINCT
			                S.[Profile] ,
			                S.[Specialization],
			                S.[Rank]
	                FROM dbo.[DB.User.Profiles.To.MedicalSpecializations] AS S
	                WHERE S.[Rank] = 3
	                ) S3 ON S3.[Profile] = [profile].[Id]
                WHERE [profile].IsNeedSynchronization = 1
            ";
    }
}
