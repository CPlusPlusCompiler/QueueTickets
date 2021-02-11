using QueueTickets.Entities;
using QueueTickets.Models;
using System.Threading.Tasks;

namespace QueueTickets.Repositories
{
    public interface ISpecialistRepository
    {
        public Task<CurrentAndUpcomingVisits> GetCurrentAndUpcomingVisits(long specialistId);
        public Task<bool> SetTicketStatus(string ticketId, VisitStatus status);
    }
}