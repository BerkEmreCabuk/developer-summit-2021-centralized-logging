using DeveloperSummit.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperSummit.Core.Infrastructure.ElasticSearch
{
    public interface IElasticContext
    {
        Task<IndexResponseModel> IndexCustomAsync<T>(string indexName, T document, CancellationToken ct = default) where T : class;
        IndexResponseModel IndexCustom<T>(string indexName, T document) where T : class;
    }
}
