using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Copys
{
    public class SqlBulkCopyHelper
    {
        /// <summary>
        /// 拷贝数据表到数据库中
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="dt"></param>
        public static void CopyTableToSql(string connectionString, DataTable dt)
        {
            using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
            {
                sqlbulkcopy.DestinationTableName = dt.TableName;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                }
                sqlbulkcopy.WriteToServer(dt);
            }
        }

        /// <summary>
        /// 拷贝DataSet里的多个表到数据库中
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="DS"></param>
        public static void CopyTableToSql(string connectionString, DataSet DS)
        {
            using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
            {
                foreach (DataTable item in DS.Tables)
                {
                    if (item.Rows.Count > 0)
                    {
                        sqlbulkcopy.ColumnMappings.Clear();
                        sqlbulkcopy.DestinationTableName = item.TableName;
                        foreach (DataColumn Column in item.Columns)
                        {
                            sqlbulkcopy.ColumnMappings.Add(Column.ColumnName, Column.ColumnName);
                        }
                        sqlbulkcopy.WriteToServer(item);
                    }
                }
            }
        }

    }
}
