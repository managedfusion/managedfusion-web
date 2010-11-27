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
			: this(model, new AutoSerializedView()) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceResult"/> class.
		/// </summary>
		public SerializedResult(object model, SerializedView view)
		{
			Model = model;
			ModelSerializer = view;

			StatusCode = 200;
			StatusDescription = "OK";
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
		public int StatusCode { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string StatusDescription { get; set; }

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
			if (ModelSerializer is AutoSerializedView)
				UpdateModelSerializer(context);

			ModelSerializer.StatusCode = StatusCode;
			ModelSerializer.StatusDescription = StatusDescription;

			View = ModelSerializer;
			ViewData = new ViewDataDictionary(Model);

			base.ExecuteResult(context);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		private void UpdateModelSerializer(ControllerContext context)
		{
			ResponseType responseType = ResponseType.None;
			var routeData = context.RouteData.Values;

			object tempObj;
			if (routeData.TryGetValue("responseType", out tempObj) && tempObj is ResponseType)
				responseType = (ResponseType)tempObj;

			if (responseType == ResponseType.None)
				responseType = ServiceHelper.GetResponseType(context);

			var view = ModelSerializer;

			if (view == null)
				view = new AutoSerializedView();

			switch (responseType)
			{
				case ResponseType.JavaScript:
					ModelSerializer = new JavaScriptCallbackResult();
					break;

				case ResponseType.Json:
					ModelSerializer = new JsonView();
					break;

				case ResponseType.Xml:
					ModelSerializer = new XmlView();
					break;

				case ResponseType.Csv:
					ModelSerializer = new CsvResult();
					break;

				case ResponseType.Html:
				default:
					break;
			}

			if (ModelSerializer is SerializedView)
			{
				var resultX = (ModelSerializer as SerializedView);

				resultX.FollowFrameworkIgnoreAttributes = view.FollowFrameworkIgnoreAttributes;
				resultX.SerializePublicMembers = view.SerializePublicMembers;
				resultX.SerializedRootName = view.SerializedRootName;

				foreach (var header in view.SerializedHeader)
					resultX.SerializedHeader.Add(header.Key, header.Value);
			}
		}
	}
}