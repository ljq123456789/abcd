using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.IDAL
{
    public interface Idal<T>
    {
        int Add(dynamic filename);
        void Adds(DataTable dt);
        List<T> Show();
    }
}
