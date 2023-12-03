using Account_Loging.BL.Dtos;
using Account_Loging.DAL.Model;
using AutoMapper;

namespace Account_Loging.BL.Mapper
{
    public class DomainProfile:Profile
    {
        public DomainProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
