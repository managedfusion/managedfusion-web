using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedFusion.Serialization;

namespace ManagedFusion.Web.Mvc
{
	public class CsvResult : SerializedResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="JsonResult"/> class.
		/// </summary>
		public CsvResult()
		{
			ContentType = "application/csv";
		}

		/// <summary>
		/// 
		/// </summary>
		protected internal override string ContentFileExtension
		{
			get { return "csv"; }
		}

		/// <summary>
		/// Gets the content.
		/// </summary>
		/// <returns></returns>
		protected internal override string GetContent()
		{
			return Data.Serialize(new CsvSerializer(), SerializePublicMembers);
		}
	}
}
