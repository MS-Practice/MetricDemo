using App.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PrometheusNetCore.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PrometheusNetCore.Middlewares
{
    public class HttpMetricsMiddleware
    {
        private readonly ILogger<HttpMetricsMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly IMetrics _metrics;

        public HttpMetricsMiddleware(RequestDelegate next,
            IMetrics metrics,
            ILogger<HttpMetricsMiddleware> logger)
        {
            _logger = logger;
            _metrics = metrics;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                // 最好设定好要监控的地址，不建议监控所有的地址
                _metrics.Measure.Counter.Increment(MetricsRegistry.HttpRequestCounter, e.Message);
            }
        }
    }
}
