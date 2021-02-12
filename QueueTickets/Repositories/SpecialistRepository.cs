using Microsoft.EntityFrameworkCore;
using QueueTickets.Models;
using System.Linq;
using System.Threading.Tasks;
using QueueTickets.Entities;

namespace QueueTickets.Repositories
{
    public class SpecialistRepository : ISpecialistRepository
    {
        private readonly DatabaseContext _context;

        public SpecialistRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<CurrentAndUpcomingVisits> GetCurrentAndUpcomingVisits(long specialistId)
        {
            var currentVisit = await _context.Tickets
                .Where(t => t.SpecialistId == specialistId
                && t.Status == Entities.VisitStatus.VISITING)
                .FirstOrDefaultAsync();

            var upcomingVisits = await _context.Tickets
                .Where(t => t.SpecialistId == specialistId
                && t.Status == Entities.VisitStatus.WAITING)
                .Take(5)
                .ToListAsync();

            var visits = new CurrentAndUpcomingVisits(currentVisit, upcomingVisits);
            return visits;
        }


        public async Task<bool> SetTicketStatus(string ticketId, Entities.VisitStatus status)
        {
            var ticket = await _context.Tickets.FirstAsync();

            if (ticket == null)
                return false;

            ticket.Status = status;
            _context.SaveChanges();
            return true;
        }
    }
}