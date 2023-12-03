using Account_Loging.BL.Dtos;
using Account_Loging.DAL.Model;
using Account_Loging.DAL.Repositories;
using AccountLog.DAL.DbContainer;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Account_Loging.BL.AcountSecvices
{
    public class AcountServices:IAcountServices
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IRepository<User> _repositoryUser;
      
        private readonly AccountLogContext _context;

        public AcountServices(IMapper mapper,IConfiguration configuration, IRepository<User> repositoryUser, AccountLogContext context)
        {
            _mapper = mapper;
            _configuration = configuration;
            _repositoryUser = repositoryUser;
            _context = context;
        }
        public UserDto Add(UserDto UserDto)
        {
            CreatepasswordHash(UserDto.Password, out byte[] PasswordHash, out byte[] PasswordSalt);
            if (UserDto == null)
                throw new ArgumentNullException(nameof(UserDto));
            var UserEnt = _mapper.Map<User>(UserDto);
            UserEnt.PasswordHash = PasswordHash;
            UserEnt.PasswordSalt = PasswordSalt;
            _repositoryUser.Add(UserEnt);
            _context.SaveChanges();
            UserDto = _mapper.Map<UserDto>(UserEnt);
            return UserDto;
        }

        public void Delete(int id)
        {
            //var UserEnt= _mapper.Map<User>(id);
            var UserEnt = _repositoryUser.Find(id);
            _repositoryUser.Delete(UserEnt);
            _context.SaveChanges();
        }

        public List<UserDto> GetAll()
        {
            var UserEnt = _repositoryUser.GetAll();
           
            var UserDto = _mapper.Map<List<UserDto>>(UserEnt);
            
            return UserDto;
        }

        public UserDto GetById(int id)
        {
            var UserEnt = _repositoryUser.Find(id);
            var pass = Getpassword(UserEnt.PasswordHash);
            var UserDto = _mapper.Map<UserDto>(UserEnt);
            UserDto.Password = pass;
            return UserDto;
        }

        public UserDto Update(UserDto UserDto)
        {
            var User = _mapper.Map<User>(UserDto);
            _repositoryUser.Update(User);
            UserDto = _mapper.Map<UserDto>(User);
             
            _context.SaveChanges();
            return UserDto;
        }

        private string CeateToken(UserDto userDto)
        {
            List<Claim> claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name,userDto.UserName),
                new Claim(ClaimTypes.Role,"User")
            };
            var key =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSetting.Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                claims: claim,
                expires: DateTime.Now.AddDays(1),
                signingCredentials:cred
                );
            var jwt=new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;

        }



        private void CreatepasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using (var Hmac = new HMACSHA512())
            {
                PasswordSalt = Hmac.Key;
                PasswordHash = Hmac.ComputeHash(Encoding.UTF8.GetBytes(Password));
            }
        }
        //private void GetpasswordHash(out string Password,  byte[] PasswordHash)
        //{
        //    using (var Hmac = new HMACSHA512())
        //    {
        //        string s3 = Convert.ToBase64String(PasswordHash);   
        //        byte[] decByte3 = Convert.FromBase64String(s3);
        //        string data = BitConverter.ToString(PasswordHash);
        //        Password = UTF8Encoding.GetDecoder().GetHashCode(s3);
        //    }
        //}

        //public string Getpassword(string encodedData)
        //{
        //    System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        //    System.Text.Decoder utf8Decode = encoder.GetDecoder();
        //    byte[] todecode_byte = Convert.FromBase64String(encodedData);
        //    int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        //    char[] decoded_char = new char[charCount];
        //    utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        //    string result = new String(decoded_char);
        //    return result;
        //}

        private string Getpassword( byte[] PasswordHash)
        {

            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            int charCount = utf8Decode.GetCharCount(PasswordHash, 0, PasswordHash.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(PasswordHash, 0, PasswordHash.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
            
        }

        private bool VerfiypasswordHash(string Password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using (var Hmac = new HMACSHA512())
            {
                var computedHash = Hmac.ComputeHash(Encoding.UTF8.GetBytes(Password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != PasswordHash[i]) return false;
                }
                return computedHash.SequenceEqual(PasswordHash);
            }
        }
    }
}
