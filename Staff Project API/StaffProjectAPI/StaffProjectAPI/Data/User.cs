using Microsoft.AspNetCore.Identity;

namespace StaffProjectAPI.Data
{
    public class User : IdentityUser
    {
        public List<Payment> Payments { get; set; }
    }
}
