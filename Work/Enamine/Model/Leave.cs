using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enamine.Model
{
    public class Leave
    {
        public int Id { get; set; }
        /// <summary>
        /// 绑定员工表
        /// </summary>
        public int ManageId { get; set; }
        /// <summary>
        /// 员工姓名--非本表字段
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 部门名称--非本表字段
        /// </summary>
        public string Depart { get; set; }
        /// <summary>
        /// 工号--非本表字段
        /// </summary>
        public string WId { get; set; }
        /// <summary>
        /// 请假开始时间
        /// </summary>
        public DateTime Start { get; set; } 
        /// <summary>
        /// 请假结束时间
        /// </summary>
        public DateTime Ends { get; set; }  
        /// <summary>
        /// 是由
        /// </summary>
        public string Reason { get; set; } 
        /// <summary>
        /// 备注
        /// </summary>
        public string Ramark { get; set; }
        /// <summary>
        /// 0:未提交，1：待审核(部门)，2：待审核(总经理)，3：已通过，4：已驳回 
        /// </summary>
        public int Statio { get; set; } 
        /// <summary>
        /// 请假时间
        /// </summary>
        public DateTime Qing { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime Shen { get; set; }
        /// <summary>
        /// 驳回理由
        /// </summary>
        public string Bo { get; set; }
    }
}