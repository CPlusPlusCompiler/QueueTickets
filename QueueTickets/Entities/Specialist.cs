using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QueueTickets.Models;

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
        [Required]
        
        [ForeignKey("SpecialistPasswords")]
        public int PasswordId { get; set; }

        public SpecialistPassword Password { get; set; }
        public ICollection<WorkSchedule> WorkSchedules { get; set; } 
        public ICollection<Ticket> Tickets { get; set; }
        
        public Specialist() {}

        public Specialist(string username, string name, string surname, int passwordId)
        {
            Username = username;
            Name = name;
            Surname = surname;
            PasswordId = passwordId;
        }
    }
}