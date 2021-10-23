using System.Threading.Tasks;

namespace DeveloperSummit.Core.Infrastructure.RabbitMq
{
    public interface IRabbitMqService
    {
        Task PublishAsync<T>(string name, T data);
    }
}
