using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForagerWebAPIDB.Data;
using ForagerWebAPIDB.Models;

namespace ForagerWebAPIDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private IUserService users;

        public UserLoginController(IUserService userService)
        {
            users = userService;
        }
        // GET: api/<UserLoginController>
        [HttpGet]
        public async Task<ActionResult<User>> LogInUserAsync([FromQuery] string Email, [FromQuery] string pass)
        {
            User user;
            try
            {
                user = await users.ValidateUserAsync(Email, pass);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return StatusCode(500, e.Message);
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUserAsync(int id)
        {
            User user;
            try
            {
                user = await users.GetUserAsync(id);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return StatusCode(500, e.Message);
            }
        }
    }
}
