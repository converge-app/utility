using System;
using System.Reflection;
using Application.Utility.Models;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTracing;
using OpenTracing.Util;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Application.Utility.Startup
{
    public static class Logging
    {
        public static void CreateLogger()
        {
            var elasticUri = Environment.GetEnvironmentVariable("ELASTICSEARCH_URI");
            if (!string.IsNullOrEmpty(elasticUri))
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                    {
                        AutoRegisterTemplate = true
                    }).CreateLogger();
        }

        public static ILoggerFactory AddLogging(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            return loggerFactory;
        }

        public static IServiceCollection AddTracing(this IServiceCollection services, Action<JaegerTracingOptions> setupAction = null)
        {
            if (setupAction != null)
                services.ConfigureJaegerTracing(setupAction);

            services.AddSingleton<ITracer>(cli =>
            {
                var options = cli.GetService<IOptions<JaegerTracingOptions>>().Value;

                var senderConfig = new Jaeger.Configuration.SenderConfiguration(options.LoggerFactory)
                    .WithAgentHost(options.JaegerAgentHost)
                    .WithAgentPort(options.JaegerAgentPort);

                var reporter = new RemoteReporter.Builder()
                    .WithLoggerFactory(options.LoggerFactory)
                    .WithSender(senderConfig.GetSender())
                    .Build();

                var sampler = new GuaranteedThroughputSampler(options.SamplingRate, options.LowerBound);

                var tracer = new Tracer.Builder(options.ServiceName)
                    .WithLoggerFactory(options.LoggerFactory)
                    .WithReporter(reporter)
                    .WithSampler(sampler)
                    .Build();

                if (!GlobalTracer.IsRegistered())
                    GlobalTracer.Register(tracer);
                return tracer;
            });

            services.AddOpenTracing(builder =>
            {
                builder.ConfigureAspNetCore(options =>
                {
                    options.Hosting.IgnorePatterns.Add(x => x.Request.Path == "/api/health/ping");
                    options.Hosting.IgnorePatterns.Add(x => x.Request.Path == "/health");
                    options.Hosting.IgnorePatterns.Add(x => x.Request.Path == "/metrics");
                });
            });

            return services;

        }

        public static void ConfigureJaegerTracing(this IServiceCollection services,
            Action<JaegerTracingOptions> setupAction)
        {
            services.Configure<JaegerTracingOptions>(setupAction);
        }
    }
}