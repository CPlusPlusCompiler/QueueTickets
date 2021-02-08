using System.ComponentModel.DataAnnotations.Schema;

namespace QueueTickets.Entities
{
    [Table("SpecialistPasswords")]
    public class SpecialistPassword
    {
        public int Id { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }

        public SpecialistPassword()
        {
        }

        public SpecialistPassword(string salt, string hash)
        {
            Salt = salt;
            Hash = hash;
        }
    }
}