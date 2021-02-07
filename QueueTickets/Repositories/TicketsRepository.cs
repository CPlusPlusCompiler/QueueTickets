using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QueueTickets.Entities;
using QueueTickets.Models;

namespace QueueTickets.Repositories
{
    public class TicketsRepository: ITicketsRepository
    {
        private readonly DatabaseContext _context;

        public TicketsRepository(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Looks for a specialist with given id and includes their schedules and upcoming
        /// </summary>
        /// <param name="specialistId"><see cref="Specialist.Id"/> in the database</param>
        /// <param name="dayOfWeek">Day of week from which schedules will be given</param>
        /// <returns><see cref="Specialist"/> with schedules and upcoming tickets.</returns>
        public async Task<Specialist> GetSpecialistWithData(long specialistId, int dayOfWeek)
        {
            var specialist = await _context.Specialists
                .Where(s => s.Id == specialistId)
                .Include(s => s.WorkSchedules.Where(sc => sc.DayOfWeek >= dayOfWeek))
                .Include(s => s.Tickets.Where(t => t.Status == ReservationStatus.WAITING)
                    .OrderBy(t => t.EndTime)
                    .ThenBy(t => t.PlannedEndTime))
                .FirstAsync();

            return  specialist;
        }
        
        public async Task<string> AddTicket(Ticket ticket)
        {
            var id = _context.Add(ticket).Entity.Uuid;
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<List<Ticket>> GetTickets(int dayOfWeek)
        {
            var tickets = await _context.Tickets
                .OrderBy(t => t.EndTime)
                .ThenBy(t => t.PlannedEndTime)
                .ToListAsync();
            return tickets;
        }


        // public async Task<IEnumerable<Ticket>> GetActiveTickets()
        // {
        //     return await _context.Tickets
        //         .Where(t => t.Status == ReservationStatus.WAITING)
        //         .ToListAsync();
        // }
    }
}