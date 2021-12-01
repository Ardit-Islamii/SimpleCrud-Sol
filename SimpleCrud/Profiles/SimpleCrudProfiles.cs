﻿using AutoMapper;
using SimpleCrud.Dtos;
using SimpleCrud.Models;

namespace SimpleCrud.Profiles
{
    public class SimpleCrudProfiles : Profile
    {
        public SimpleCrudProfiles()
        {
            CreateMap<Item, ItemReadDto>();
        }
    }
}
