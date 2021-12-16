using Models;
using SimpleCrud.Contracts.Repositories;
using SimpleCrud.Contracts.Services;
using SimpleCrud.Models;
using System.Threading.Tasks;

namespace SimpleCrud.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchaseService(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<Purchase> Create(Item entity)
        {
            Purchase purchase = new Purchase()
            {
                ItemId = entity.Id,
                Amount = 1
            };
            return await _purchaseRepository.Create(purchase);
        }
    }
}
