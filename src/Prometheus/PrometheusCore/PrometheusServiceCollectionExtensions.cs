using App.Metrics;
using PrometheusCore.Options;
using System;

namespace PrometheusCore
{
    public static class PrometheusServiceCollectionExtensions
    {

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
