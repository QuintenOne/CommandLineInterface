using System;

namespace CommandLineInterface.Model {
	class CustomerModel : IModel {
		public string TableName { get { return this.GetType().Name.Replace("Model", "s"); } }
		public Guid Id { get; set; }
		public string Name { get; set; }

		public CustomerModel() {
		}

		public CustomerModel(string name) {
			this.Id = new Guid();
			this.Name = name;
		}

		public CustomerModel(Guid id, string name) {
			this.Id = id;
			this.Name = name;
		}

		public static CustomerModel FromDatabase(Guid id, string name) {
			return new CustomerModel(id, name.Trim());
		}
	}
}