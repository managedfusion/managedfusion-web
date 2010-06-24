using System;
using System.IO;
using System.Text;
using System.Web;
using System.Collections.Generic;

namespace ManagedFusion.Serialization
{
	/// <summary>
	/// 
	/// </summary>
	public static class SerializationExtensions
	{
		/// <summary>
		/// Serializes the specified obj.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The obj.</param>
		/// <param name="serializer">The serializer.</param>
		/// <returns></returns>
		public static string Serialize<T>(this T obj, ISerializer serializer)
		{
			return Serialize(obj, serializer, false, false);
		}

		/// <summary>
		/// Serializes the specified obj.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="serializer"></param>
		/// <param name="serializePublic"></param>
		/// <param name="useFrameworkIgnores"></param>
		/// <returns></returns>
		public static string Serialize<T>(this T obj, ISerializer serializer, bool serializePublic, bool useFrameworkIgnores)
		{
			Serializer ser = new Serializer() {
				SerializePublicMembers = serializePublic,
				FollowFrameworkIgnoreAttributes = useFrameworkIgnores
			};
			return ser.Serialize(obj, serializer);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static IDictionary<string, object> ToDictionary<T>(this T obj)
		{
			return ToDictionary(obj, false, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="serializer"></param>
		/// <param name="serializePublic"></param>
		/// <param name="useFrameworkIgnores"></param>
		/// <returns></returns>
		public static IDictionary<string, object> ToDictionary<T>(this T obj, bool serializePublic, bool useFrameworkIgnores)
		{
			Serializer ser = new Serializer() {
				SerializePublicMembers = serializePublic,
				FollowFrameworkIgnoreAttributes = useFrameworkIgnores
			};
			return ser.FromObject(obj, new SerlizerOptions {
				CheckForObjectName = false,
				MaxSerializableLevelsSupported = null
			});
		}

		/// <summary>
		/// Toes the json.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The obj.</param>
		/// <returns></returns>
		public static string ToJson<T>(this T obj)
		{
			return Serialize<T>(obj, new JsonSerializer());
		}

		/// <summary>
		/// Toes the json.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The obj.</param>
		/// <param name="serializePublic">if set to <see langword="true"/> [serialize public].</param>
		/// <returns></returns>
		public static string ToJson<T>(this T obj, bool serializePublic)
		{
			return Serialize<T>(obj, new JsonSerializer(), serializePublic, serializePublic);
		}

		/// <summary>
		/// Toes the XML.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The obj.</param>
		/// <returns></returns>
		public static string ToXml<T>(this T obj)
		{
			return Serialize<T>(obj, new XmlSerializer());
		}

		/// <summary>
		/// Toes the XML.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The obj.</param>
		/// <param name="serializePublic">if set to <see langword="true"/> [serialize public].</param>
		/// <returns></returns>
		public static string ToXml<T>(this T obj, bool serializePublic)
		{
			return Serialize<T>(obj, new XmlSerializer(), serializePublic, serializePublic);
		}
	}
}
