using Microsoft.EntityFrameworkCore;

namespace StaffProjectAPI.Data
{
    public class PaymentsDBContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }
        public PaymentsDBContext(DbContextOptions<PaymentsDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
