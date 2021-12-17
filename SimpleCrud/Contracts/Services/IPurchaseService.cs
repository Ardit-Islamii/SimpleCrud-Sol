using System.Threading.Tasks;
using Models;

namespace OrderService.Contracts.Services
{
    public interface IPurchaseService
    {
        public Task<Purchase> Create(Item entity);
    }
}
