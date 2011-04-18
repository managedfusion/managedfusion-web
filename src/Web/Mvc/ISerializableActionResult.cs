using System;

namespace ManagedFusion.Web.Mvc
{
	public interface ISerializableActionResult
	{
		object Model { get; }
		SerializedView ModelSerializer { get; }
	}
}