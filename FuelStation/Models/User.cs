using Microsoft.AspNetCore.Identity;

namespace FuelStation.Models
{
    public class User: IdentityUser
    {
        public int Year { get; set; }
    }
}
