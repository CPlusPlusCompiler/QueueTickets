using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QueueTickets.Models;
using QueueTickets.Repositories;

namespace QueueTickets.Controllers
{
    [Route("api/[controller]")]
    public class SpecialistAuthController: ControllerBase
    {
        private readonly IUsersRepository _repo;

        public SpecialistAuthController(IUsersRepository repo)
        {
            _repo = repo;
        }
        // [HttpPost("login")]
        // public async Task<IActionResult> Login([FromBody] LoginRequest user)
        // {
        //     if (user == null)
        //         return BadRequest("No username or password strings entered");
        //     
        // }
        //

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            try
            {
                var result = await _repo.Authenticate(request);
                
                if (result == null)
                    return BadRequest("Bad login data");

                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ObjectResult("Error occured") {StatusCode = 500};
            }
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("No registration data given");
                else if (request.Username.Length == 0 || request.Password.Length == 0)
                    return BadRequest("Empty username or password");
                
                var result = await _repo.Register(request);

                if (result < 0)
                    return BadRequest("User with this username already exists!");
                else
                    return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ObjectResult("Error occured") { StatusCode = 500 };
            }
        }
        
    }
}