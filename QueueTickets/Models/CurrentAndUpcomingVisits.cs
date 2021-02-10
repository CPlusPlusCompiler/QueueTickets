using System.Collections.Generic;
using QueueTickets.Entities;

namespace QueueTickets.Models
{
    public class CurrentAndUpcomingVisits
    {
        public Ticket CurrentVisit { get; set; }
        public List<Ticket> UpcomingVisits { get; set; }

        
        public CurrentAndUpcomingVisits(Ticket currentVisit, List<Ticket> upcomingVisits)
        {
            CurrentVisit = currentVisit;
            UpcomingVisits = upcomingVisits;
        }
    }
}