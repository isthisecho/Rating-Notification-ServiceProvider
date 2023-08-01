using AutoMapper;

namespace HomeRun.RatingService.Mapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            MapperConfiguration mappingConfigs = new MapperConfiguration(
                x => {
                    x.CreateMap<Rating, RatingDTO>().ReverseMap();
                     }
                );

            return mappingConfigs;
        }


    }
}
