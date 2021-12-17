using System.Threading.Tasks;
using Models;

namespace OrderService.Contracts.Repositories
{
    public interface IPurchaseRepository
    {
        public Task<Purchase> Create(Purchase entity);
    }
}
