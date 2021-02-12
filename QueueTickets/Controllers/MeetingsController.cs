using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    [AllowAnonymous]
    public class MeetingsController : ControllerBase
    {
        private readonly ITicketsRepository _repo;
        private readonly int DEFAULT_MEETING_LENGTH = 40;
        private readonly string NO_TIME_LEFT = "Sorry, we could not find a time for meeting today.";

        public MeetingsController(ITicketsRepository repo)
        {
            _repo = repo;
        }
        

        [HttpPost("BookMeeting")]
        public async Task<IActionResult> BookMeeting([FromBody] string customerName)
        {
            try
            {
                var date = DateTime.Now;

                // I am using my local week format, so Monday is 1 and Sunday is 7
                var dayOfWeek = date.DayOfWeek.ToLocalDayOfWeek();
                
                var specialistsWithData = await _repo.GetSpecialistsWithData(dayOfWeek, date.TimeOfDay);
                DateTime? earliestTime = null;
                var foundTime = false;
                var specialistId = 0L;
                
                foreach (var specialist in specialistsWithData)
                {
                    if (foundTime)
                        break;
                    
                    if (!specialist.Tickets.Any())
                    {
                        if (specialist.WorkSchedules.Any())
                        {
                            foreach (var schedule in specialist.WorkSchedules)
                            {
                                if (schedule.EndTime >= date.TimeOfDay)
                                {
                                    foundTime = true;
                                    specialistId = specialist.Id;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (specialist.WorkSchedules.Any())
                        {
                            // looking for time in between visits
                            for (int i = 0; i < specialist.Tickets.Count; i++)
                            {
                                var ticket = specialist.Tickets.ElementAt(i);
                                
                                // last ticket
                                if (i + 1 == specialist.Tickets.Count)
                                {
                                    foreach(var schedule in specialist.WorkSchedules)
                                    {
                                        if (date.TimeOfDay >= schedule.StartTime &&
                                            date.TimeOfDay <= schedule.EndTime)
                                        {

                                            if (earliestTime == null || ticket.PlannedEndTime < earliestTime)
                                            {
                                                earliestTime = ticket.PlannedEndTime;
                                                specialistId = specialist.Id;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var nextTicket = specialist.Tickets.ElementAt(i + 1);
                                    
                                    // if fits between times
                                    if (date >= nextTicket.PlannedStartTime &&
                                        date.AddMinutes(DEFAULT_MEETING_LENGTH) <= nextTicket.PlannedEndTime)
                                    {
                                        if (earliestTime == null || ticket.PlannedEndTime < earliestTime)
                                        {
                                            earliestTime = date;
                                            specialistId = specialist.Id;
                                        }
                                    }
                                }
                            }
                        } 
                    }
                };

                if (earliestTime != null)
                {
                    var ticketNumber = await _repo.GetLargestTicketNumber() + 1;
                    var ticket = new Ticket(
                        System.Guid.NewGuid().ToString(),
                        ticketNumber,
                        earliestTime.Value,
                        earliestTime.Value.AddMinutes(DEFAULT_MEETING_LENGTH),
                        specialistId,
                        customerName
                    ); 
                    
                    await _repo.AddTicket(ticket);
                    return Ok(ticket);
                }
                
                return new ObjectResult(NO_TIME_LEFT) {StatusCode = 500};
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