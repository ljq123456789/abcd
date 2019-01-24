using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Work.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //第一步： 构造一个AutoFac的builder容器
            var con = new ContainerBuilder();
            //第二步：告诉AutoFac控制器工厂，控制器类的创建去哪些程序集中查找（默认控制器工厂是去扫描bin目录下的所有程序集）
            con.RegisterAssemblyTypes(Assembly.Load("Work.UI"));
            //第三步：告诉AutoFac容器，创建项目中的指定类的对象实例
            con.RegisterAssemblyTypes(Assembly.Load("Enamine"));
            //第三步：创建一个真正的AutoFac的工作容器
            IContainer _con = con.Build();
            //创建一个真正的AutoFac的工作容器
            DependencyResolver.SetResolver(new AutofacDependencyResolver(_con));
        }
    }
}
