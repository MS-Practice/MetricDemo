using System;
using System.Collections.Generic;
using System.Text;

namespace PrometheusCore.Options
{
    public class HttpMetricsOption
    {
        public bool Enabled { get; set; } = false;
        public string[] Urls { get; set; } = Array.Empty<string>();
    }
}
