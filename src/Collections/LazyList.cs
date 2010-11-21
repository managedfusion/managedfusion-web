using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace System.Linq
{
	/// <summary>
	/// An IList implementation that flexes IQueryable's delayed loading
	/// </summary>
	/// <typeparam name="T">IList of T</typeparam>
	public class LazyList<T> : IList<T>
	{
		public LazyList()
		{
			inner = new List<T>();
		}

		public LazyList(IQueryable<T> query)
		{
			this.query = query;
		}

		private IQueryable<T> query;
		private IList<T> inner;

		public int IndexOf(T item)
		{
			return Inner.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			Inner.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			Inner.RemoveAt(index);
		}

		public T this[int index]
		{
			get { return Inner[index]; }
			set { Inner[index] = value; }
		}

		public void Add(T item)
		{
			inner = inner ?? new List<T>();
			Inner.Add(item);
		}

		public void Add(object ob)
		{
			throw new NotImplementedException("This is for serialization");
		}

		public void Clear()
		{
			if (inner != null)
				Inner.Clear();
		}

		public bool Contains(T item)
		{
			return Inner.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			Inner.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			return Inner.Remove(item);
		}

		public int Count
		{
			get { return Inner.Count; }
		}

		public bool IsReadOnly
		{
			get { return Inner.IsReadOnly; }
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return Inner.GetEnumerator();
		}

		public IEnumerator GetEnumerator()
		{
			return ((IEnumerable)Inner).GetEnumerator();
		}

		public IList<T> Inner
		{
			get
			{
				if (inner == null)
					inner = query.ToList();
				return inner;
			}
		}
	}
}