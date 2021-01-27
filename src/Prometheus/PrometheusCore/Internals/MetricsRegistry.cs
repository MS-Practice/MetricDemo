using App.Metrics;
using App.Metrics.Counter;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrometheusCore.Internals
{
    internal class MetricsRegistry
    {
        public static CounterOptions HttpRequestCounter => new CounterOptions
        {
            Name = "HttpRequest Counter",
            MeasurementUnit = Unit.Calls,
        };
    }
}
