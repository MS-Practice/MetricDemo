using App.Metrics;
using App.Metrics.Timer;
using PrometheusCore.Internals;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrometheusCore
{
    public static class IMetricsExtensions
    {
        public static void RecordEndpointsRequestTime(this IMetrics metrics, string appContextName, string routeTemplate, long elapsed)
        {
            var options = string.IsNullOrWhiteSpace(appContextName) ? null : new HttpMetricsRegistryFactory().CreateTimerOptions(appContextName);
            metrics.EndpointRequestTimer(routeTemplate, options).
                    Record(
                        elapsed,
                        TimeUnit.Milliseconds, null);
        }

        public static ITimer EndpointRequestTimer(this IMetrics metrics, string routeTemplate, TimerOptions timerOptions)
        {
            var tags = new MetricTags("route", routeTemplate);
            if (timerOptions != null)
            {
                return metrics.Provider.Timer.Instance(timerOptions, tags);
            }
            return metrics.Provider.Timer.Instance(HttpMetricsRegistry.Timers.HttpApiTimeElapsed, tags);
        }
    }
}
