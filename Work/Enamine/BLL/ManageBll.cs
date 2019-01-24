using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enamine.DAL;
using Enamine.Model;

namespace Enamine.BLL
{
    public class ManageBll
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="Wid">工号</param>
        /// <param name="Pwid">密码</param>
        /// <returns></returns>
        public Manage Select(int Wid, int Pwid)
        {
            return ManageDal.Select(Wid, Pwid);
        }
        /// <summary>
        /// 获取某用户的请假信息
        /// </summary>
        /// <param name="statio">请假状态0:未提交，1：待审核，2：已通过，3：已驳回</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <param name="uid">员工id</param>
        /// <returns></returns>
        public List<Leave> Selleave(string statio, string start, string end, string uid)
        {
            return ManageDal.Selleave(statio, start, end, uid);
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="im"></param>
        /// <returns></returns>
        public int AddImg(Img im)
        {
            return ManageDal.AddImg(im);
        }
    }
}