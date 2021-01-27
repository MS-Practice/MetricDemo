using App.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PrometheusCore.Internals;
using PrometheusCore.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PrometheusCore.Middlewares
{
    public class HttpMetricsMiddleware
    {
        private readonly ILogger<HttpMetricsMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly IMetrics _metrics;
        private readonly HttpMetricsOption httpMetricsOption;

        public HttpMetricsMiddleware(RequestDelegate next,
            IConfiguration configuration,
            IMetrics metrics,
            ILogger<HttpMetricsMiddleware> logger)
        {
            _logger = logger;
            _metrics = metrics;
            _next = next;
            httpMetricsOption = configuration.GetSection("HttpMetricsOption").Get<HttpMetricsOption>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (httpMetricsOption != null && httpMetricsOption.Enabled)
                {
                    // 这里去要监控的 url 列表
                    var watch = new Stopwatch();
                    //var responseTime = "0";
                    watch.Start();
                    //var counter = Metrics.Instance.CreateCounter("prometheus_demo_request_total", "HTTP Requests Total", new CounterConfiguration
                    //{
                    //    LabelNames = new[] { "path", "method", "status" }
                    //});

                    ////只统计成功的时间
                    //var gauge = Metrics.CreateGauge("http_response_time", "Http Response Time ", new GaugeConfiguration
                    //{
                    //    LabelNames = new[] { "path", "method" }
                    //});
                    context.Response.OnStarting(() =>
                    {
                        // Stop the timer information and calculate the time   
                        watch.Stop();
                        //var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
                        // Add the Response time information in the Response headers.   
                        //responseTime = responseTimeForCompleteRequest.ToString();
                        //if (path != "/metrics")
                        //{
                        //    statusCode = httpContext.Response.StatusCode;
                        //    counter.Labels(path, method, statusCode.ToString()).Inc();
                        //    gauge.Labels(path, method).Inc();
                        //    gauge.Set(watch.ElapsedMilliseconds);
                        //}
                        return Task.CompletedTask;
                    });
                }
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
