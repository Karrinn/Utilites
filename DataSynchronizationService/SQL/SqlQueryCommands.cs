using DataSynchronizationService.DAL.SqlTableDefinitions;
using DataSynchronizationService.DAL.Sql.QueryText;
using DataSynchronizationService.DAL.Sql.QueryExecute;

namespace DataSynchronizationService.DAL.Sql.QueryCommands
{
    public class GetUsersTableRecords : BaseSqlQuery<Users>
    {
        public override string SqlQuery()
        {
            return DataDBSqlQueryText.GetDBUsers;
        }
    }
    public class GetUserProfileTableRecords : BaseSqlQuery<UserProfiles>
    {
        public override string SqlQuery()
        {
            return DataDBSqlQueryText.GetDBUserProfiles;
        }
    }
}
