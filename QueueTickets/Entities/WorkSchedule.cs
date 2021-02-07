using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueueTickets.Entities
{
    /// <summary>
    /// Represents a time between a start time and an end time. Can be multiple in a day.
    /// Useful if there is a need for multiple breaks.
    /// </summary>
    /// <remarks> Should be deleted from table if not needed anymore.</remarks>
    [Table("WorkSchedules")]
    public class WorkSchedule
    {
       [Key] 
       public int Id { get; set; }
       
       /// <remarks> Values go from 1 to 7. Monday is 1, Sunday is 7. </remarks>
       [Required]
       public int DayOfWeek { get; set; }
       [Required]
       public TimeSpan StartTime { get; set; }
       [Required]
       public TimeSpan EndTime { get; set; }
       [Required]
       [ForeignKey("Specialists")]
       public long SpecialistId { get; set; }
       
       public WorkSchedule() {}
    }
}