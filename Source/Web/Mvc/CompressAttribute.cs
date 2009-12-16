using System;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso href="http://weblogs.asp.net/rashid/archive/2008/03/28/asp-net-mvc-action-filter-caching-and-compression.aspx">Original Source by Kazi Manzur Rashid</seealso>
	public class CompressAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CompressAttribute"/> class.
		/// </summary>
		public CompressAttribute()
		{
		}

		/// <summary>
		/// Called when [action executing].
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			HttpRequestBase request = filterContext.HttpContext.Request;

			string acceptEncoding = request.Headers["Accept-Encoding"];

			if (string.IsNullOrEmpty(acceptEncoding)) 
				return;

			acceptEncoding = acceptEncoding.ToUpperInvariant();

			HttpResponseBase response = filterContext.HttpContext.Response;

			if (acceptEncoding.Contains("GZIP"))
			{
				response.AppendHeader("Content-encoding", "gzip");
				response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
			}
			else if (acceptEncoding.Contains("DEFLATE"))
			{
				response.AppendHeader("Content-encoding", "deflate");
				response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
			}
		}
	}
}
