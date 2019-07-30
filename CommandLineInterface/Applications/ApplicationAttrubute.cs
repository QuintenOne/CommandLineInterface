using System;

namespace CommandLineInterface.Applications {
	public class ApplicationAttribute : Attribute {
		public string name;

		public ApplicationAttribute(string name) => this.name = name;
	}
}