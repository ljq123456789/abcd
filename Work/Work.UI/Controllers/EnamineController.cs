using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Enamine.Model;
using Enamine.BLL;
using Newtonsoft.Json;
using System.IO;

namespace Work.UI.Controllers
{
    public class EnamineController : Controller
    {
        /// <summary>
        /// 员工表
        /// </summary>
        ManageBll bl;
        public EnamineController(ManageBll bl) //依赖构造函数进行对象注入
        {
            this.bl = bl; //在构造函数中初始化EnamineController控制器类的bll属性 （这个bll属性的类型是ManageBll）
        }
        /// <summary>
        /// 登录主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.name = Session["Name"];
            return View();
        }
        /// <summary>
        /// 登录功能
        /// </summary>
        /// <param name="Wid">工号</param>
        /// <param name="Pwid">密码</param>
        /// <returns></returns>
        public int Logins(int Wid, int Pwid)
        {
            Manage m = bl.Select(Wid, Pwid);
            if (m != null)
            {
                Session["Id"] = m.Id;
                Session["Name"] = m.Name;
                Session["Depart"] = m.Depart;
                Session["Prower"] = m.PorwerId;
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// 请假信息页
        /// </summary>
        /// <returns></returns>
        public ActionResult Leaved()
        {
            ViewBag.id = Session["Id"];
            return View();
        }
        public string Leaveds(string statio, string start, string end, string uid)
        {
            return JsonConvert.SerializeObject(bl.Selleave(statio, start, end, uid));
        }
        public ActionResult Approve()
        {
            ViewBag.name = Session["Name"];
            ViewBag.dapart = Session["Depart"];
            return View();
        }
        [HttpPost]
        public int Adds()
        {
            try
            {
                HttpFileCollectionBase file = Request.Files;
                for (int i = 0; i < file.Count; i++)
                {
                    string PathName = Server.MapPath("/Img/");
                    string textPath = Path.Combine(PathName + file[i].FileName);
                    file[i].SaveAs(textPath);
                    Img im = new Img();
                    im.Picture = textPath;
                    im.Tid = 1;
                    im.Pid = 1;
                    bl.AddImg(im);
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public ActionResult Checked()
        {
            return View();
        }
    }
}