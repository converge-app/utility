using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        
        private static void AddTracing(IServiceCollection services)
                {
                    services.AddSingleton(serviceProvider =>
                    {
                        var serviceName = Assembly.GetEntryAssembly()?.GetName().Name;
        
                        Environment.SetEnvironmentVariable("JAEGER_SERVICE_NAME", serviceName);
        
                        var loggerFactory = new LoggerFactory();
                        try
                        {
                            // add agenthost and port
                            var config = Jaeger.Configuration.FromEnv(loggerFactory);
                            var tracer = config.GetTracer();
        
                            GlobalTracer.Register(tracer);
                            return tracer;
                        }
                        catch (System.Exception)
                        {
                            Console.WriteLine("Couldn't register logger");
                        }
        
                        return null;
                    });
        
                    // Add logging
                    services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
                    services.AddOpenTracing();
                }
    }
}