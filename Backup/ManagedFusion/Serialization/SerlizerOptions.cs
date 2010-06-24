using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedFusion.Serialization
{
	public class SerlizerOptions : ISerializerOptions
	{
		#region ISerializerOptions Members

		public bool CheckForObjectName
		{
			get;
			set;
		}

		public int? MaxSerializableLevelsSupported
		{
			get;
			set;
		}

		#endregion
	}
}
