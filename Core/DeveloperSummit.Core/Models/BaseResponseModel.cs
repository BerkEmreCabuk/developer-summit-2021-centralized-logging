using System.Net;

namespace DeveloperSummit.Core.Models
{
    public class BaseResponseModel<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
        public bool IsSuccess()
        {
            return StatusCode == HttpStatusCode.OK || StatusCode == HttpStatusCode.Created;
        }
    }
}
