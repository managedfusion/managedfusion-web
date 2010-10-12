using System;

namespace ManagedFusion.Web.Mvc
{
	public interface ISerializableExceptionResult : ISerializableActionResult
	{
		string ExceptionMessage { get; set; }

		Exception Exception { get; set; }
	}
}
