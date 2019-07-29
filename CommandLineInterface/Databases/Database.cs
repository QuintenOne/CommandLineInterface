using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommandLineInterface.Model;

namespace CommandLineInterface
{
	public static class Database {

		public const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QDatabase;Integrated Security=True";

		public static void insert(IModel model) {
			List<string> propertyNames = getPropertyNames(model.GetType());
			joinPropertyNames(propertyNames, true);

			using (SqlConnection conn = new SqlConnection(connectionString)) {

				conn.Open();

				string sql = $"INSERT {model.TableName}(Id, {joinPropertyNames(propertyNames, false)}) VALUES (@Id, {joinPropertyNames(propertyNames, true)})";
				using (SqlCommand command = new SqlCommand(sql, conn)) {
					foreach (var item in propertyNames) {
						string properyName = item;
						string propertyValue = model.GetType().GetProperty(properyName).GetValue(model, null).ToString().Trim();
						
						command.Parameters.AddWithValue(properyName, propertyValue);
					}
					command.Parameters.AddWithValue("@Id", Guid.NewGuid());
					command.ExecuteNonQuery();
				}

				conn.Close();
			}
		}

		public static List<object> select(IModel model) {
			List<object> models = new List<object>();

			using (SqlConnection conn = new SqlConnection(connectionString)) {

				conn.Open();

				string sql = $"SELECT * FROM {model.TableName}";
				using (SqlCommand command = new SqlCommand(sql, conn)) {
					using (SqlDataReader reader = command.ExecuteReader()) {
						
						while (reader.Read()) {
							
							PropertyInfo[] props = model.GetType().GetProperties();

							object[] list = new object[props.Length - 1];

							for (int i = 0; i < props.Length - 1; i++) {

								Type type = typeof(SqlDataReader);
								MethodInfo method = type.GetMethod("GetFieldValue");
								MethodInfo genericMethod = method.MakeGenericMethod( reader.GetFieldType(i) );

								list[i] = genericMethod.Invoke(reader, new object[] { i });
							}

							models.Add(model.GetType().GetMethod("FromDatabase").Invoke(reader, list));

						}
					}
				}

				conn.Close();
			}

			return models;
		}

		static List<string> getPropertyNames(Type type)
		{
			List<string> propertyNames = new List<string>();
			foreach (PropertyInfo property in type.GetProperties()) {
				propertyNames.Add(property.Name);
			}
			propertyNames.Remove("TableName");
			propertyNames.Remove("Id");
			return propertyNames;
		}

		static List<Type> getPropertyTypes(Type type)
		{
			List<Type> propertyNames = new List<Type>();
			foreach (PropertyInfo property in type.GetProperties()) {
				propertyNames.Add(property.PropertyType);
			}
			return propertyNames;
		}

		static string joinPropertyNames(List<string> propertyNames, bool includeAt) {
			string separator = ", ";
			if (includeAt)
				separator += "@";

			string joinedNames = string.Join(separator, propertyNames);

			if (includeAt)
				joinedNames = "@" + joinedNames;

			return joinedNames;
		}

		static List<string> prependPropertiesWithAt(List<string> propertyNames) {
			List<string> AttedpropertyNames = new List<string>();
			foreach (var item in propertyNames)
				AttedpropertyNames.Add("@" + item);
			return AttedpropertyNames;
		}
	}
}
