using Account_Loging.BL.Dtos;

namespace Account_Loging.BL.AcountSecvices
{
    public interface IAcountServices
    {
        List<UserDto> GetAll();
        UserDto GetById(int id);
        UserDto Add(UserDto invoice);
        UserDto Update(UserDto invoice);
        void Delete(int id);
    }
}
