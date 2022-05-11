using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StaffProjectAPI.Controllers;
using StaffProjectAPI.Data;
using StaffProjectAPI.Responses;

namespace StaffProjectAPI.Services
{
    public interface IPaymentService
    {
        List<Payment> GetAllPayments(User user, string sort_by, string sort_type, string s);

        ActionResult<Payment> GetPaymentById(User user, int id);

        PaymentResponse AddPayment(User user, decimal amount, string currency, string payerEmail, DateTime dueToDate, string transactionId);

        PaymentResponse UpdatePayment(User user, int id, decimal amount, string currency, string payerEmail, DateTime dueToDate, string transactionId);

        PaymentResponse DeletePayment(User user, int id);
    }
}
