using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedFusion.Serialization
{
	public interface IDeserializer : ISerializerOptions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		IDictionary<string, object> Deserialize(string input);
	}
}
