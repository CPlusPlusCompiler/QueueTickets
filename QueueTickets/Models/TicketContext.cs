using Microsoft.EntityFrameworkCore;
using QueueTickets.Entities;

namespace QueueTickets.Models
{
    public class TicketContext: DbContext
    {
        public DbSet<Ticket> Tickets { get; set; }

        public TicketContext(DbContextOptions options): base(options)
        {
        }
    }
}