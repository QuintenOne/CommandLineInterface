using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using CommandLineInterface.Model;

namespace CommandLineInterface {
	public static class Database {

		public const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QDatabase;Integrated Security=True";

		public static bool insert(IModel model) {
			int affectedRows = 0;

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
					affectedRows = command.ExecuteNonQuery();
				}

				conn.Close();
			}

			return affectedRows != 0;
		}


		public static List<IModel> select<T>() where T : IModel, new() {
			return select(new T());
		}

		public static List<IModel> select(IModel model) {
			List<IModel> models = new List<IModel>();

			using (SqlConnection conn = new SqlConnection(connectionString)) {

				conn.Open();

				string sql = generateSelectQuery(model);
				using (SqlCommand command = new SqlCommand(sql, conn)) {
					using (SqlDataReader reader = command.ExecuteReader()) {

						while (reader.Read()) {
							object[] list = new object[model.GetType().GetProperties().Length - 1];

							for (int i = 0; i < list.Length; i++) {
								MethodInfo method = typeof(SqlDataReader).GetMethod("GetFieldValue");
								MethodInfo genericMethod = method.MakeGenericMethod(reader.GetFieldType(i));

								list[i] = genericMethod.Invoke(reader, new object[] { i });
							}

							models.Add((IModel)model.GetType().GetMethod("FromDatabase").Invoke(reader, list));

						}
					}
				}

				conn.Close();
			}

			return models;
		}

		static string generateSelectQuery(IModel model) {
			var properties = model.GetType().GetProperties();
			for (int propertyId = 1; propertyId < properties.Length; propertyId++) {
				var property = properties[propertyId];
				var propertyName = property.Name;
				var propertyValue = model.GetType().GetProperty(property.Name).GetValue(model, null);
			}

			return $"SELECT * FROM {model.TableName}";
		}

		static List<string> getPropertyNames(Type type) {
			List<string> propertyNames = new List<string>();
			foreach (PropertyInfo property in type.GetProperties()) {
				propertyNames.Add(property.Name);
			}
			propertyNames.Remove("TableName");
			propertyNames.Remove("Id");
			return propertyNames;
		}

		static List<Type> getPropertyTypes(Type type) {
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