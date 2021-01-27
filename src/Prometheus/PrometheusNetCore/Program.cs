using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrometheusNetCore
{
    public class Program
    {
        public static IMetricsRoot Metrics { get; set; }
        public static void Main(string[] args)
        {
            //Metrics = AppMetrics.CreateDefaultBuilder()
            //    .OutputMetrics.AsPrometheusPlainText()
            //    .OutputMetrics.AsPrometheusProtobuf()
            //    .Build();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureMetrics(Metrics)
                //.UseMetrics(options => {
                //    options.EndpointOptions = endpoints =>
                //    {
                //        endpoints.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                //        endpoints.MetricsEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                //    };
                //})
                .UseMetrics()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
