using AutoMapper;
using Models;
using SubscriberExample.Dtos;


namespace SubscriberExample.Profiles
{
    public class SubscriberProfiles : Profile
    {
        public SubscriberProfiles()
        {
            CreateMap<Item, ItemReadDto>();
            CreateMap<ItemPublishedDto, Item>();
        }
    }
}
