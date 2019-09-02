using CommandLineInterface.Applications;
using CommandLineInterface.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CommandLineInterface {
	public class Program {
		public const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QDatabase;Integrated Security=True";

		static void Main(string[] args) {
			string command = Console.ReadLine();


			ProductModel product = new ProductModel("Jans Hesp", 31.45m);

			var select = Database.select<ProductModel>(new {
				product.Name,
				product.Price
			});

			Console.Write($"--- JSON Start ---\n {JsonConvert.SerializeObject(select, Formatting.Indented)} --- JSON END ---");

			Application.discover();

			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;

			while (CLI.keepRunning) {
				try {
					CLI.wait();
				} catch (Exception exception) {
					Console.BackgroundColor = ConsoleColor.Red;
					Console.WriteLine($"Error: {exception.Message}");
					Console.BackgroundColor = ConsoleColor.Black;
				}
			}
		}
	}

	public class CLI {
		public static List<Command> commands = new List<Command>();
		public static bool keepRunning = true;

		public static void wait() {
			string command = Console.ReadLine();

			if (Regex.Match(command, "^[a-zA-Z0-9 ]*$").Length != command.Length)
				throw new Exception("Invalid command");

			string output = Application.execute(new Command(command));

			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.WriteLine(output);
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
		}


		public class Command {
			List<string> list = new List<string>();
			public string applicationName { get { return list.Count < 1 ? "" : list[0]; } }
			public string methodName { get { return list.Count < 2 ? "" : list[1]; } }

			public object[] getParameters() {
				List<object> param = new List<object>();



				for (int i = 2; i < list.Count; i++) {
					param.Add(list[i]);
				}


				return param.ToArray();
			}

			public Command(string command) {
				foreach (var group in command.Split(' ')) {
					list.Add(group);
				}
			}
		}
	}
}