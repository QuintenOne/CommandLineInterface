using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using CommandLineInterface.Model;
using Newtonsoft.Json;

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

		public static List<IModel> select<TModel>(object param = null) where TModel : IModel, new() {
			List<IModel> models = new List<IModel>();

			using (SqlConnection conn = new SqlConnection(connectionString)) {

				conn.Open();

				string sql = generateSelectQuery<TModel>(param);
				using (SqlCommand command = new SqlCommand(sql, conn)) {
					using (SqlDataReader reader = command.ExecuteReader()) {

						while (reader.Read()) {
							object[] list = new object[typeof(TModel).GetProperties().Length - 1];

							for (int i = 0; i < list.Length; i++) {
								MethodInfo method = typeof(SqlDataReader).GetMethod("GetFieldValue");
								MethodInfo genericMethod = method.MakeGenericMethod(reader.GetFieldType(i));

								list[i] = genericMethod.Invoke(reader, new object[] { i });
							}

							models.Add((IModel)typeof(TModel).GetMethod("FromDatabase").Invoke(reader, list));
						}
					}
				}

				conn.Close();
			}

			return models;
		}

		public static List<TModel> exist<TModel>(object param = null) where TModel : IModel, new() {
			List<TModel> models = new List<TModel>();

			using (SqlConnection conn = new SqlConnection(connectionString)) {

				conn.Open();

				string sql = generateSelectQuery<TModel>(param);
				using (SqlCommand command = new SqlCommand(sql, conn)) {
					using (SqlDataReader reader = command.ExecuteReader()) {

						while (reader.Read()) {
							object[] list = new object[typeof(TModel).GetProperties().Length - 1];

							for (int i = 0; i < list.Length; i++) {
								MethodInfo method = typeof(SqlDataReader).GetMethod("GetFieldValue");
								MethodInfo genericMethod = method.MakeGenericMethod(reader.GetFieldType(i));

								list[i] = genericMethod.Invoke(reader, new object[] { i });
							}

							models.Add((TModel)typeof(TModel).GetMethod("FromDatabase").Invoke(reader, list));
						}
					}
				}

				conn.Close();
			}

			return models;
		}

		static string generateSelectQuery<TModel>(object param) where TModel : IModel, new() {
			string sql = $"SELECT * FROM {new TModel().TableName}";
			Console.WriteLine($"--- START PARAM ---");
			if (param != null) {
				var properties = param.GetType().GetProperties();
				
				for (int propertyId = 0; propertyId < properties.Length; propertyId++) {

					if (propertyId == 0)
						sql += " WHERE ";

					PropertyInfo property = properties[propertyId];
					string propertyName = property.Name;
					object propertyValue = param.GetType().GetProperty(property.Name).GetValue(param, null);

					if (propertyValue.GetType() == typeof(string))
						propertyValue = ((string)propertyValue).Replace("\"", "\\\\\\\"");

					sql += $"{propertyName} = {JsonConvert.SerializeObject(propertyValue).Replace('\"','\'')}";


					if (propertyId != properties.Length - 1)
						sql += " AND ";

				}
			}
			Console.WriteLine($"{sql}");
			Console.WriteLine($"--- END PARAM ---");

			return sql;
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