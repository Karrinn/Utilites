using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using DataSynchronizationService.Properties;

namespace DataSynchronizationService.DAL.Sql.QueryExecute
{
    public abstract class BaseSqlQuery<T>
    {
        public abstract string SqlQuery();

        public List<T> Execute()
        {
            return ExecuteQuery();
        }

        private List<T> ExecuteQuery()
        {
            //var connStr = useMainConnStr ? AppConfiguration.MainConnectionString : AppConfiguration.StatisticConnectionString;
            var connStr = AppConfiguration.MainConnectionString;
            
            using (var conn = new SqlConnection(connStr))
            {
                var result = conn.Query<T>(SqlQuery()).ToList();
                
                return result;
            }
        }
    }
}
