using System;

using ManagedFusion.Serialization;

namespace ManagedFusion.Web.Mvc
{
	public class JsonView : SerializedView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="JsonResult"/> class.
		/// </summary>
		public JsonView()
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
