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
        private readonly TicketContext _context;

        public TicketsRepository(TicketContext context)
        {
            _context = context;
        } 
       
        
        public async Task<string> AddTicket(Ticket ticket)
        {
            var id = _context.Add(ticket).Entity.Uuid;
            await _context.SaveChangesAsync();
            return id;
        }

        
        // public async Task<IEnumerable<Ticket>> GetActiveTickets()
        // {
        //     return await _context.Tickets
        //         .Where(t => t.Status == ReservationStatus.WAITING)
        //         .ToListAsync();
        // }
    }
}