using AutoMapper;

namespace DoorUnlocker.API.Infrastructure.Helpers
{
    public static class MapperHelpers
    {
        public static TResult MultipleMap<TResult>(this IMapper mapper, object initial, params object[] others)
        {
            var result = mapper.Map<TResult>(initial);

            foreach (var other in others)
            {
                mapper.Map(other, result);
            }

            return result;
        }
    }
}