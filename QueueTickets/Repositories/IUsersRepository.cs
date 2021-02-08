using System.Collections;
using System.Threading.Tasks;
using QueueTickets.Entities;
using QueueTickets.Models;

namespace QueueTickets.Repositories
{
    public interface IUsersRepository
    {
        public Task<AuthResponse> Authenticate(AuthRequest request);
        public Task<Specialist> GetByUsername(string username);

        /// <summary> Registers a user and saves password to another table</summary>
        /// <remarks> Used as a helper to populate the database in this project </remarks>
        /// <param name="request"></param>
        /// <returns>Registered user's id or an error string</returns>
        public Task<long> Register(RegisterRequest request);

        public Task<Specialist> GetById(int userId);
    }
}