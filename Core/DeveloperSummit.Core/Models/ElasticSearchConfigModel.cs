using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperSummit.Core.Models
{
    public class ElasticSearchConfigModel
    {
        public string ConnectionString { get; set; }
        public int PingTimeMilliSeconds { get; set; }
    }
}
