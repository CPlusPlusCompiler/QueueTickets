namespace QueueTickets.Models
{
    public class SaltAndHash
    {
        public string Salt { get; set; }
        public string Hash { get; set; }

        public SaltAndHash()
        {
        }

        public SaltAndHash(string salt, string hash)
        {
            Salt = salt;
            Hash = hash;
        }
    }
}