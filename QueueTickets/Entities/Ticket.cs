using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueueTickets.Entities
{
    [Table("Tickets")]
    public class Ticket
    {
        /// <remarks>Might get changed in the future</remarks>
        [Key]
        public string Uuid { get; set; }
        
        [Required]
        public long TicketNumber { get; set; }
        
        [Required]
        public DateTime PlannedStartTime { get; set; }
        
        /// <remarks> Calculated by adding max visit time to <see cref="PlannedStartTime"/> </remarks> 
        [Required]        
        public DateTime PlannedEndTime { get; set; } 
        
        /// <summary> What time the visit actually started </summary>
        public DateTime? StartTime { get; set; }
        
        /// <summary> What time the visit actually ended </summary>
        public DateTime? EndTime { get; set; }
       
        [ForeignKey("Specialists")]
        public long SpecialistId { get; set; }
        
        [Required]
        public string CustomerName { get; set; }

        [Required]
        public VisitStatus Status { get; set; }
        public Ticket() {}

        public Ticket(string uuid, long ticketNumber, DateTime plannedStartTime, DateTime plannedEndTime,
            long specialistId, string customerName)
        {
            Uuid = uuid;
            TicketNumber = ticketNumber;
            PlannedStartTime = plannedStartTime;
            PlannedEndTime = plannedEndTime;
            StartTime = null;
            EndTime = null;
            SpecialistId = specialistId;
            CustomerName = customerName;
            Status = VisitStatus.WAITING;
        }
    }

    public enum VisitStatus
    {
        WAITING,
        VISITING,
        DONE,
        CANCELLED
    }
}