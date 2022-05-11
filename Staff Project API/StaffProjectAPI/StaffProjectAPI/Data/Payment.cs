namespace StaffProjectAPI.Data
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string PayerEmail { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime DueToDate { get; set; }
        public string TransactionId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
