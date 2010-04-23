using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ManagedFusion.Web.Mvc
{
	public class SerializedView : IView
	{
		public SerializedView()
		{
			SerializePublicMembers = true;
			FollowFrameworkIgnoreAttributes = true;

			SerializedHeader = new Dictionary<string, object>();
		}

		/// <summary>
		/// Gets or sets a value indicating whether [serialize public members].
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if [serialize public members]; otherwise, <see langword="false"/>.
		/// </value>
		public bool SerializePublicMembers
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public bool FollowFrameworkIgnoreAttributes
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public IDictionary<string, object> SerializedHeader
		{
			get;
			private set;
		}

		#region IView Members

		public void Render(ViewContext viewContext, System.IO.TextWriter writer)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
