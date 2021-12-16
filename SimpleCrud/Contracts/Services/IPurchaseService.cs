using Models;
using SimpleCrud.Models;
using System.Threading.Tasks;

namespace SimpleCrud.Contracts.Services
{
    public interface IPurchaseService
    {
        public Task<Purchase> Create(Item entity);
    }
}
