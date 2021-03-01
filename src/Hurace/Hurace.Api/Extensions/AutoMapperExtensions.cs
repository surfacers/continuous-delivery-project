using AutoMapper;

namespace Hurace.Api.Extensions
{
    public static class AutoMapperExtensions
    {
        public static void CreateMapBidirectional<TSource, TDestination>(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TSource, TDestination>();
            cfg.CreateMap<TDestination, TSource>();
        }
    }
}
