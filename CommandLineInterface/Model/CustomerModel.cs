using System;
using System.Collections.Generic;
using System.Dynamic;

namespace CommandLineInterface.Model {
	class CustomerModel : IModel
	{
		public string TableName { get; set; }
		public Guid Id { get; set; }
		public string Name { get; set; }
		
		public CustomerModel()
		{
			this.TableName = "Customers";
		}

		public CustomerModel(string name)
		{
			this.TableName = "Customers";
			this.Id = new Guid();
			this.Name = name;
		}
		
		public CustomerModel(Guid id, string name)
		{
			this.TableName = "Customers";
			this.Id = id;
			this.Name = name;
		}

		public static CustomerModel FromDatabase(Guid id, string name) {

			Console.WriteLine($"{id} - {name.Trim()}");

			return new CustomerModel(id, name.Trim());
		}
	}
}