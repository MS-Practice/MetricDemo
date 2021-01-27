using System;
using System.Collections.Generic;
using System.Text;

namespace PrometheusCore.Options
{
    public class HttpMetricsOption
    {
        public bool Enabled { get; set; }
        public string[] Urls { get; set; }
    }
}
