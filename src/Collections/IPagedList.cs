using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
	public interface IPagedList
	{
		int TotalCount
		{
			get;
			set;
		}

		int TotalPages
		{
			get;
			set;
		}

		int PageIndex
		{
			get;
			set;
		}

		int PageSize
		{
			get;
			set;
		}

		bool IsPreviousPage
		{
			get;
		}

		bool IsNextPage
		{
			get;
		}
	}
}
