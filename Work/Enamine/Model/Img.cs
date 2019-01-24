using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enamine.Model
{
    public class Img
    {
        public int Id { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 图片父级表 0:用户，1:请假
        /// </summary>
        public int Tid { get; set; }
        /// <summary>
        /// 关联编号
        /// </summary>
        public int Pid { get; set; }
    }
}
