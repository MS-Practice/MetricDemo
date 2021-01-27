using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace PrometheusCore
{
    public class PrometheusServiceBuilder
    {
        public IConfiguration Configuration { get; set; }
        private readonly IServiceCollection _services;
        public PrometheusServiceBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public void UseHealthCheck(Action<HealthChecksBuilder> setupBuilder)
        {
            _services.AddAppMetricsHealthPublishing();
            var healthCheckBuilder = _services.AddHealthChecks();
            var builder = new HealthChecksBuilder(_services, healthCheckBuilder);
            setupBuilder(builder);
        }
    }
}