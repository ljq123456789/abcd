using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enamine.Model;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Enamine.DAL
{
    public class ManageDal
    {
        private readonly static string conn = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="Wid">工号</param>
        /// <param name="Pwid">密码</param>
        /// <returns></returns>
        public static Manage Select(int Wid,int Pwid)
        {
            try
            {
                using(IDbConnection con = new SqlConnection(conn))
                {
                    var pram = new DynamicParameters();
                    pram.Add("Wid", Wid);
                    pram.Add("Pwid", Pwid);
                    return con.Query<Manage>("Manage_login", pram, null, true, null, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取某用户的请假信息
        /// </summary>
        /// <param name="statio">请假状态0:未提交，1：待审核，2：已通过，3：已驳回</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <param name="uid">员工id</param>
        /// <returns></returns>
        public static List<Leave> Selleave(string statio, string start, string end, string uid)
        {
            try
            {
                using(IDbConnection con=new SqlConnection(conn))
                {
                    string sql = string.Format("select Leave.Statio,Manage.Depart,Manage.WId,Manage.Name,Leave.Qing,Leave.Shen,Leave.Bo from Leave join Manage on Leave.ManageId = Manage.Id where ManageId='{0}' order by Qing desc", uid);
                    if (statio != "" && start != "" && end != "")
                    {
                        sql = string.Format("select Leave.Statio,Manage.Depart,Manage.WId,Manage.Name,Leave.Qing,Leave.Shen,Leave.Bo from Leave join Manage on Leave.ManageId = Manage.Id where Start>'{0}' and Ends <'{1}' and ManageId='{2}' and Statio='{3}' order by Qing desc", start, end, uid, statio);
                    }
                    else if (statio != "")
                    {
                        sql = string.Format("select Leave.Statio,Manage.Depart,Manage.WId,Manage.Name,Leave.Qing,Leave.Shen,Leave.Bo from Leave join Manage on Leave.ManageId = Manage.Id where Statio='{0}' and ManageId= '{1}' order by Qing desc", statio, uid);
                    }
                    else if (start != "" && end != "")
                    {
                        sql = string.Format("select Leave.Statio,Manage.Depart,Manage.WId,Manage.Name,Leave.Qing,Leave.Shen,Leave.Bo from Leave join Manage on Leave.ManageId = Manage.Id where Start>'{0}' and Ends <'{1}' and ManageId= '{2}' order by Qing desc", start, end, uid);
                    }
                    List<Leave> cc = con.Query<Leave>(sql).ToList();
                    return cc;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="im"></param>
        /// <returns></returns>
        public static int AddImg(Img im)
        {
            try
            {
                using(IDbConnection con=new SqlConnection(conn))
                {
                    string sql = string.Format("insert into Img values('{0}','{1}','{2}')", im.Picture, im.Tid, im.Pid);
                    return con.Execute(sql);
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
