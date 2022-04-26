using Microsoft.AspNetCore.Mvc;
using StaffProjectAPI.Controllers;
using StaffProjectAPI.Data;
using StaffProjectAPI.Repositories;
using StaffProjectAPI.Responses;

namespace StaffProjectAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepositoryPayment _repositoryPayment;

        public PaymentService(IRepositoryPayment repository)
        {
            _repositoryPayment = repository;
        }

        public List<Payment> GetAllPayments()
        {
            return _repositoryPayment.GetPaymentList().ToList();
        }

        public ActionResult<Payment> GetPaymentById(int id)
        {
            return _repositoryPayment.GetPayment(id);
        }

        public PaymentResponse AddPayment(decimal amount, string currency, string payerEmail, DateTime dueToDate, string transactionId)
        {
            List<Error> errors = 
                PaymentValidation.Validate(amount, currency, payerEmail, DateTime.Now, dueToDate, transactionId);
            
            if (errors.Count > 0)
                return errors;

            Payment payment = new Payment
            {
                Amount = amount,
                Currency = currency,
                PayerEmail = payerEmail,
                RequestDate = DateTime.Now,
                DueToDate = dueToDate,
                TransactionId = transactionId
            };

            _repositoryPayment.Create(payment);
            _repositoryPayment.Save();

            return new PaymentResponse { Succeeded = true, Payment = payment };
        }

        public PaymentResponse UpdatePayment(int id, decimal amount, string currency, string payerEmail, DateTime dueToDate, string transactionId)
        {
            List<Error> errors =
                PaymentValidation.Validate(amount, currency, payerEmail, DateTime.Now, dueToDate, transactionId);

            if (errors.Count > 0)
                return errors;

            Payment payment = _repositoryPayment.GetPayment(id);

            if(payment is null)
                return new PaymentResponse { Payment = null };

            payment.Amount = amount;
            payment.Currency = currency;
            payment.PayerEmail = payerEmail;
            payment.DueToDate = dueToDate;
            payment.TransactionId = transactionId;

            _repositoryPayment.Update(payment);
            _repositoryPayment.Save();

            return new PaymentResponse { Succeeded = true, Payment = payment };
        }

        public PaymentResponse DeletePayment(int id)
        {
            Payment deletedPayment = _repositoryPayment.GetPayment(id);

            if (deletedPayment is null)
                return false;

            _repositoryPayment.Delete(id);
            _repositoryPayment.Save();

            return new PaymentResponse { Succeeded = true, Payment = deletedPayment };
        }
    }
}
