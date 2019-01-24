using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.DAL
{
    public class EFBulkCopyHelper
    {
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T">泛型集合的类型</typeparam>
        /// <param name="conn">连接对象</param>
        /// <param name="tableName">将泛型集合插入到本地数据库表的表名</param>
        /// <param name="list">要插入大泛型集合</param>
        public static void BulkInsert<T>(SqlConnection conn, string tableName, IList<T> list)
        {
            using (var bulkCopy = new SqlBulkCopy(conn))
            {
                bulkCopy.BatchSize = list.Count;
                bulkCopy.DestinationTableName = tableName;

                var table = new DataTable();
                var props = TypeDescriptor.GetProperties(typeof(T))

                    .Cast<PropertyDescriptor>()
                    .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                    .ToArray();

                foreach (var propertyInfo in props)
                {
                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                }

                var values = new object[props.Length];
                foreach (var item in list)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    table.Rows.Add(values);
                }

                bulkCopy.WriteToServer(table);
            }
        }
        /// <summary>
        /// 使用SqlBulkCopy将DataTable中的数据批量插入数据库中
        /// </summary>
        /// <param name="strTableName">数据库中对应的表名</param>
        /// <param name="dtData">数据集</param>
        public static void SqlBulkCopyInsert(SqlConnection conn, string strTableName, DataTable dtData)
        {
            try
            {
                using (SqlBulkCopy sqlRevdBulkCopy = new SqlBulkCopy(conn))//引用SqlBulkCopy
                {
                    sqlRevdBulkCopy.DestinationTableName = strTableName;//数据库中对应的表名
                    sqlRevdBulkCopy.NotifyAfter = dtData.Rows.Count;//有几行数据
                    sqlRevdBulkCopy.WriteToServer(dtData);//数据导入数据库
                    sqlRevdBulkCopy.Close();//关闭连接
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}