using System.Linq;
using System.Collections.Generic;
using DataSynchronizationService.DAL.Sql.TVP;
using DataSynchronizationService.DAL.SqlTableDefinitions;
using DataSynchronizationService.DAL.Sql.QueryExecute;

namespace DataSynchronizationService.DAL.Sql.StoredProcedures
{
    public class MergeUsersTable : BaseSqlStoredProcedureTVP
    {
        private List<Users> userList;

        public MergeUsersTable(List<Users> userList)
        {
            this.userList = userList;
        }

        public override string GetSqlStredProcedureName()
        {
            return "[dbo].[DB.Users.Merge]";
        }

        public override object GetSqlTVPparam()
        {
            return new TVP<Users>("[dbo].[DB.UsersType]", userList).GetTVPparamForSqlSP();
        }
    }

    public class MergeUserProfilesTable : BaseSqlStoredProcedureTVP
    {
        private List<UserProfiles> userProfilesList;

        public MergeUserProfilesTable(List<UserProfiles> userProfilesList)
        {
            this.userProfilesList = userProfilesList;
        }

        public override string GetSqlStredProcedureName()
        {
            return "[dbo].[DB.UserProfiles.Merge]";
        }

        public override object GetSqlTVPparam()
        {
            return new TVP<UserProfiles>("[dbo].[DB.UserProfilesType]", userProfilesList).GetTVPparamForSqlSP();

        }
    }

    public class UpdateUsersTableSynchronizationBit : BaseSqlStoredProcedureTVP
    {
        private List<TableSynchronizationBitIds> idList;

        public UpdateUsersTableSynchronizationBit(List<Users> idList)
        {
            this.idList = idList.Select(s => new TableSynchronizationBitIds { Id = s.Id }).ToList();
        }

        public override string GetSqlStredProcedureName()
        {
            return "[dbo].[DB.UpdateUsersTableSynchronizationBitIds]";
        }

        public override object GetSqlTVPparam()
        {
            return new TVP<TableSynchronizationBitIds>("[dbo].[DB.TablesSynchronizationBitIdsType]", idList).GetTVPparamForSqlSP();
        }
    }
    public class UpdateUserProfilesTableSynchronizationBit : BaseSqlStoredProcedureTVP
    {
        private List<TableSynchronizationBitIds> idList;

        public UpdateUserProfilesTableSynchronizationBit(List<UserProfiles> idList)
        {
            this.idList = idList.Select(s => new TableSynchronizationBitIds { Id = s.Id }).ToList();
        }

        public override string GetSqlStredProcedureName()
        {
            return "[dbo].[DB.UpdateUserProfilesTableSynchronizationBitIds]";
        }

        public override object GetSqlTVPparam()
        {
            return new TVP<TableSynchronizationBitIds>("[dbo].[DB.TablesSynchronizationBitIdsType]", idList).GetTVPparamForSqlSP();
        }
    }
}
