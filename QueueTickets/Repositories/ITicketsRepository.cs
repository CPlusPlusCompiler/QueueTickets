using System.Threading.Tasks;
using QueueTickets.Entities;

namespace QueueTickets.Repositories
{
    public interface ITicketsRepository
    {
        public Task<string> AddTicket(Ticket ticket);
    }
}