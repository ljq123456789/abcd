using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Work.Model
{
    public class DataContext:DbContext
    {
        public DataContext() : base("Default")
        {

        }
        public DbSet<Member> me { get; set; }
    }
}
