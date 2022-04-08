using Dapper;
using System;
using System.Data;
using System.Collections.Generic;
using DataSynchronizationService.DAL.SqlTableDefinitions;

namespace DataSynchronizationService.DAL.Sql.TVP
{
    public class TVP<T>
    {
        private DataTable dt;

        private List<T> listOfData;

        private string tvpName;

        public TVP(string tvpName, List<T> listOfData)
        {
            this.tvpName = tvpName;
            this.listOfData = listOfData;

            dt = new DataTable();
            // заполняем таблицу колонками через рефлексию
            foreach (var classProp in typeof(T).GetProperties())
            {
                // некоторые колонки таблицы надо сделать AlowNull
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
        public object GetTVPparamForSqlSP()
        {
            foreach (var listOfDataElement in listOfData)
            {
                var dtRow = dt.NewRow();
                foreach (var classProp in typeof(T).GetProperties())
                    dtRow[classProp.Name] = listOfDataElement.GetPropValue(classProp.Name) ?? DBNull.Value;

                dt.Rows.Add(dtRow);
            }
            //       SP param name   dataSource 
            return new { ParInformationType = dt.AsTableValuedParameter(tvpName) };
        }
    }
}
