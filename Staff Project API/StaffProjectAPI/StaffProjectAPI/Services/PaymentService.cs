using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public List<Payment> GetAllPayments(User user, string sort_by, string sort_type, string s)
        {
            var payments = _repositoryPayment.GetPaymentList().Where(p => p.User.UserName == user.UserName).ToList();

            if (sort_by != null)
            {
                var property =
                    typeof(Payment)
                    .GetProperties()
                    .FirstOrDefault(prop => prop.Name.ToLower() == sort_by?.ToLower());

                if (property != null)
                {
                    if (sort_type.ToLower() == "asc")
                        payments = payments.OrderBy(x => property.GetValue(x)).ToList();
                    else if (sort_type.ToLower() == "desc")
                        payments = payments.OrderByDescending(x => property.GetValue(x)).ToList();
                }
            }
            
            if (s != null)
            {
                var properties = typeof(Payment).GetProperties();
                var searchedPayments = new List<Payment>();

                foreach(Payment payment in payments)
                    foreach(var property in properties)
                        if (property.GetValue(payment).ToString().ToLower().Contains(s.ToLower()))
                        {
                            searchedPayments.Add(payment);
                            break;
                        }
                return searchedPayments;
            }

            return payments;
        }

        public ActionResult<Payment> GetPaymentById(User user, int id)
        {
            Payment payment = _repositoryPayment.GetPayment(id);

            if (payment.User.UserName == user.UserName)
                return payment;

            return null;
        }

        public PaymentResponse AddPayment(User user, decimal amount, string currency, string payerEmail, DateTime dueToDate, string transactionId)
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
                TransactionId = transactionId,
                User = user
            };

            _repositoryPayment.Create(payment);
            _repositoryPayment.Save();

            return new PaymentResponse { Succeeded = true, Payment = payment };
        }

        public PaymentResponse UpdatePayment(User user, int id, decimal amount, string currency, string payerEmail, DateTime dueToDate, string transactionId)
        {
            List<Error> errors =
                PaymentValidation.Validate(amount, currency, payerEmail, DateTime.Now, dueToDate, transactionId);

            if (errors.Count > 0)
                return errors;

            Payment payment = _repositoryPayment.GetPayment(id);

            if(payment is null || payment.User.UserName != user.UserName)
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

        public PaymentResponse DeletePayment(User user, int id)
        {
            Payment deletedPayment = _repositoryPayment.GetPayment(id);

            if (deletedPayment is null || deletedPayment.User.UserName != user.UserName)
                return false;

            _repositoryPayment.Delete(id);
            _repositoryPayment.Save();

            return new PaymentResponse { Succeeded = true, Payment = deletedPayment };
        }
    }
}
