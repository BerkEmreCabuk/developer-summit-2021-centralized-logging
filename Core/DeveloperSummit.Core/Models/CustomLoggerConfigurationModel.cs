using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperSummit.Core.Models
{
    public class CustomLoggerConfigurationModel
    {
        public CustomLoggerConfigurationModel()
        { }
        public CustomLoggerConfigurationModel(string application, string instanceId)
        {
            Application = application;
            InstanceId = instanceId;
        }
        public string Application { get; set; }
        public string InstanceId { get; set; }
        public LogEventLevel DefaultLogLevel { get; set; } = LogEventLevel.Information;
        public LogEventLevel MicrosoftLogLevel { get; set; } = LogEventLevel.Warning;
        public LogEventLevel SystemLogLevel { get; set; } = LogEventLevel.Warning;
    }
}
