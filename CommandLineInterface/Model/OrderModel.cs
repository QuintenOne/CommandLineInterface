using System;

namespace CommandLineInterface.Model {
	class OrderModel : IModel {
		public string TableName { get { return this.GetType().Name.Replace("Model", "s"); } }
		public Guid Id { get; set; }
		public string Name { get; set; }
		public Guid ProductId { get; set; }
		public Guid CostumerId { get; set; }
		public int Quantity { get; set; }

		public OrderModel() {}

		public OrderModel(string name, Guid productId, Guid customerID, int quantity) {
			this.Id = new Guid();
			this.Name = Name;
			this.ProductId = productId;
			this.CostumerId = customerID;
			this.Quantity = quantity;
		}

		public OrderModel(Guid id, string name, Guid productId, Guid customerID, int quantity) {
			this.Id = new Guid();
			this.Name = Name;
			this.ProductId = productId;
			this.CostumerId = customerID;
			this.Quantity = quantity;
		}

		public static OrderModel FromDatabase(Guid id, string name, Guid productId, Guid customerID, int quantity) {
			return new OrderModel(id, name.Trim(), productId, customerID, quantity);
		}
	}
}