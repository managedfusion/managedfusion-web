using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Web;
using System.Collections;

namespace ManagedFusion.Web.Mvc
{
	public abstract class SerializedView : IView
	{
		public SerializedView()
		{
			SerializePublicMembers = true;
			FollowFrameworkIgnoreAttributes = true;

			SerializedHeader = new Dictionary<string, object>();
			SerializedRootName = null;

			StatusCode = 200;
			StatusDescription = "OK";
		}

		/// <summary>
		/// Gets or sets the content encoding.
		/// </summary>
		/// <value>The content encoding.</value>
		public Encoding ContentEncoding { get; set; }

		/// <summary>
		/// Gets or sets the type of the content.
		/// </summary>
		/// <value>The type of the content.</value>
		public string ContentType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [serialize public members].
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if [serialize public members]; otherwise, <see langword="false"/>.
		/// </value>
		public bool SerializePublicMembers { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool FollowFrameworkIgnoreAttributes { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected object Model { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public IDictionary<string, object> SerializedHeader { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public string SerializedRootName { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int StatusCode { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string StatusDescription { get; set; }

		/// <summary>
		/// Builds the response.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns></returns>
		protected IDictionary<string, object> BuildResponse(object serializableObject, IDictionary<string, object> serializedContent)
		{
			// create body of the response
			IDictionary<string, object> response = new Dictionary<string, object>();
			response.Add("timestamp", DateTime.UtcNow);

			// add serialization headers to the response
			foreach (var header in SerializedHeader)
				response.Add(header.Key, header.Value);

			// check for regular collection
			if (serializableObject is ICollection)
				response.Add("count", ((ICollection)serializableObject).Count);

			// check if only one object was returned, if it was then we can rename the root
			if (serializedContent.Count == 1)
			{
				var rootName = SerializedRootName;

				if (String.IsNullOrEmpty(rootName))
					rootName = serializableObject is ICollection && !(serializableObject is IDictionary<string, object>) ? "collection" : "object";

				response.Add(rootName, serializedContent.Single().Value);
			}
			else
				foreach (var item in serializedContent)
					response.Add(item);

			return response;
		}

		/// <summary>
		/// Gets the content.
		/// </summary>
		protected internal virtual string GetContent() { return null; }

		/// <summary>
		/// 
		/// </summary>
		protected internal virtual string ContentFileExtension { get { return null; } }

		#region IView Members

		public virtual void Render(ViewContext viewContext, TextWriter writer)
		{
			Model = viewContext.ViewData.Model;

			string action = viewContext.RouteData.GetRequiredString("action");
			HttpRequestBase request = viewContext.HttpContext.Request;
			HttpResponseBase response = viewContext.HttpContext.Response;
			response.ClearContent();

			response.StatusCode = StatusCode;
			response.StatusDescription = StatusDescription;

			if (!String.IsNullOrEmpty(ContentType))
				response.ContentType = ContentType;

			if (ContentEncoding != null)
				response.ContentEncoding = ContentEncoding;

			response.Cache.SetExpires(DateTime.Today.AddDays(-1D));
			response.AppendHeader("X-Robots-Tag", "noindex, follow, noarchive, nosnippet");
			response.AppendHeader("Content-Disposition", String.Format("inline; filename={0}.{1}; creation-date={2:r}", action, ContentFileExtension, DateTime.UtcNow));

			if (!request.IsSecureConnection)
			{
				response.Cache.SetCacheability(HttpCacheability.NoCache);
				response.AppendHeader("Pragma", "no-cache");
				response.AppendHeader("Cache-Control", "private, no-cache, must-revalidate, no-store, pre-check=0, post-check=0, max-stale=0");
			}

			if (Model != null)
			{
				string content = GetContent();

				if (content != null)
					response.Write(content);
			}
		}

		#endregion
	}
}
