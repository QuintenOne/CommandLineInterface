using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineInterface.Applications
{
    public class MethodAttribute : Attribute
    {
        public string name;
        public MethodAttribute(string name) => this.name = name;
    }
}
