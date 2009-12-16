using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedFusion
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="R"></typeparam>
	public class Switch<T, R>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Switch&lt;T, R&gt;"/> class.
		/// </summary>
		/// <param name="o">The o.</param>
		public Switch(T o)
		{
			Object = o;
		}

		/// <summary>
		/// Gets or sets the object.
		/// </summary>
		/// <value>The object.</value>
		public T Object { get; private set; }

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
		/// Cases the specified term.
		/// </summary>
		/// <param name="term">The term.</param>
		/// <param name="function">The function.</param>
		/// <returns></returns>
		public Switch<T, R> Case(T term, Func<T, R> function)
		{
			return Case(x => object.Equals(x, term), function);
		}

		/// <summary>
		/// Cases the specified condition.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <param name="function">The function.</param>
		/// <returns></returns>
		public Switch<T, R> Case(Func<T, bool> condition, Func<T, R> function)
		{
			if (!this.HasValue && condition(this.Object))
				this.Set(function(this.Object));

			return this;
		}

		/// <summary>
		/// Cases the specified term.
		/// </summary>
		/// <param name="term">The term.</param>
		/// <param name="response">The response.</param>
		/// <returns></returns>
		public Switch<T, R> Case(T term, R response)
		{
			return Case(x => object.Equals(x, term), response);
		}

		/// <summary>
		/// Cases the specified condition.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <param name="response">The response.</param>
		/// <returns></returns>
		public Switch<T, R> Case(Func<T, bool> condition, R response)
		{
			if (!this.HasValue && condition(this.Object))
				this.Set(response);

			return this;
		}

		/// <summary>
		/// Defaults the specified function.
		/// </summary>
		/// <param name="function">The function.</param>
		/// <returns></returns>
		public R Default(Func<T, R> function)
		{
			if (!this.HasValue)
				this.Set(function(this.Object));

			return this.Value;
		}

		/// <summary>
		/// Defaults the specified response.
		/// </summary>
		/// <param name="response">The response.</param>
		/// <returns></returns>
		public R Default(R response)
		{
			if (!this.HasValue)
				this.Set(response);

			return this.Value;
		}
	}
}
