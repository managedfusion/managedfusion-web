using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public enum ResponseType
	{
		/// <summary>
		/// 
		/// </summary>
		None,
		/// <summary>
		/// 
		/// </summary>
		Html,
		/// <summary>
		/// 
		/// </summary>
		Xml,
		/// <summary>
		/// 
		/// </summary>
		Json,
		/// <summary>
		/// 
		/// </summary>
		JavaScript,
		/// <summary>
		/// 
		/// </summary>
		Csv
	}
}
