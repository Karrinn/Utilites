using Dapper;
using System.Data.SqlClient;
using System.Data;
using DataSynchronizationService.Properties;

namespace DataSynchronizationService.DAL.Sql.QueryExecute
{
    public abstract class BaseSqlStoredProcedureTVP
    {
        public abstract string GetSqlStredProcedureName();
        public abstract object GetSqlTVPparam();

        public void Execute(bool useMainConnStr = false)
        {
            ExecuteStoredProcedure(useMainConnStr);
        }

        private void ExecuteStoredProcedure(bool useMainConnStr)
        {
            var connStr = useMainConnStr ? AppConfiguration.MainConnectionString : AppConfiguration.StatisticConnectionString;
            //var connStr = DbConnections.StatisticConnectionString;

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Execute(GetSqlStredProcedureName(), GetSqlTVPparam(), commandType: CommandType.StoredProcedure);
                }
            }
            catch (System.Exception)
            {
                throw;
            }            
        }
    }
}
