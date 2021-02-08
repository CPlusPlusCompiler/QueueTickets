using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueTickets.Repositories;

namespace QueueTickets.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class SpecialistConsoleController: ControllerBase
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
            return BadRequest();
        }


        [HttpPost("setvisitstarted")]
        public async Task<IActionResult> SetVisitStarted([FromBody] string ticketId)
        {
            return BadRequest();
        }


        [HttpPost("setvisitended")]
        public async Task<IActionResult> SetVisitEnded([FromBody] string ticketId)
        {
            return BadRequest();
        }
      
        
        // todo gauti iš tokeno vartotojo id
        [HttpGet("getcurrentvisits")]
        public async Task<IActionResult> GetCurrentVisits()
        {
            return BadRequest();
        }
    }
}