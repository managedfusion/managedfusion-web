using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Caching;
using System.Web.Compilation;

using  ManagedFusion.Web.Mvc.Controls;

namespace System.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public static class HtmlHelperExtension
	{
		/// <summary>
		/// Ifs the specified response.
		/// </summary>
		/// <param name="response">The response.</param>
		/// <param name="condition">if set to <see langword="true"/> [condition].</param>
		/// <param name="function">The function.</param>
		/// <returns></returns>
		public static ManagedFusion.If<string> If(this HtmlHelper helper, bool condition, Func<string> function)
		{
			return new ManagedFusion.If<string>(condition, function);
		}

		/// <summary>
		/// Ifs the specified helper.
		/// </summary>
		/// <param name="helper">The helper.</param>
		/// <param name="condition">if set to <see langword="true"/> [condition].</param>
		/// <param name="response">The response.</param>
		/// <returns></returns>
		public static ManagedFusion.If<string> If(this HtmlHelper helper, bool condition, string response)
		{
			return new ManagedFusion.If<string>(condition, response);
		}

		/// <summary>
		/// Switches the specified response.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="response">The response.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static ManagedFusion.Switch<T, string> Switch<T>(this HtmlHelper helper, T type)
		{
			return new ManagedFusion.Switch<T, string>(type);
		}

		/// <summary>
		/// Captchas the text box.
		/// </summary>
		/// <param name="helper">The helper.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static string CaptchaTextBox(this HtmlHelper helper, string name)
		{
			return String.Format(@"<input type=""text"" id=""{0}"" name=""{0}"" value="""" maxlength=""{1}"" autocomplete=""off"" />",
					name,
					ManagedFusion.Web.Mvc.Controls.CaptchaImage.TextLength
					);

		}

		/// <summary>
		/// Generates the captcha image.
		/// </summary>
		/// <param name="helper">The helper.</param>
		/// <param name="height">The height.</param>
		/// <param name="width">The width.</param>
		/// <returns>
		/// Returns the <see cref="Uri"/> for the generated <see cref="CaptchaImage"/>.
		/// </returns>
		public static string CaptchaImage(this HtmlHelper helper, int height, int width)
		{
			CaptchaImage image = new CaptchaImage {
				Height = height,
				Width = width,
			};

			HttpRuntime.Cache.Add(
				image.UniqueId,
				image,
				null,
				DateTime.Now.AddSeconds(ManagedFusion.Web.Mvc.Controls.CaptchaImage.CacheTimeOut),
				Cache.NoSlidingExpiration,
				CacheItemPriority.NotRemovable,
				null);

			StringBuilder stringBuilder = new StringBuilder(256);
			stringBuilder.Append("<input type=\"hidden\" name=\"captcha-guid\" value=\"");
			stringBuilder.Append(image.UniqueId);
			stringBuilder.Append("\" />");
			stringBuilder.AppendLine();
			stringBuilder.Append("<img src=\"");
			stringBuilder.Append("captcha.ashx?guid=" + image.UniqueId);
			stringBuilder.Append("\" alt=\"CAPTCHA\" width=\"");
			stringBuilder.Append(width);
			stringBuilder.Append("\" height=\"");
			stringBuilder.Append(height);
			stringBuilder.Append("\" />");

			return stringBuilder.ToString();
		}
	}
}
