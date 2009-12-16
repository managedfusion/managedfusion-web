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
}
