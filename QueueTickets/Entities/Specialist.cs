using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueueTickets.Entities
{
    [Table("Specialists")]
    public class Specialist
    {
        [Key]
        public long Id { get; set; } 
        [Required]
        public string Username { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        
        public ICollection<WorkSchedule> WorkSchedules { get; set; } 
        public ICollection<Ticket> Tickets { get; set; }
        
        public Specialist() {}
    }
}