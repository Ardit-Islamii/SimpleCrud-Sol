using System.Threading.Tasks;

namespace OrderService.ClientFactory
{
    public interface IClientFactory<T>
    {
        public Task<T> CreateClient(string uri);
    }
}
