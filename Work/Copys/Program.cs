using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Copys
{
    class Program
    {
        static void Main(string[] args)
        {
            string sqlcon = "Data Source=.;User ID=sa;pwd=123456";
            SqlConnection conn = new SqlConnection(sqlcon);
            try
            {
                List<People> ll = Show();
                EFBulkCopyHelper.BulkInsert(conn, "dbo.People", ll);
                Console.WriteLine("添加成功");
            }
            catch
            {
                Console.WriteLine("添加失败");
            }
            Console.ReadKey();
        }
        public static List<People> Show()
        {
            List<People> list = new List<People>();
            for (var i = 0; i < 10; i++)
            {
                People p = new People();
                p.Id =i;
                p.Name = "a" + i;
                list.Add(p);
            }
            return list;
        }
    }
    public class People
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
