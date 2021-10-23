using DeveloperSummit.Core.Constants;
using DeveloperSummit.Core.Enums;
using DeveloperSummit.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperSummit.Core.Infrastructure.Http
{
    public class MainHttpService : IMainHttpService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly List<string> _headerKeyList;

        public MainHttpService(
            HttpClient client,
            IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            _headerKeyList = new List<string>
            {
                HeaderKeyConstants.CorrelationId,
                HeaderKeyConstants.Version,
                HeaderKeyConstants.RequestSequence
            };
        }

        public async Task<BaseResponseModel<T>> HttpRequest<T>(HttpServiceEnum httpServiceEnum, string url, HttpMethod httpMethod, object payload = null, CancellationToken ct = default)
        {
            try
            {
                _client.BaseAddress = GetUri(httpServiceEnum);
                using (var request = new HttpRequestMessage(httpMethod, url))
                {
                    foreach (var headerKey in _headerKeyList)
                    {
                        if (!request.Headers.Any(x => x.Key == headerKey) &&
                            !string.IsNullOrEmpty(_httpContextAccessor?.HttpContext?.Request.Headers[headerKey]))
                        {
                            request.Headers.Add(headerKey, _httpContextAccessor?.HttpContext?.Request.Headers[headerKey].FirstOrDefault());
                        }
                    }

                    if (payload != null)
                    {
                        var contentJson = JsonConvert.SerializeObject(payload, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore });
                        if (!string.IsNullOrEmpty(contentJson))
                        {
                            var content = new StringContent(contentJson, Encoding.UTF8, "application/json");
                            request.Content = content;
                        }
                    }

                    var responseModel = await _client.SendAsync(request, ct);
                    return new BaseResponseModel<T>
                    {
                        StatusCode = responseModel.StatusCode,
                        Data = JsonConvert.DeserializeObject<T>(await responseModel.Content.ReadAsStringAsync())
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Uri GetUri(HttpServiceEnum httpServiceEnum)
        {
            var baseUrl = "";
            switch (httpServiceEnum)
            {
                case HttpServiceEnum.CARGO:
                    baseUrl = "http://localhost:5101/";
                    break;
                case HttpServiceEnum.INVOICE:
                    baseUrl = "http://localhost:5102/";
                    break;
                case HttpServiceEnum.ORDER:
                    baseUrl = "http://localhost:5103/";
                    break;
                default:
                    break;
            }
            return new Uri(baseUrl);
        }
    }
}
