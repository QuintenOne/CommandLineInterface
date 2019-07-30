using System;

namespace CommandLineInterface.Model {
	public interface IModel {
		string TableName { get; }
		Guid Id { get; set; }
		string Name { get; set; }
	}
}
