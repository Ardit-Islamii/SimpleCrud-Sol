using AutoMapper;
using InventoryService.Dtos;
using Models;

namespace InventoryService.Profiles
{
    public class ItemProfiles : Profile
    {
        public ItemProfiles()
        {
            CreateMap<Item, ItemReadDto>();
            CreateMap<ItemPublishedDto, Item>();
        }
    }
}
