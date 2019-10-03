using Microsoft.Extensions.Logging;

namespace Application.Utility.Models
{
    public class JaegerTracingOptions
    {
        public JaegerTracingOptions(string hostName, int agentPort)
        {
            SamplingRate = 1;
            LowerBound = 1d;
            LoggerFactory = new LoggerFactory();
            JaegerAgentHost = hostName;
            JaegerAgentPort = agentPort;
        }

        public JaegerTracingOptions()
        {
            SamplingRate = 1;
            LowerBound = 1d;
            LoggerFactory = new LoggerFactory();
            JaegerAgentHost = "localhost";
            JaegerAgentPort = 6831;
        }

        public double SamplingRate { get; set; }
        public double LowerBound { get; set; }
        public ILoggerFactory LoggerFactory { get; set; }
        public string JaegerAgentHost { get; set; }
        public int JaegerAgentPort { get; set; }
        public string ServiceName { get; set; }
    }
}