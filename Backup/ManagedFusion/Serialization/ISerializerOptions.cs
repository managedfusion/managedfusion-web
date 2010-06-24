using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedFusion.Serialization
{
	public interface ISerializerOptions
	{
		/// <summary>
		/// Gets a value indicating whether to check for object name.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if [check for object name]; otherwise, <see langword="false"/>.
		/// </value>
		bool CheckForObjectName { get; }

		/// <summary>
		/// Gets the max levels allowed to be serialized
		/// </summary>
		int? MaxSerializableLevelsSupported { get; }
	}
}
