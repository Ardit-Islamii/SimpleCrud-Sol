using AutoMapper;
using InventoryService.Dtos;
using Models;

namespace InventoryService.Profiles
{
    public class InventoryProfiles : Profile
    {
        public InventoryProfiles()
        {
            CreateMap<Inventory, InventoryReadDto>().ReverseMap();
        }
    }
}
