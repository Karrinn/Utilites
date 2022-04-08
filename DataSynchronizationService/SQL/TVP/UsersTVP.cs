using Dapper;
using System;
using System.Data;
using System.Collections.Generic;
using DataSynchronizationService.DAL.SqlTableDefinitions;

namespace DataSynchronizationService.DAL.Sql.TVP
{
    public class UsersTVP
    {
        private DataTable dt;
        
        public UsersTVP()
        {
            dt = new DataTable();
            foreach(var classProp in typeof(Users).GetProperties())
            {
                if (Nullable.GetUnderlyingType(classProp.PropertyType) != null)
                    dt.Columns.Add(classProp.Name, Nullable.GetUnderlyingType(classProp.PropertyType)).AllowDBNull = true;
                else
                    dt.Columns.Add(classProp.Name, classProp.PropertyType);
            }

        }

        /// <summary>
        /// Возвращает object параметр TVP для хранимой процедуры(имя параметра, имя TVP таблицы).
        /// </summary>
        /// <param name="dataTable">Таблица с данными для загрузки в TVP</param>
        /// <returns></returns>
        public object GetTVPparamForSqlSP(List<Users> dtUser)
        {
            foreach (Users dtUserRow in dtUser)
            {
                var dtRow = dt.NewRow();
                foreach (var classProp in typeof(Users).GetProperties())
                    dtRow[classProp.Name] = dtUserRow.GetPropValue(classProp.Name) ?? DBNull.Value;

                dt.Rows.Add(dtRow);
            }
            //       SP param name   dataSource 
            return new { ParInformationType = dt.AsTableValuedParameter("[dbo].[DB.UsersType]") };
        }
    }
}
