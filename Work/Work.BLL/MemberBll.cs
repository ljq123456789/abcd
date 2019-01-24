using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.IDAL;
using Work.Model;
using Work.Factory;
using System.Data;

namespace Work.BLL
{
    public class MemberBll
    {
        static string typename = "MemberDal";
        Idal<Member> bll = Factory<Idal<Member>>.getClass(typename);
        public List<Member> Show()
        {
            return bll.Show();
        }
        public void Adds(DataTable dt)
        {
            bll.Adds(dt);
        }
    }
}
