using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QueueTickets.Entities;
using QueueTickets.Repositories;

namespace QueueTickets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingsController: ControllerBase
    {
        private readonly ITicketsRepository _repo;

        public MeetingsController(ITicketsRepository repo)
        {
            _repo = repo;
        }


        [HttpGet("BookMeeting")]
        public async Task<IActionResult> BookMeeting(long specialistId)
        {
            try
            {
                var id = await _repo.AddTicket(new Ticket("testteestestestes", 11, DateTime.Now, DateTime.Now));
                return Ok(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                if (e is SqlException)
                {
                    var result = new ObjectResult("Database error.");
                    return result;
                }
                else
                {
                    var result = new ObjectResult("Some kind of another error");
                    return result;
                }
            }
        }
    }
}