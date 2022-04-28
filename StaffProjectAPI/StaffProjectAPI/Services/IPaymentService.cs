using Microsoft.AspNetCore.Mvc;
using StaffProjectAPI.Controllers;
using StaffProjectAPI.Data;
using StaffProjectAPI.Responses;

namespace StaffProjectAPI.Services
{
    public interface IPaymentService
    {
        List<Payment> GetAllPayments(string sort_by, string sort_type, string s);

        ActionResult<Payment> GetPaymentById(int id);

        PaymentResponse AddPayment(decimal amount, string currency, string payerEmail, DateTime dueToDate, string transactionId);

        PaymentResponse UpdatePayment(int id, decimal amount, string currency, string payerEmail, DateTime dueToDate, string transactionId);

        PaymentResponse DeletePayment(int id);
    }
}
