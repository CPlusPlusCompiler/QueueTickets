using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueTickets.Entities;
using QueueTickets.Repositories;

namespace QueueTickets.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class SpecialistConsoleController : ControllerBase
    {
        private readonly ISpecialistRepository _repo; 

        public SpecialistConsoleController(ISpecialistRepository repo)
        {
            _repo = repo;
        }


        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok("ok");
        }


        [HttpPost("cancelvisit")]
        public async Task<IActionResult> CancelVisit([FromBody] string ticketId)
        {
            try
            {
                var success = await _repo.SetTicketStatus(ticketId, VisitStatus.VISITING);
                if (!success)
                    return BadRequest("Incorrect ticket id!");
                else
                    return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ObjectResult("Error occured") { StatusCode = 500 };
            }    
        }


        [HttpPost("setvisitstarted")]
        public async Task<IActionResult> SetVisitStarted([FromBody] string ticketId)
        {
            try
            {
                var success = await _repo.SetTicketStatus(ticketId, VisitStatus.VISITING);
                if (!success)
                    return BadRequest("Incorrect ticket id!");
                else
                    return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ObjectResult("Error occured") { StatusCode = 500 };
            }
        }


        [HttpPost("setvisitended")]
        public async Task<IActionResult> SetVisitEnded([FromBody] string ticketId)
        {
            try
            {
                var success = await _repo.SetTicketStatus(ticketId, VisitStatus.DONE);
                if (!success)
                    return BadRequest("Incorrect ticket id!");
                else
                    return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ObjectResult("Error occured") { StatusCode = 500 };
            }
        }


        // todo gauti iš tokeno vartotojo id
        [HttpGet("getcurrentvisits")]
        public async Task<IActionResult> GetCurrentVisits()
        {
            try
            {
                var user = HttpContext.User;
                if(user.HasClaim(c => c.Type == "id"))
                {
                    var userIdStr = user.Claims.FirstOrDefault(c => c.Type == "id").Value;
                    var userId = Convert.ToInt64(userIdStr);
                    var visits = await _repo.GetCurrentAndUpcomingVisits(userId);
                              
                    return Ok(visits);
                }

                return BadRequest("Unknown user");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ObjectResult("Error occured") { StatusCode = 500};
            }
        }
    }
}