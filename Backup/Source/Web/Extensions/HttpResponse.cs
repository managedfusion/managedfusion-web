using System;
using System.Net;
using System.Web;

namespace System.Web
{
	/// <summary>
	/// 
	/// </summary>
	public static class HttpResponseExtensions
	{
		/// <summary>
		/// Redirects the specified response.
		/// </summary>
		/// <param name="response">The response.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="url">The URL.</param>
		public static void Redirect(this HttpResponseBase response, HttpStatusCode responseCode, string url)
		{
			Redirect(HttpContext.Current.Response, (int)responseCode, url);
		}

		/// <summary>
		/// Redirects the specified response.
		/// </summary>
		/// <param name="response">The response.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="url">The URL.</param>
		public static void Redirect(this HttpResponseBase response, int responseCode, string url)
		{
			Redirect(HttpContext.Current.Response, responseCode, url);
		}

		/// <summary>
		/// Redirects the specified response.
		/// </summary>
		/// <param name="response">The response.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="url">The URL.</param>
		public static void Redirect(this HttpResponse response, HttpStatusCode responseCode, string url)
		{
			Redirect(response, (int)responseCode, url);
		}

		/// <summary>
		/// Redirects the specified response.
		/// </summary>
		/// <param name="response">The response.</param>
		/// <param name="responseCode">The response code.</param>
		/// <param name="url">The URL.</param>
		public static void Redirect(this HttpResponse response, int responseCode, string url)
		{
			response.Clear();

			switch (responseCode)
			{
				case 301:
					response.StatusCode = (int)HttpStatusCode.MovedPermanently;
					response.StatusDescription = "Moved Permanently";
					break;

				case 302:
					response.StatusCode = (int)HttpStatusCode.Found;
					response.StatusDescription = "Found";
					break;

				case 303:
					response.StatusCode = (int)HttpStatusCode.SeeOther;
					response.StatusDescription = "See Other";
					break;

				case 304:
					response.StatusCode = (int)HttpStatusCode.NotModified;
					response.StatusDescription = "Not Modified";
					break;

				case 307:
					response.StatusCode = (int)HttpStatusCode.TemporaryRedirect;
					response.StatusDescription = "Temporary Redirect";
					break;

				default:
					goto case 302;
			}

			response.RedirectLocation = url;
			response.ContentType = "text/html";

			response.Write("<html><head><title>");
			response.Write(response.StatusDescription);
			response.Write("</title></head><body><p>The URI that you requested has been <a href=\"");
			response.Write(HttpUtility.HtmlAttributeEncode(url));
			response.Write("\">moved to here</a>.</p></body></html>");

			response.End();
		}
	}
}
