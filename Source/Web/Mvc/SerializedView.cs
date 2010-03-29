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
			SerializedHeader = new Dictionary<string, object>();
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
