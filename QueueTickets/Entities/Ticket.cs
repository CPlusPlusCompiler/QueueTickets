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
        public DateTime StartTime { get; set; }
        
        /// <summary> What time the visit actually ended </summary>
        public DateTime EndTime { get; set; }
        
        [Required]
        public ReservationStatus Status { get; set; }
        public Ticket() {}

        public Ticket(string uuid, long ticketNumber, DateTime plannedStartTime, DateTime plannedEndTime)
        {
            Uuid = uuid;
            TicketNumber = ticketNumber;
            PlannedStartTime = plannedStartTime;
            PlannedEndTime = plannedEndTime;
            Status = ReservationStatus.WAITING;
        }
    }

    public enum ReservationStatus
    {
        WAITING,
        VISITING,
        DONE,
        CANCELLED
    }
}