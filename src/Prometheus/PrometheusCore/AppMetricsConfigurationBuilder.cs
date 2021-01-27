using Microsoft.AspNetCore.Builder;
using PrometheusCore.Middlewares;

namespace PrometheusCore
{
    public class AppMetricsConfigurationBuilder
    {
        private IApplicationBuilder builder;

        public AppMetricsConfigurationBuilder(IApplicationBuilder builder)
        {
            this.builder = builder;
        }

        public void MetricsAllMiddlewares()
        {
            builder.UseMetricsAllMiddleware();
            builder.UseMiddleware<HttpMetricsMiddleware>();
        }
    }
}