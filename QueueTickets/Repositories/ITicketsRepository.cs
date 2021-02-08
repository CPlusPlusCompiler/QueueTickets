using System.Collections.Generic;
using System.Threading.Tasks;
using QueueTickets.Entities;

namespace QueueTickets.Repositories
{
    public interface ITicketsRepository
    {
        public Task<string> AddTicket(Ticket ticket);

        public Task<List<Ticket>> GetTickets(int dayOfWeek);
        public Task<Specialist> GetSpecialistWithData(long specialistId, int dayOfWeek);

        public Task CancelMeeting(string uuid);
    }
}