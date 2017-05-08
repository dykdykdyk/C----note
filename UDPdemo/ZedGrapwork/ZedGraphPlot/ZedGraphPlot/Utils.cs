using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZedGraphPlot
{
    class Utils
    {
        public static string getname(string name)
        {
            int counts = name.IndexOf(":");
            string tempname = name.Substring(0, counts);
            return tempname;
        }
        public static int getdata(string name)
        {
            int counts = name.IndexOf(":");
            string tempname = name.Substring(counts + 1);
            return int.Parse(tempname);
        }
    }
}
