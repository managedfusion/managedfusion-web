using System;
using System.Collections.Generic;
using NUnit.Framework;
using ManagedFusion.Web.Mvc;
using System.Web.Mvc;
using System.IO;
using System.Web.Routing;
using ManagedFusion.Serialization;

namespace ManagedFusion.Web.Tests
{
	[TestFixture]
	public class SerializedViewTest
	{
		[Test]
		public void Simple_Json()
		{
			// arrange
			var expected = "\"object\":{\"name\":\"value\"}";
			var obj = new Dictionary<string, object>() {
				{ "name", "value" }
			};

			var view = new JsonView();

			var viewContext = new ViewContext();
			viewContext.ViewData = new ViewDataDictionary(obj);
			viewContext.RouteData = new RouteData();
			viewContext.RouteData.Values.Add("action", "test");
			viewContext.HttpContext = HttpHelpers.MockHttpContext();

			// act
			var result = new StringWriter();
			view.Render(viewContext, result);

			// assert
			Assert.IsTrue(result.ToString().Contains(expected), "expected: {0}, but was: {1}", expected, result);
		}

		[Test]
		public void Simple_ModelName()
		{
			// arrange
			var expected = "\"test\":{\"name\":\"value\"}";
			var obj = new Dictionary<string, object>() {
				{ Serializer.ModelNameKey, "test" },
				{ "name", "value" }
			};

			var view = new JsonView();

			var viewContext = new ViewContext();
			viewContext.ViewData = new ViewDataDictionary(obj);
			viewContext.RouteData = new RouteData();
			viewContext.RouteData.Values.Add("action", "test");
			viewContext.HttpContext = HttpHelpers.MockHttpContext();

			// act
			var result = new StringWriter();
			view.Render(viewContext, result);

			// assert
			Assert.IsTrue(result.ToString().Contains(expected), "expected: {0}, but was: {1}", expected, result);
		}
	}
}
