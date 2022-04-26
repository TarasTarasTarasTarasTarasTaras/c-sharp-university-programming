namespace StaffProjectAPI.Repositories
{
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class SQLRepositoryPayment : IRepositoryPayment
    {
        private PaymentsDBContext db;

        public SQLRepositoryPayment(PaymentsDBContext context)
        {
            this.db = context;
        }

        public IEnumerable<Payment> GetPaymentList()
        {
            return db.Payments.ToList();
        }

        public Payment GetPayment(int id)
        {
            return db.Payments.Find(id);
        }

        public void Create(Payment payment)
        {
            db.Payments.Add(payment);
        }

        public void Update(Payment payment)
        {
            db.Entry(payment).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Payment payment = db.Payments.Find(id);
            if (payment != null)
                db.Payments.Remove(payment);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
