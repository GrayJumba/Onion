using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Numbers
{
    public static class NumberBuilder
    {
        public static bool isInterger(string name,string code)
        {
            if (Strings.NameBuilder.buildString(name) == code)
                return true;
            else return false;
        }
    }
}
