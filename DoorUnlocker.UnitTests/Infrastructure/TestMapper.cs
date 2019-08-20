using System;
using AutoMapper;
using DoorUnlocker.API.Application.Mapping;

namespace DoorUnlocker.UnitTests.Infrastructure
{
    public static class TestMapper
    {
        public static readonly Lazy<IMapper> Instance = new Lazy<IMapper>(Init);

        private static IMapper Init()
        {
            var config = new MapperConfiguration(conf =>
            {
                conf.AddProfile<DoorsProfile>();
            });
            
            return new Mapper(config);
        }
    }
}