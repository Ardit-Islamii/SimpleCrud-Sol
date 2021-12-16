using Models;
using Refit;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace SimpleCrud.DataAccess
{
    public interface IInventoryData
    {
        [Get("/{itemId}")]
        public Task<Inventory> GetInventory(Guid itemId);
    }
}
