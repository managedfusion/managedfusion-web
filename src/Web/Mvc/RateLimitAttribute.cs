using System;
using System.Collections.Generic;
using System.Web.Caching;
using System.Web.Mvc;
using ManagedFusion.Web.Mvc;

namespace ManagedFusion.Web.Mvc
{
	public class RateLimitAttribute : ActionFilterAttribute
	{
		private TimeSpan _timeout;

		public RateLimitAttribute()
		{
			Limit = 100;
			_timeout = new TimeSpan(0, 1, 0);
		}

		public int Limit { get; set; }

		public int Timeout
		{
			get { return Convert.ToInt32(_timeout.TotalSeconds); }
			set { _timeout = new TimeSpan(0, 0, value); }
		}

		protected virtual void HandleRateLimitedRequest(ActionExecutingContext filterContext)
		{
			filterContext.Result = new RateLimitedResult();
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var httpContext = filterContext.HttpContext;
			var response = httpContext.Response;
			var cache = httpContext.Cache;
			var user = httpContext.User;

			if (!user.IsInRole("Admin"))
			{
				var key = user.Identity.ToString();
				var throttle = cache[key] as ThrottleContext;

				if (throttle == null)
				{
					throttle = new ThrottleContext(Limit, _timeout);
					cache.Insert(key, throttle, null, Cache.NoAbsoluteExpiration, _timeout);
				}

				var success = throttle.Increment();

				response.AppendHeader("X-RateLimit-Limit", Limit.ToString());
				response.AppendHeader("X-RateLimit-Remaining", (Limit - throttle.Count).ToString());
				response.AppendHeader("X-RateLimit-Timeout", _timeout.ToString());

				if (!success)
					HandleRateLimitedRequest(filterContext);
			}

			base.OnActionExecuting(filterContext);
		}

		private class ThrottleContext
		{
			private int _requestCount;
			private TimeSpan _timeout;

			private object _lock;
			private Queue<DateTime> _requests;

			public ThrottleContext(int requestCount, TimeSpan timeout)
			{
				_requestCount = requestCount;
				_timeout = timeout;

				_lock = new object();
				_requests = new Queue<DateTime>(_requestCount);
			}

			public int Count
			{
				get { return _requests.Count; }
			}

			public bool Increment()
			{
				lock (_lock)
				{
					var now = DateTime.Now;

					// dequeue all the requests that have exceeded the timeout
					while (_requests.Count > 0 && (now - _requests.Peek()) > _timeout)
						_requests.Dequeue();

					if (_requests.Count < _requestCount)
					{
						_requests.Enqueue(now);
						return true;
					}

					return false;
				}
			}
		}
	}
}
