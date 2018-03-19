using System;
using System.Collections.Generic;
using System.Reflection;
using Firebase.DB.Helpers.Models;
using Foundation;

namespace Firebase.DB.Helpers
{
    public static class ExtensionModelsObject
    {
		/// <summary>
		/// Genero un NSDictionary de un modelo que derive de la clase FirebaseBaseObjectModel para guardarlo en firebase de un jalon
		/// </summary>
		/// <returns>Las propiedades del objeto y sus valores como un NSDictionary</returns>
		/// <param name="modelo"> instancia del tipo FirebaseBaseObjectModel</param>
		public static NSDictionary ToNSDictionary(this FirebaseBaseObjectModel modelo)
		{
			var keys = new List<NSObject>();
			var values = new List<NSObject>();
			var type = modelo.GetType();
			var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (var pi in properties)
			{
				object[] esOpcional = pi.GetCustomAttributes(typeof(OptionalAttribute), true);

				if (esOpcional.Length <= 0)//NO es opcional
				{
					var selfValue = type.GetProperty(pi.Name).GetValue(modelo, null);
                   
				    keys.Add(NSObject.FromObject(pi.Name));

					if (selfValue != null)
					{
                        if(selfValue is DateTime)
                            values.Add(NSObject.FromObject(((DateTime)selfValue).Ticks));
                        else    
						    values.Add(NSObject.FromObject(selfValue));
					}
					else
					{
						values.Add(NSObject.FromObject(null));
					}
				}
			}

			return NSDictionary.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count); ;
		}

		public static T ToFirebaseBaseModel<T>(this NSDictionary data) where T : new()
		{
			T tha_model = new T();
			var type = tha_model.GetType();
			var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (var pi in properties)
			{
				PropertyInfo propertyInfo = type.GetProperty(pi.Name);
                object[] esID = pi.GetCustomAttributes(typeof(IdAttribute), true);
                NSObject value;
                if (esID.Length > 0 && esID[0] is IdAttribute) // es el ID
                    value = data.ValueForKey((NSString)"key");
                else
                    value = data.ValueForKey((NSString)pi.Name);
				//
				try
				{
                    object convertedValue;
                    if (propertyInfo.PropertyType != typeof(System.DateTime))
                        convertedValue = Convert.ChangeType(value.ToObject(propertyInfo.PropertyType), propertyInfo.PropertyType);
                    else
                    {
                        long ticks = (long)Convert.ChangeType(value.ToObject(typeof(long)), typeof(long));
                        convertedValue = new DateTime(ticks);
                    }
					propertyInfo.SetValue(tha_model, convertedValue);
				}
				catch (Exception ex)
				{
					Console.WriteLine("*****************************************************************");
                    Console.WriteLine(ex.Message);
					Console.WriteLine("*****************************************************************");
					propertyInfo.SetValue(tha_model, null);
				}
			}
			return tha_model;
		}
    }
}
