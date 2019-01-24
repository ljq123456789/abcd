using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Work.BLL;
using Newtonsoft.Json;
using Work.Model;
using System.IO;
using System.Data;

namespace Work.UI.Controllers
{
    public class MemberController : Controller
    {
        MemberBll bl = new MemberBll();
        /// <summary>
        /// 主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public string Show()
        {
            return JsonConvert.SerializeObject(bl.Show());
        }
        /// <summary>
        /// 上传
        /// </summary>
        /// <returns></returns>
        public int Upload()
        {
            try
            {
                HttpPostedFileBase file = Request.Files["Files"];
                string PathName = Server.MapPath("/Uploads/");
                string textPath = Path.Combine(PathName + file.FileName);
                file.SaveAs(textPath);
                DataTable dt = NPOIExcelHelper<Member>.Import(textPath);
                bl.Adds(dt);
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        public ActionResult Chu()
        {
            try
            {
                string PathName = Server.MapPath("/save/");
                string textPath = Path.Combine(PathName + "1.xls");
                List<Member> m = bl.Show();
                Dictionary<string, string> str = new Dictionary<string, string>();
                str.Add("Id", "编号");
                str.Add("Name", "姓名");
                str.Add("Sex", "性别");
                str.Add("Deadline", "使用期限");
                str.Add("Create", "创建日期");
                NPOIExcelHelper<Member>.ExportExcel(m, str, textPath);
                return Content("<script>alert('导出成功');location.href='/Member/Index'</script>");
            }
            catch
            {
                return Content("<script>alert('导出失败');location.href='/Member/Index'</script>");
            }
        }
        /// <summary>
        /// 添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            List<Item> li = new List<Item>();
            //li.AddRange(GetEnum());
            //li.AddRange(GetEnumSex());
            li.AddRange(GetEnumList<Deadline>());
            li.AddRange(GetEnumList<Sex>());
            return View(li);
        }
        /// <summary>
        /// 输出枚举值Sex
        /// </summary>
        /// <returns></returns>
        public List<Item> GetEnumSex()
        {
            List<Item> llist = new List<Item>();
            var list1 = Enum.GetValues(typeof(Sex));//得到枚举值
            foreach (var item in list1)
            {
                int id = (int)item;
                string name = item.ToString();
                Item it = new Item();
                it.id = id;
                it.Name = name;
                it.Type = "Sex";
                llist.Add(it);
            }
            return llist;
        }
        /// <summary>
        /// 输出枚举值Deadline
        /// </summary>
        /// <returns></returns>
        public List<Item> GetEnum()
        {
            List<Item> llist = new List<Item>();
            var list1 = Enum.GetValues(typeof(Deadline));//得到枚举值
            foreach (var item in list1)
            {
                int id = (int)item;
                string name = item.ToString();
                Item it = new Item();
                it.id = id;
                it.Name = name;
                it.Type = "Deadline";
                llist.Add(it);
            }
            return llist;
        }
        /// <summary>
        /// 泛型输出枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<Item> GetEnumList<T>()
        {
            List<Item> list = new List<Item>();
            Type _type = typeof(T);
            var _list = Enum.GetValues(_type);
            foreach (var item in _list)
            {
                Item it = new Item();
                it.id = (int)item;
                it.Name = item.ToString();
                it.Type = _type.Name;
                list.Add(it);
            }
            return list;
        }
        /// <summary>
        /// 期限
        /// </summary>
        public enum Deadline
        {
            一年 = 1, 两年 = 2, 三年 = 3, 终身 = 4, 其他 = 5,
        }
        /// <summary>
        /// 性别
        /// </summary>
        public enum Sex
        {
            男 = 1, 女 = 2, 未知 = 3
        }
    }
    /// <summary>
    /// 循环输出
    /// </summary>
    public class Item
    {
        public int id { get; set; } //id
        public string Name { get; set; }
        public string Type { get; set; } //枚举名
    }
}