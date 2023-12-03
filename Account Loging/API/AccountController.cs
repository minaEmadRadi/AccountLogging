using Account_Loging.BL.AcountSecvices;
using Account_Loging.BL.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Account_Loging.API
{
    public class AccountController : Controller 
    {
        private readonly IAcountServices _acountServices;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAcountServices acountServices,ILogger<AccountController> logger)
        {
            _acountServices = acountServices;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_acountServices.GetAll());
        }
        [HttpGet("{id}")]
        public IActionResult GetUserbyId(int id)
        {
            return Ok(_acountServices.GetById(id));
        }

        [HttpPost("Action")]
        public IActionResult AddUser([FromBody] UserDto userDto)
        {
            _acountServices.Add(userDto);

            return Ok();
        }


        [HttpPut("Action")]
        public IActionResult UpdateUser([FromBody] UserDto userDto)
        {
            _acountServices.Update(userDto);

            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int userId)
        {
            _acountServices.Delete(userId);

            return Ok();
        }

        //[HttpPost("Action")]
        //public IActionResult Register([FromBody] UserDto userDto)
        //{
        //    _acountServices.Add(userDto);

        //    return Ok();
        //}

        //[HttpPost("Action")]
        //public IActionResult Login([FromBody] UserDto userDto)
        //{
        //   if (!ModelState.IsValid)
        //    {
        //        _acountServices.GetById(userDto.Id);
              
        //    }

        //    return Ok();
        //}


      
    }
}
