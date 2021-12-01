using AutoMapper;
using SubscriberExample.Dtos;
using SubscriberExample.Models;

namespace SubscriberExample.Profiles
{
    public class SubscriberProfiles : Profile
    {
        public SubscriberProfiles()
        {
            CreateMap<Item, ItemReadDto>();
            CreateMap<ItemPublishedDto, Item>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
