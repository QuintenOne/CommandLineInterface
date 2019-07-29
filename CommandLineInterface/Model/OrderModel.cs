using System;

namespace CommandLineInterface.Model
{
	class OrderModel : IModel
	{
		public string TableName { get; set; }
		public Guid Id { get; set; }
		public string Name { get; set; }
		public Guid ProductId { get; set; }
		public Guid CostumerId { get; set; }
		public int Quantity { get; set; }

		public OrderModel()
		{
			this.TableName = "Orders";
		}

		public OrderModel(string name, Guid productId, Guid customerID, int quantity)
		{
			this.TableName = "Orders";
			this.Id = new Guid();
			this.Name = Name;
			this.ProductId = productId;
			this.CostumerId = customerID;
			this.Quantity = quantity;
		}
		public OrderModel(Guid id, string name, Guid productId, Guid customerID, int quantity)
		{
			this.TableName = "Orders";
			this.Id = new Guid();
			this.Name = Name;
			this.ProductId = productId;
			this.CostumerId = customerID;
			this.Quantity = quantity;
		}

		public static OrderModel FromDatabase(Guid id, string name, Guid productId, Guid customerID, int quantity)
		{

			Console.WriteLine($"{id}, {name}, {productId}, {customerID}, {quantity}");

			return new OrderModel(id, name.Trim(), productId, customerID, quantity);
		}
	}
}