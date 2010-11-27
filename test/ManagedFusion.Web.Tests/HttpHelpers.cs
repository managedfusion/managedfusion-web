using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Moq;

namespace ManagedFusion.Web.Tests
{
	public static class HttpHelpers
	{
		public static HttpContextBase MockHttpContext()
		{
			var context = new Mock<HttpContextBase>();
			var request = new Mock<HttpRequestBase>();
			var response = new Mock<HttpResponseBase>();
			var session = new Mock<HttpSessionStateBase>();
			var server = new Mock<HttpServerUtilityBase>();
			var cache = new Mock<HttpCachePolicyBase>();

			response.SetupGet(x => x.Cache).Returns(cache.Object);

			context.SetupGet(x => x.Request).Returns(request.Object);
			context.SetupGet(x => x.Response).Returns(response.Object);
			context.SetupGet(x => x.Session).Returns(session.Object);
			context.SetupGet(x => x.Server).Returns(server.Object);

			return context.Object;
		}
	}
}
