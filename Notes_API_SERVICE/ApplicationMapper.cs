using AutoMapper;
using Notes_API_CORE.Entities;
using Notes_API_SERVICE.DTOs;

namespace Notes_API_SERVICE
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<User, CompleteUserDTO>().ReverseMap();
            CreateMap<User, CreateUserDTO>().ReverseMap();
            CreateMap<User, LoginUserDTO>().ReverseMap();
            CreateMap<User, LoginUserResultDTO>().ReverseMap();
            CreateMap<User, PasswordResetDTO>().ReverseMap();
            CreateMap<User, PasswordResetReturnDTO>().ReverseMap();
            CreateMap<UserNote, ShowNoteDTO>().ReverseMap();
            CreateMap<UserNote, NewNoteDTO>().ReverseMap();
        }
    }
}
