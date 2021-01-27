using System;
using System.Collections.Generic;
using System.Text;

namespace PrometheusCore.Options
{
    public class DatabaseMetricsOption
    {
        public string DatabaseServer { get; set; }
        public string Database { get; set; }
        public string Uid { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// 刷新频率，秒
        /// </summary>
        public int FlushInterval { get; set; }
        /// <summary>
        /// 秒
        /// </summary>
        public int Timeout { get; set; }
    }
}
