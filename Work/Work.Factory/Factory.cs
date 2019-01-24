using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Work.IDAL;

namespace Work.Factory
{
    public class Factory<T>
    {
        public static T getClass(string typeName)
        {
            return (T)Assembly.Load("Work.DAL").CreateInstance("Work.DAL." + typeName);
        }
    }
}
