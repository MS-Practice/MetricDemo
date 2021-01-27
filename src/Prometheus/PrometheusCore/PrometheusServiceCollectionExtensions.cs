using App.Metrics;
using App.Metrics.Formatters.Prometheus;
using Microsoft.Extensions.DependencyInjection;
using PrometheusCore.Options;
using System;

namespace PrometheusCore
{
    public static class PrometheusServiceCollectionExtensions
    {
        public static PrometheusServiceBuilder AddPrometheusCore(this IServiceCollection services)
        {
            var metricsRoot = AppMetrics.CreateDefaultBuilder()
                .OutputMetrics.AsPrometheusPlainText()
                .OutputMetrics.AsPrometheusProtobuf()
                .Build();
           
            return AddPrometheusCore(services, metricsRoot);
        }

        public static PrometheusServiceBuilder AddPrometheusCore(this IServiceCollection services, IMetricsBuilder metricsBuilder)
        {
            services.AddMetrics(metricsBuilder);
            AddServices(services);
            return new PrometheusServiceBuilder(services);
        }

        public static PrometheusServiceBuilder AddPrometheusCore(this IServiceCollection services, IMetricsRoot metricsRoot) {
            services.AddMetrics(metricsRoot);
            AddServices(services);
            return new PrometheusServiceBuilder(services);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddMetricsTrackingMiddleware();
            services.AddMetricsEndpoints(options =>
            {
                options.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                options.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
            });
        }
    }

    public static class IMetricsBuilderExtensions
    {
        public static void UseDatabaseMetrics(this IMetricsBuilder metricsBuilder, Action<DatabaseMetricsOption> optionsConfig)
        {
            var databaseMetricsOption = new DatabaseMetricsOption();
            optionsConfig.Invoke(databaseMetricsOption);

            metricsBuilder.Report.ToInfluxDb(options =>
            {
                options.InfluxDb.BaseUri = new Uri(databaseMetricsOption.DatabaseServer);
                options.InfluxDb.Database = databaseMetricsOption.Database;
                options.InfluxDb.UserName = databaseMetricsOption.Uid;
                options.InfluxDb.Password = databaseMetricsOption.Password;
                options.HttpPolicy.Timeout = TimeSpan.FromSeconds(databaseMetricsOption.Timeout);
                options.FlushInterval = TimeSpan.FromSeconds(databaseMetricsOption.FlushInterval);
            });
        }
    }
}
