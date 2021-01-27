using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrometheusCore
{
    public class HealthChecksBuilder
    {
        private readonly IServiceCollection _services;
        private readonly IHealthChecksBuilder _healthChecksBuilder;
        public HealthChecksBuilder(IServiceCollection services, IHealthChecksBuilder healthChecksBuilder)
        {
            _services = services;
            _healthChecksBuilder = healthChecksBuilder;
        }

        public HealthChecksBuilder AddHealthCheck<THealthCheck>(string name)
            where THealthCheck : class, IHealthCheck
        {
            _healthChecksBuilder.AddCheck<THealthCheck>(name);
            return this;
        }

        public HealthChecksBuilder AddHealthCheck(string name, Func<HealthCheckResult> check, IEnumerable<string> tags)
        {
            _healthChecksBuilder.AddCheck(name, check, tags);
            return this;
        }
    }
}
