using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrometheusCore
{
    public static class PrometheusApplicationBuilderExtensions
    {
        public static void UseAppMetrics(this IApplicationBuilder builder,Action<AppMetricsConfigurationBuilder> configBuilder)
        {
            var metricsConfiguration = new AppMetricsConfigurationBuilder(builder);
            configBuilder(metricsConfiguration);
        }
    }
}
