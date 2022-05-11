using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StaffProjectAPI.Data
{
    public class PaymentsDBContext : IdentityDbContext<User>
    {
        public DbSet<Payment> Payments { get; set; }

        public PaymentsDBContext(DbContextOptions<PaymentsDBContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
