using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QueueTickets.Entities;
using QueueTickets.Models;
using QueueTickets.Repositories;

namespace QueueTickets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly ITicketsRepository _repo;
        private readonly int DEFAULT_MEETING_LENGTH = 40;

        public MeetingsController(ITicketsRepository repo)
        {
            _repo = repo;
        }
        

        [HttpPost("BookMeeting")]
        public async Task<IActionResult> BookMeeting([FromBody] BookMeetingRequest data)
        {
            try
            {
                var date = DateTime.Today;

                // I am using my local week format, so Monday is 1 and Sunday is 7
                var dayOfWeek = 0;
                if (date.DayOfWeek == DayOfWeek.Sunday)
                    dayOfWeek = 7;
                else if (date.DayOfWeek == DayOfWeek.Saturday)
                    dayOfWeek = 6;
                else
                    dayOfWeek = (int)date.DayOfWeek;
                
                var specialistWithData = await _repo.GetSpecialistWithData(data.SpecialistId, dayOfWeek);
                var newNumber = 1L;
                DateTime? plannedStartTime = null;
                DateTime? plannedEndTime = null;

                if (!specialistWithData.WorkSchedules.Any())
                {
                    // todo find a proper statusCode
                    return new ObjectResult("Selected specialist has no schedules registered") {StatusCode = 500};
                }

                var schedules = specialistWithData.WorkSchedules;
                DateTime lastEndTime;

                if (specialistWithData.Tickets.Any())
                {
                    var lastTicket = specialistWithData.Tickets.Last();
                    lastEndTime = lastTicket.EndTime ?? lastTicket.PlannedEndTime;
                    newNumber = lastTicket.TicketNumber + 1L;
                }
                else
                {
                    // todo queryje atrinkti tik ateities schedules.
                    var time = specialistWithData.WorkSchedules.First().StartTime;
                    lastEndTime = date.Add(time);
                }

                for (int i = 0; i < schedules.Count(); i++)
                {
                    var tempEndTime = lastEndTime.AddMinutes(DEFAULT_MEETING_LENGTH);

                    // todo ideti paklaida
                    if (tempEndTime.TimeOfDay >= schedules.ElementAt(i).EndTime)
                    {
                        plannedStartTime = lastEndTime;
                        plannedEndTime = tempEndTime;
                        break;
                    }
                }

                if (plannedEndTime.HasValue && plannedStartTime.HasValue)
                {
                    var id = await _repo.AddTicket(new Ticket(
                        System.Guid.NewGuid().ToString(),
                        newNumber,
                        plannedStartTime.Value,
                        plannedEndTime.Value,
                        data.SpecialistId,
                        data.CustomerName));

                    return Ok(id);
                }

                return new ObjectResult("Sorry, we could not find a time for meeting this week.") {StatusCode = 500};
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ObjectResult result;

                if (e is SqlException)
                    return new ObjectResult("Database error.") {StatusCode = 500};
                else
                    return new ObjectResult("Some kind of another error") {StatusCode = 500};
            }
        }


        [HttpDelete]
        public async Task<IActionResult> CancelMeeting(string meetingId)
        {
            return new ObjectResult("Not yet implemented");
        } 
    }
}