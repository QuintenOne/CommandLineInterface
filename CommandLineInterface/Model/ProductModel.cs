using System;

namespace CommandLineInterface.Model
{
	class ProductModel : IModel
	{
		public string TableName { get; set; }
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		
		public ProductModel()
		{
			this.TableName = "Customers";
		}

		public ProductModel(string name, decimal price = 1)
		{
			this.TableName = "Products";
			this.Id = new Guid();
			this.Name = name;
			this.Price = price;
		}

		public ProductModel(Guid id, string name, decimal price = 1)
		{
			this.TableName = "Products";
			this.Id = id;
			this.Name = name;
			this.Price = price;
		}

		public static ProductModel FromDatabase(Guid id, string name, decimal price)
		{

			Console.WriteLine($"{id} - {name.Trim()} - {price}");

			return new ProductModel(id, name.Trim(), price);
		}

	}
}