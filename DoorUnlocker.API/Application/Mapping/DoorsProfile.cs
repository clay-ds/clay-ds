using System;
using System.Security.Claims;
using AutoMapper;
using DoorUnlocker.API.Application.Domain.Models;
using DoorUnlocker.API.Infrastructure.Helpers;
using DoorUnlocker.API.Models.Entrance;
using DoorUnlocker.API.Models.Offices;
using DoorUnlocker.API.Models.Permissions;

namespace DoorUnlocker.API.Application.Mapping
{
    public class DoorsProfile : Profile
    {
        public DoorsProfile()
        {
            CreateMap<CreateDoorRequest, Door>();

            CreateMap<CreateOfficeRequest, Office>();

            CreateMap<Office, OfficeModel>();

            CreateMap<Door, DoorModel>();
            
            CreateMap<DoorPermission, PermittedDoorModel>()
                .ForMember(dest => dest.Id, m => m.MapFrom(src => src.DoorId))
                .ForMember(dest => dest.Name, m => m.MapFrom(src => src.Door.Name))
                .ForMember(dest => dest.OfficeId, m => m.MapFrom(src => src.Door.OfficeId))
                .ForMember(dest => dest.OfficeName, m => m.MapFrom(src => src.Door.Office.Name));

            CreateMap<Door, EntranceLogEntry>()
                .ForMember(dest => dest.Id, m => m.Ignore())
                .ForMember(dest => dest.DoorId, m => m.MapFrom(src => src.Id))
                .ForMember(dest => dest.DoorName, m => m.MapFrom(src => src.Name))
                .ForMember(dest => dest.EntranceDate, m => m.MapFrom(_ => DateTime.UtcNow));

            CreateMap<ClaimsPrincipal, EntranceLogEntry>()
                .ForMember(dest => dest.UserId, m => m.MapFrom(src => src.GetUserId()))
                .ForMember(dest => dest.UserName, m => m.MapFrom(src => src.GetUserName()));

            CreateMap<EntranceLogEntry, EntranceHistoryModel>();
        }
    }
}