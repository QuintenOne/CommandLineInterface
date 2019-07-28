using CommandLineInterface.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommandLineInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.discover();
            Console.WriteLine(Application.writeMethods());

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

            validate(command);

            run(new Command(command));
        }

        static void validate(string command)
        {
            if (Regex.Match(command, "^[a-zA-Z0-9 ]*$").Length != command.Length)
                throw new Exception("Invalid command");
        }

        static void run(Command command){
            Console.WriteLine(Application.execute(command));
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
