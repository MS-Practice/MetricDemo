using App.Metrics;
using App.Metrics.Counter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrometheusNetCore.Metrics
{
    public static class MetricsRegistry
    {
        public static CounterOptions HttpRequestCounter => new CounterOptions
        {
            Name = "HttpRequest Counter",
            MeasurementUnit = Unit.Calls,
        };
    }
}
