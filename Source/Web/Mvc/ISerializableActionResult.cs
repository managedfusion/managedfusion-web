using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedFusion.Web.Mvc
{
	public interface ISerializableActionResult
	{
		object Model { get; }
	}

	public interface ISerializableErrorResult : ISerializableActionResult
	{
		string Error { get; set; }

		int StatusCode { get; set; }

		string StatusDescription { get; set; }
	}
}
