using CommandLineInterface.Applications;
using CommandLineInterface.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CommandLineInterface
{
	class Program
    {
        static void Main(string[] args)
        {
			var select = Database.select(new ProductModel() {
			});

			Console.Write(JsonConvert.SerializeObject(select, Formatting.Indented));

			Application.discover();

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            while (true)
            {
                try
                {

                    CLI.wait();

                }
                catch (Exception exception)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {exception.Message}");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
        }
    }

    public class CLI
    {
        public static List<Command> commands = new List<Command>();

        public static void wait()
        {
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


        public class Command
        {
            List<string> list = new List<string>();
            public string applicationName { get { return list.Count < 1 ? "" : list[0]; } }
            public string methodName { get { return list.Count < 2 ? "" : list[1]; } }

            public object[] getParameters() {
                List<object> param = new List<object>();

                

                for (int i = 2; i < list.Count; i++)
                {
                    param.Add(list[i]);
                }


                return param.ToArray();
            }

            public Command(string command)
            {
                foreach (var group in command.Split(' '))
                {
                    list.Add(group);
                }
            }
        }
    }
}
