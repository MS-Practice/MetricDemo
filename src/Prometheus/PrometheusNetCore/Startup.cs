using App.Metrics;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PrometheusCore;
using PrometheusNetCore.Healthchecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrometheusNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Metrics
            var metricsRoot = AppMetrics.CreateDefaultBuilder()
                .OutputMetrics.AsPrometheusPlainText()
                .OutputMetrics.AsPrometheusProtobuf()
                .Build();
            services.AddMetrics(metricsRoot);
            //services.AddMetrics(builder =>
            //{
            //    builder.OutputMetrics.AsPrometheusPlainText();
            //    builder.OutputMetrics.AsPrometheusProtobuf();
            //    //builder.UseDatabaseMetrics(option =>
            //    //{
            //    //    option.Database = "db_cp_config";
            //    //    option.DatabaseServer = "http://192.168.3.10";
            //    //    option.Uid = "root";
            //    //    option.Password = "123456";
            //    //    option.Timeout = 10;
            //    //    option.FlushInterval = 10;
            //    //});
            //});
            services.AddMetricsTrackingMiddleware();
            services.AddMetricsReportingHostedService();

            services.AddMetricsEndpoints(options =>
            {
                options.MetricsEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                options.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
            });

            services.AddAppMetricsHealthPublishing();
            #endregion

            services.AddControllers();

            services.AddHealthChecks()
                .AddCheck<HelloworldHealthCheck>("helloworld_health_check")
                .AddCheck("helloworld_health_check_unhealth", () => HealthCheckResult.Unhealthy("Example is Unhealth"), tags: new[] { "unhealth" });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMetricsAllMiddleware();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
