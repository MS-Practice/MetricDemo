using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Timer;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrometheusCore.Internals
{
    internal class HttpMetricsRegistry
    {
        public static string ContextName = "Application.HttpRequests";

        public static class Counters
        {

        }

        public static class Timers
        {
            public static readonly TimerOptions HttpApiTimeElapsed = new TimerOptions
            {
                Context = ContextName,
                Name = "ElapsedTime",
                MeasurementUnit = Unit.Custom("Request Elapsed"),
                DurationUnit = TimeUnit.Milliseconds
            };
        }
    }

    internal class HttpMetricsRegistryFactory
    {
        public HttpMetricsRegistryFactory()
        {

        }

        public TimerOptions CreateTimerOptions(string appMetricsContextName) =>
            new TimerOptions
            {
                Context = string.IsNullOrEmpty(appMetricsContextName) ? HttpMetricsRegistry.ContextName : appMetricsContextName,
                Name = "ElapsedTime",
                MeasurementUnit = Unit.Custom("Request Elapsed"),
                DurationUnit = TimeUnit.Milliseconds
            };
    }
}
