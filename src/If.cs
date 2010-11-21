using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedFusion
{
	/// <summary>
	/// 
	/// </summary>
	public class If<R>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="If"/> class.
		/// </summary>
		/// <param name="o">The o.</param>
		public If(bool condition, Func<R> function)
		{
			if (condition)
				Set(function());
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="If&lt;R&gt;"/> class.
		/// </summary>
		/// <param name="condition">if set to <see langword="true"/> [condition].</param>
		/// <param name="response">The response.</param>
		public If(bool condition, R response)
		{
			if (condition)
				Set(response);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance has value.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if this instance has value; otherwise, <see langword="false"/>.
		/// </value>
		public bool HasValue { get; private set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public R Value { get; private set; }

		/// <summary>
		/// Sets the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		public void Set(R value)
		{
			Value = value;
			HasValue = true;
		}

		/// <summary>
		/// Else ifs the specified function..
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <param name="function">The function.</param>
		/// <returns></returns>
		public If<R> ElseIf(bool condition, Func<R> function)
		{
			if (!this.HasValue && condition)
				this.Set(function());

			return this;
		}

		/// <summary>
		/// Elses if.
		/// </summary>
		/// <param name="condition">if set to <see langword="true"/> [condition].</param>
		/// <param name="response">The response.</param>
		/// <returns></returns>
		public If<R> ElseIf(bool condition, R response)
		{
			if (!this.HasValue && condition)
				this.Set(response);

			return this;
		}

		/// <summary>
		/// Elses the specified function.
		/// </summary>
		/// <param name="function">The function.</param>
		/// <returns></returns>
		public R Else(Func<R> function)
		{
			if (!this.HasValue)
				this.Set(function());

			return this.Value;
		}

		/// <summary>
		/// Elses the specified response.
		/// </summary>
		/// <param name="response">The response.</param>
		/// <returns></returns>
		public R Else(R response)
		{
			if (!this.HasValue)
				this.Set(response);

			return this.Value;
		}
	}
}