using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperSummit.Core.Models
{
    public class IndexResponseModel : IElasticsearchResponse
    {
        private bool _isValid = false;
        public bool IsValid { get { return ApiCall != null ? ApiCall.Success : _isValid; } set { _isValid = value; } }
        public string StatusMessage { get { return ApiCall != null ? ApiCall.ToString() : null; } }
        public Exception Exception { get { return ApiCall != null ? ApiCall.OriginalException : null; } }
        public IApiCallDetails ApiCall { get; set; }

        public bool TryGetServerErrorReason(out string reason)
        {
            reason = "";
            return false;
        }
    }
}
