using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedFusion.Serialization;

namespace ManagedFusion.Web.Mvc
{
	public class CsvResult : SerializedView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="JsonResult"/> class.
		/// </summary>
		public CsvResult()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected internal override string ContentFileExtension
		{
			get { return "csv"; }
		}

		public override string ContentType
		{
			get { return "application/csv"; }
			set { ; }
		}

		/// <summary>
		/// Gets the content.
		/// </summary>
		/// <returns></returns>
		protected internal override string GetContent()
		{
			CsvSerializer csvSerializer = new CsvSerializer();

			Serializer serializer = new Serializer() {
				SerializePublicMembers = SerializePublicMembers,
				FollowFrameworkIgnoreAttributes = FollowFrameworkIgnoreAttributes
			};

			// don't want to build a response because we want this as flat as possible
			var response = serializer.FromObject(Model, csvSerializer);

			return csvSerializer.Serialize(response);
		}
	}
}
