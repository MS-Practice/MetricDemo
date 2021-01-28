using App.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PrometheusCore.Internals;
using PrometheusCore.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PrometheusCore.Middlewares
{
    public class HttpMetricsMiddleware
    {
        private readonly ILogger<HttpMetricsMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly IMetrics _metrics;
        private readonly HttpMetricsOption httpMetricsOption;
        private readonly string appMetricsContextName;

        public HttpMetricsMiddleware(RequestDelegate next,
            IConfiguration configuration,
            IMetrics metrics,
            ILogger<HttpMetricsMiddleware> logger)
        {
            _logger = logger;
            _metrics = metrics;
            _next = next;
            httpMetricsOption = configuration.GetSection("HttpMetricsOption").Get<HttpMetricsOption>();
            appMetricsContextName = configuration["MetricsOption:MetricsContextName"];
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                HttpMetrics(context);
                await _next(context);
            }
            catch (Exception e)
            {
                // 最好设定好要监控的地址，不建议监控所有的地址
                _metrics.Measure.Counter.Increment(MetricsRegistry.HttpRequestCounter, e.Message);
            }
        }

        private void HttpMetrics(HttpContext context)
        {
            if (httpMetricsOption != null && httpMetricsOption.Enabled)
            {
                var metrics = MetricsUrlsContains(context);
                if (metrics.contains)
                {
                    var watch = Stopwatch.StartNew();
                    // 这里去要监控的 url 列表
                    context.Response.OnStarting(() =>
                    {
                        if (metrics.contains)
                        {
                            watch.Stop();
                            _metrics.RecordEndpointsRequestTime(appMetricsContextName, metrics.url, watch.ElapsedMilliseconds);
                        }
                        return Task.CompletedTask;
                    });
                }
            }
        }

        private (bool contains, string url) MetricsUrlsContains(HttpContext context)
        {
            string url = context.Request.Path;
            var contains = httpMetricsOption.Urls.Any(p => Regex.IsMatch(url, p, RegexOptions.IgnoreCase));
            return (contains, contains ? context.Request.Scheme + "://" + context.Connection.RemoteIpAddress.MapToIPv4().ToString() + url : "");
        }
    }
}
