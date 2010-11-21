using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using ManagedFusion.Serialization;

namespace ManagedFusion.Web.Mvc
{
	public class JsonResult : SerializedView
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
			JsonSerializer jsonSerializer = new JsonSerializer {
				CheckForObjectName = true,
				MaxSerializableLevelsSupported = null
			};

			Serializer serializer = new Serializer() {
				SerializePublicMembers = SerializePublicMembers,
				FollowFrameworkIgnoreAttributes = FollowFrameworkIgnoreAttributes
			};

			var response = BuildResponse(Model, serializer.FromObject(Model, jsonSerializer));

			return jsonSerializer.Serialize(response);
		}
	}
}
