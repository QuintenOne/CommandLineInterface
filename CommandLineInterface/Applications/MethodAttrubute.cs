using System;

namespace CommandLineInterface.Applications {
	public class MethodAttribute : Attribute {
		public string name;
		public MethodAttribute(string name) => this.name = name;
	}
}