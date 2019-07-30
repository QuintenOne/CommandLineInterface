using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static CommandLineInterface.CLI;

namespace CommandLineInterface.Applications {
	public class Application {

		public static Dictionary<Tuple<string, string, int>, MethodInfo> methods = new Dictionary<Tuple<string, string, int>, MethodInfo>();

		public static void discover() {
			foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()) {
				var applicationAttribute = (ApplicationAttribute)type.GetCustomAttribute(typeof(ApplicationAttribute));
				if (applicationAttribute != null) {
					foreach (var method in type.GetMethods()) {
						var methodAttribute = method.GetCustomAttribute<MethodAttribute>();

						if (methodAttribute != null) {
							methods.Add(
								new Tuple<string, string, int>(
									applicationAttribute.name,
									methodAttribute.name,
									method.GetParameters().Count()
							   ),
							   method
							);
						}
					}
				}
			}
		}

		public static string writeMethods() {

			string strMethods = "";

			foreach (KeyValuePair<Tuple<string, string, int>, MethodInfo> keyValuePair in methods) {
				string applicationName = keyValuePair.Key.Item1;
				string methodName = keyValuePair.Key.Item2;
				int countParameters = keyValuePair.Key.Item3;

				strMethods += $"{applicationName}::{methodName}({countParameters})\n";
			}

			return strMethods;
		}

		public static string execute(Command command) {

			MethodInfo method;

			try {
				method = methods[new Tuple<string, string, int>(command.applicationName, command.methodName, command.getParameters().Length)];
			} catch (Exception) {
				throw new Exception("Command not found");
			}

			try {
				return method.Invoke(null, command.getParameters()).ToString();

			} catch (Exception) {
				throw;
			}
		}
	}
}