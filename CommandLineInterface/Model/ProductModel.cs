using System;
using Newtonsoft.Json;

namespace CommandLineInterface.Model {
	class ProductModel : IModel {
		public string TableName { get { return this.GetType().Name.Replace("Model","s") ; } }
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }

		public ProductModel() {}

		public ProductModel(string name, decimal price = 1) {
			this.Id = new Guid();
			this.Name = name;
			this.Price = price;
		}

		public ProductModel(Guid id, string name, decimal price = 1) {
			this.Id = id;
			this.Name = name;
			this.Price = price;
		}

		public static ProductModel FromDatabase(Guid id, string name, decimal price) {
			return new ProductModel(id, name.Trim(), price);
		}

		public override string ToString() {
			return JsonConvert.SerializeObject(this);
		}
	}
}