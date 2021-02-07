using Microsoft.EntityFrameworkCore;
using QueueTickets.Entities;

namespace QueueTickets.Models
{
    public class DatabaseContext: DbContext
    {
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Specialist> Specialists { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }

        public DatabaseContext(DbContextOptions options): base(options)
        {
        }
    }
}