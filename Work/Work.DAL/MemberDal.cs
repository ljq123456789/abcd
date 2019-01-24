using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Model;
using Work.IDAL;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Work.DAL
{
    public class MemberDal : Idal<Member>
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
        public void Adds(DataTable dt)
        {
            DataContext context = new DataContext();
            context.Database.Connection.Open();
            EFBulkCopyHelper.SqlBulkCopyInsert(con, "Members", dt);
            context.Database.Connection.Close();
            //return context.SaveChanges();
        }

        public int Add(dynamic filename)
        {
            throw new NotImplementedException();
        }

        public List<Member> Show()
        {
            DataContext context = new DataContext();
            return context.me.ToList();
        }
    }
}
