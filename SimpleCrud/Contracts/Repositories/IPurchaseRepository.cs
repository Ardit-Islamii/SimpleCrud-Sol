using SimpleCrud.Models;
using System.Threading.Tasks;

namespace SimpleCrud.Contracts.Repositories
{
    public interface IPurchaseRepository
    {
        public Task<Purchase> Create(Purchase entity);
    }
}
