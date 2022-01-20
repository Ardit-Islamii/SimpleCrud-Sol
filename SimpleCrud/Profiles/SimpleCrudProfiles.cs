using AutoMapper;
using Models;
using OrderService.Dtos;

namespace OrderService.Profiles
{
    public class SimpleCrudProfiles : Profile
    {
        public SimpleCrudProfiles()
        {
            CreateMap<Item, ItemReadDto>().ReverseMap();
            CreateMap<Purchase, PurchaseReadDto>().ReverseMap();
        }
    }
}
