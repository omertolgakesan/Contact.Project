using AutoMapper;
using Contact.Api.Common.Conracts;
using System;

namespace Contact.Api.Mapper
{
    public class MapperBase : IMapperBase
    {
        private readonly IMapper mapper;
        public MapperBase(IMapper iMapper)
        {
            mapper = iMapper;
        }
        public TDestination Map<TDestination>(object source)
        {
            return mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return mapper.Map<TDestination>(source);
        }
    }
}
