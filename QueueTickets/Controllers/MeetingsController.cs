using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QueueTickets.Entities;
using QueueTickets.Helpers;
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
        

        // todo pakeisti šitą nesąmonę pagal emailą
        [HttpPost("BookMeeting"), Authorize]
        public async Task<IActionResult> BookMeeting([FromBody] BookMeetingRequest data)
        {
            try
            {
                var date = DateTime.Today;

                // I am using my local week format, so Monday is 1 and Sunday is 7
                var dayOfWeek = date.DayOfWeek.ToLocalDayOfWeek();

                var specialistWithData = await _repo.GetSpecialistWithData(data.SpecialistId, dayOfWeek);
                if (!specialistWithData.WorkSchedules.Any())
                {
                    // todo find a proper statusCode
                    return new ObjectResult("Selected specialist has no schedules registered") {StatusCode = 500};
                }
                         
                var newNumber = 1L;
                DateTime? plannedStartTime = null;
                DateTime? plannedEndTime = null;

                var schedules = specialistWithData.WorkSchedules;
                DateTime lastEndTime;

                if (specialistWithData.Tickets.Any())
                {
                    lastEndTime = specialistWithData.Tickets.Max(s => s.EndTime ?? s.PlannedEndTime);
                    var largestNumber = specialistWithData.Tickets.Max(s => s.TicketNumber);
                    newNumber = largestNumber + 1L;
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
                    // todo i diena neatsizvelgia...
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

                if (e is SqlException)
                    return new ObjectResult("Database error.") {StatusCode = 500};
                else
                    return new ObjectResult("Some kind of another error") {StatusCode = 500};
            }
        }


        [HttpPost("CancelMeeting")]
        public async Task<IActionResult> CancelMeeting([FromBody]string uuid)
        {
            try
            {
                await _repo.CancelMeeting(uuid);
                return Ok("Cancelled successfully");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return new ObjectResult("Failed to cancel the meeting.") { StatusCode = 500 };
            }
        }
      
    }
}