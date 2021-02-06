using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueueTickets.Entities
{
    [Table("DaySchedules")]
    public class WorkSchedule
    {
       [Key] 
       public int Id { get; set; }
       [Required]
       public int DayOfWeek { get; set; }
       [Required]
       public DateTime StartTime { get; set; }
       [Required]
       public DateTime EndTime { get; set; }
    }
}