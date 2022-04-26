using StaffProjectAPI.Responses;
using System.Text.RegularExpressions;

namespace StaffProjectAPI.Services
{
    public static class PaymentValidation
    {
        private static string[] _allowedCurrencies = new string[] { "usd", "eur", "uah" };
        private static string _patternEmail = @"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$";
        private static string _patternTransactionId = @"^\d{8}-\d{2}$";

        public static List<Error>
            Validate(decimal amount, string currency, string payerEmail, DateTime requestDate, DateTime dueToDate, string transactionId)
        {
            List<Error> errors = new List<Error>();

            if (amount <= 0 || amount > 10000000)
                errors.Add(new Error("Amount", "Amount must be positive and less than 10000000"));

            if (_allowedCurrencies.FirstOrDefault(c => c == currency) is null)
                errors.Add(new Error("Currency", "Incorrect currency specified"));

            if (!Regex.IsMatch(payerEmail, _patternEmail))
                errors.Add(new Error("PayerEmail", "Bad email format"));

            if (dueToDate < requestDate)
                errors.Add(new Error("DueToDate", "Request date must be earlier than due to date"));

            if (!Regex.IsMatch(transactionId, _patternTransactionId))
                errors.Add(new Error("TransactionId", "Bad TransactionId format. Must be: ********-** and contain only numbers"));

            return errors;
        }
    }
}
