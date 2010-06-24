using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedFusion.Serialization
{
	/// <summary>
	/// 
	/// </summary>
	public interface ISerializer : ISerializerOptions
	{
		/// <summary>
		/// Serializes the specified serialization.
		/// </summary>
		/// <param name="serialization">The serialization.</param>
		/// <returns></returns>
		string Serialize(IDictionary<string, object> serialization);
	}
}
