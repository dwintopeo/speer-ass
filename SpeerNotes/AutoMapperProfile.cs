using AutoMapper;

namespace SpeerNotes
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Db.Note, Models.Note>();
        }
    }
}
