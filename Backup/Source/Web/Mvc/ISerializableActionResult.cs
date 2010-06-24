using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ManagedFusion.Web.Mvc
{
	public interface ISerializableActionResult
	{
		object Model { get; }

		int StatusCode { get; set; }

		string StatusDescription { get; set; }
	}

	public interface ISerializableErrorResult : ISerializableActionResult
	{
		string Error { get; set; }
	}
}
