using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enamine.Model
{
    public class Manage
    {
        public int Id { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public int WId { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public int Pwid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Depart { get; set; }
        /// <summary>
        /// 权限id 0:无审核权限，1：本部门，2：全部
        /// </summary>
        public int PorwerId { get; set; }
    }
}
