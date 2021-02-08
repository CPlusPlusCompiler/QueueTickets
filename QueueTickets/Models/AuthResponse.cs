using QueueTickets.Entities;
using QueueTickets.Repositories;

namespace QueueTickets.Models
{
    public class AuthResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        public AuthResponse(string name, string surname, string username, string token)
        {
            Name = name;
            Surname = surname;
            Username = username;
            Token = token;
        }


        public AuthResponse(Specialist user, string token)
        {
            Name = user.Name;
            Surname = user.Surname;
            Username = user.Username;
            Token = token;
        }
    }
}