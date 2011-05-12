using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using ManagedFusion.Serialization;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public class XmlView : SerializedView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XmlResult"/> class.
		/// </summary>
		public XmlView()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected internal override string ContentFileExtension
		{
			get { return "xml"; }
		}

		public override string ContentType
		{
			get { return "text/xml"; }
			set { ; }
		}

		/// <summary>
		/// Gets the content.
		/// </summary>
		/// <returns></returns>
		protected internal override string GetContent()
		{
			XmlSerializer xmlSerializer = new XmlSerializer {
				CheckForObjectName = true,
				MaxSerializableLevelsSupported = null
			};

			Serializer serializer = new Serializer() {
				SerializePublicMembers = SerializePublicMembers,
				FollowFrameworkIgnoreAttributes = FollowFrameworkIgnoreAttributes
			};

			var response = BuildResponse(Model, serializer.FromObject(Model, xmlSerializer));

			Dictionary<string, object> wrapper = new Dictionary<string, object>();
			wrapper.Add("response", response);

			return xmlSerializer.Serialize(wrapper);
		}
	}
}
