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
		}

		protected internal override string DispositionType
		{
			get { return "inline"; }
		}

		protected internal override string ContentFileExtension
		{
			get { return "json"; }
		}

		public override string ContentType
		{
			get { return "application/json"; }
			set { ; }
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
