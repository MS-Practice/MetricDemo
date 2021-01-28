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
        /// <summary>
        /// 详情见 <see cref="https://www.app-metrics.io/reporting/reporters/microsoft-health-checks/"/>
        /// </summary>
        /// <param name="setupBuilder"></param>
        /// <returns></returns>
        public PrometheusServiceBuilder UseHealthCheck(Action<HealthChecksBuilder> setupBuilder)
        {
            _services.AddAppMetricsHealthPublishing();
            var healthCheckBuilder = _services.AddHealthChecks();
            var builder = new HealthChecksBuilder(_services, healthCheckBuilder);
            setupBuilder(builder);
            return this;
        }
        /// <summary>
        /// 应用程序硬件检测（CPU，垃圾回收，虚拟内存，系统事件等）
        /// 详情见 <see cref="https://www.app-metrics.io/reporting/collectors/"/>
        /// </summary>
        /// <returns></returns>

        public PrometheusServiceBuilder UseSystemMetrics()
        {
            // Metrics 4.2 预览版
            //_services.AddAppMetricsCollectors();
            //services.AddAppMetricsSystemMetricsCollector(systemUsageOptionsSetup);
            //services.AddAppMetricsGcEventsMetricsCollector(gcEventsOptionsSetup);
            return this;
        }
    }
}