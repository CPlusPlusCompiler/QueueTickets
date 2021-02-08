using Microsoft.AspNetCore.Authorization;

namespace QueueTickets.Models
{
    public class Policies
    {
        public const string Specialist = "Specialist";
        public const string User = "User";

        public static AuthorizationPolicy SpecialistPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(Specialist)
                .Build();
        }
    }
}
