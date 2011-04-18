using System;
using System.Web.Mvc;
using System.Web;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public class SerializedResult : ViewResultBase, ISerializableActionResult
	{
		protected SerializedResult()
			: this(null) { }

		public SerializedResult(object model)
		{
			Model = model;
			ModelSerializer = new AutoSerializedView();
		}

		/// <summary>
		/// Gets or sets the model serializer.
		/// </summary>
		public SerializedView ModelSerializer
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
		public virtual object Model
		{
			get;
			protected set;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected override ViewEngineResult FindView(ControllerContext context)
		{
			return new ViewEngineResult(ModelSerializer, new NullViewEngine());
		}

		/// <summary>
		/// Executes the result.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void ExecuteResult(ControllerContext context)
		{
			View = ModelSerializer;
			ViewData = new ViewDataDictionary(Model);

			base.ExecuteResult(context);
		}
	}
}