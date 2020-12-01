using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForagerWebAPIDB.Data;
using ForagerWebAPIDB.Models;
using System.Text.Json;

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
        [HttpPost]
        public async Task<ActionResult<string>> CreateUser([FromQuery] string userAsString)
        {
            User user = JsonSerializer.Deserialize<User>(userAsString);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string id = await users.CreateUserAsync(user);
                return Ok(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}
