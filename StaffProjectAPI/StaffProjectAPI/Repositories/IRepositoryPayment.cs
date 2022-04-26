namespace StaffProjectAPI.Repositories
{
    using Data;

    public interface IRepositoryPayment
    {
        IEnumerable<Payment> GetPaymentList();
        Payment GetPayment(int id);
        void Create(Payment item);
        void Update(Payment item);
        void Delete(int id);
        void Save();
    }
}
