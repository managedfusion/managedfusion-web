using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public class HttpPostOnlyAttribute : AllowedHttpMethodsAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HttpPostOnlyAttribute"/> class.
		/// </summary>
		public HttpPostOnlyAttribute()
			: base("POST") { }
	}
}
