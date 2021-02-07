namespace QueueTickets.Models
{
    public class BookMeetingRequest
    {
        public long SpecialistId { get; set; }
        public string CustomerName { get; set; }

        public BookMeetingRequest(long specialistId, string customerName)
        {
            SpecialistId = specialistId;
            CustomerName = customerName;
        }

        public BookMeetingRequest() { }
    }
}