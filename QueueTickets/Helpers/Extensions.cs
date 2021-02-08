using System;

namespace QueueTickets.Helpers
{
    public static class Extensions
    {
        public static int ToLocalDayOfWeek(this DayOfWeek dayOfWeek)
        {
            var localDayOfWeek = dayOfWeek switch
            {
                DayOfWeek.Sunday => 7,
                DayOfWeek.Saturday => 6,
                _ => (int) dayOfWeek
            };

            return localDayOfWeek;
        }
    

    }
}