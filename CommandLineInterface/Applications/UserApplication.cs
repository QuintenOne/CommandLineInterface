using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineInterface.Applications
{
    [Application("user")]
    public class UserApplication
    {
        [Method("a")]
        public static string multi(string name)
        {
            return "one";
        }

        [Method("b")]
        public static string multi(string name, string username)
        {
            return "two";
        }

        [Method("c")]
        public static string multi()
        {
            return "zero";
        }

    }
}
