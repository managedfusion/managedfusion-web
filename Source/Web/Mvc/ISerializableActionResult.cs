using System;

namespace ManagedFusion.Web.Mvc
{
	public interface ISerializableActionResult
	{
		object Model { get; }

		int StatusCode { get; set; }

		string StatusDescription { get; set; }
	}
}