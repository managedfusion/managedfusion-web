using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using ManagedFusion.Serialization;

namespace ManagedFusion.Web.Mvc
{
	public class JsonResult : SerializedResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="JsonResult"/> class.
		/// </summary>
		public JsonResult()
		{
			ContentType = "application/json";
		}

		/// <summary>
		/// 
		/// </summary>
		protected internal override string ContentFileExtension
		{
			get { return "json"; }
		}

		/// <summary>
		/// Gets the content.
		/// </summary>
		/// <returns></returns>
		protected internal override string GetContent()
		{
			return Data.Serialize(new JsonSerializer(Data), SerializePublicMembers, UseFrameworkIgnores);
		}

		internal class JsonSerializer : ManagedFusion.Serialization.JsonSerializer
		{
			private object _serializableObject;

			/// <summary>
			/// Initializes a new instance of the <see cref="JsonSerializer"/> class.
			/// </summary>
			/// <param name="obj">The obj.</param>
			public JsonSerializer(object obj)
			{
				_serializableObject = obj;
			}

			/// <summary>
			/// Gets a value indicating whether to check for object name.
			/// </summary>
			/// <value>
			/// 	<see langword="true"/> if [check for object name]; otherwise, <see langword="false"/>.
			/// </value>
			public override bool CheckForObjectName
			{
				get { return true; }
			}

			/// <summary>
			/// Serializes to json.
			/// </summary>
			/// <param name="serialization">The serialization.</param>
			/// <returns></returns>
			public override string SerializeToString(Dictionary<string, object> serialization)
			{
				return base.SerializeToString(SerializedResult.BuildResponse(_serializableObject, serialization));
			}
		}
	}
}
