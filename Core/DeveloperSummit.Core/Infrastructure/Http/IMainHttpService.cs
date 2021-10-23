using DeveloperSummit.Core.Enums;
using DeveloperSummit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperSummit.Core.Infrastructure.Http
{
    public interface IMainHttpService
    {
        Task<BaseResponseModel<T>> HttpRequest<T>(
            HttpServiceEnum httpServiceEnum,
            string url,
            HttpMethod httpMethod,
            object payload = null,
            CancellationToken ct = default);
    }
}
