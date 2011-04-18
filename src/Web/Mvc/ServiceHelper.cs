using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ManagedFusion.Web.Mvc
{
	public static class ServiceHelper
	{
		public static void CopyProperties(SerializedView fromResult, SerializedView toResult)
		{
			toResult.FollowFrameworkIgnoreAttributes = fromResult.FollowFrameworkIgnoreAttributes;
			toResult.SerializePublicMembers = fromResult.SerializePublicMembers;
			toResult.SerializedRootName = fromResult.SerializedRootName;
			toResult.StatusCode = fromResult.StatusCode;
			toResult.StatusDescription = fromResult.StatusDescription;
			toResult.ContentType = fromResult.ContentType;
			toResult.ContentEncoding = fromResult.ContentEncoding;

			foreach (var header in fromResult.SerializedHeader)
				toResult.SerializedHeader.Add(header.Key, header.Value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private static string NormalizeType(string type)
		{
			if (String.Equals(type, "javascript", StringComparison.InvariantCultureIgnoreCase))
				return "javascript";
			else if (String.Equals(type, "jsonp", StringComparison.InvariantCultureIgnoreCase))
				return "javascript";

			return type;
		}

		/// <summary>
		/// Called when [action executing].
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public static ResponseType GetResponseType(ControllerContext filterContext, ResponseType defaultResponseType = ResponseType.Html)
		{
			string type = NormalizeType(filterContext.HttpContext.Request.QueryString["type"]);
			ResponseType responseType = ResponseType.None;

			// check to see if we should try to parse it to an enum
			responseType = ManagedFusion.Utility.ParseEnum<ResponseType>(type);

			var acceptTypes = filterContext.HttpContext.Request.AcceptTypes;

			if (responseType == ResponseType.None && filterContext.HttpContext.Request.AcceptTypes != null)
			{
				if (acceptTypes.Any(x => x.StartsWith("text/html") || x.StartsWith("application/xhtml+xml")))
					responseType = ResponseType.Html;

				if (responseType == ResponseType.None)
				{
					foreach (string accept in acceptTypes)
					{
						var value = accept;
						var seperatorIndex = value.IndexOf(';');

						if (seperatorIndex > -1)
							value = value.Substring(0, seperatorIndex);

						switch (accept.ToLower())
						{
							case "application/xhtml+xml":
							case "text/html": responseType = ResponseType.Html; break;

							case "application/json":
							case "application/x-json": responseType = ResponseType.Json; break;

							case "application/javascript":
							case "application/x-javascript":
							case "text/javascript": responseType = ResponseType.JavaScript; break;

							case "application/xml":
							case "text/xml": responseType = ResponseType.Xml; break;

							case "text/csv": responseType = ResponseType.Csv; break;
						}

						if (responseType != ResponseType.None)
							break;
					}
				}
			}

			if (responseType == ResponseType.None)
				responseType = defaultResponseType;

			return responseType;
		}
	}
}
