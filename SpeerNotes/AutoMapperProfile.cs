using AutoMapper;

namespace SpeerNotes
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Db.Note, Models.Note>();
            //CreateMap<Db.BankApiClient, Models.BankApiClient>();
            //CreateMap<Db.BankApiClientModule, Models.BankApiClientModule>();
        }
    }
}
