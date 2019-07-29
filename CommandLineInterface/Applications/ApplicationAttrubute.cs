using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineInterface.Applications
{
	public class ApplicationAttribute : Attribute
	{
		public string name;

		public ApplicationAttribute(string name) => this.name = name;
	}
}