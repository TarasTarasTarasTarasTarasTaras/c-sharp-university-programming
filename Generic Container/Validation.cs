namespace Generic_Container
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    class Validation
    {
        public static class Payment
        {
            private static string[] _allowedCurrencies = new string[] { "usd", "eur", "uah" };
            private static string _patternEmail = @"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$";
            private static string _patternTransactionId = @"^\d{8}-\d{2}$";

            public static int ValidateId(int id)
            {
                if (id < 0 || id > 10000000)
                    throw new BadModelException("Id must be positive or 0 and less than 10000000");
                return id;
            }

            public static decimal ValidateAmount(decimal amount)
            {
                if (amount <= 0 || amount > 10000000)
                    throw new BadModelException("Amount must be positive and less than 10000000");
                return amount;
            }

            public static string ValidateCurrency(string currency)
            {
                if (_allowedCurrencies.FirstOrDefault(c => c == currency) is null)
                    throw new BadModelException("Incorrect currency specified");
                return currency;
            }

            public static string ValidateEmail(string email)
            {
                if (!Regex.IsMatch(email, _patternEmail))
                    throw new BadModelException("Bad email format");
                return email;
            }

            public static DateTime ValidateRequestDate(DateTime requestDate, DateTime dueToDate)
            {
                if (requestDate.Year < 1980)
                    throw new BadModelException("The year of the date of the request cannot be less than 1980");
                if (requestDate > DateTime.Now)
                    throw new BadModelException($"The date of the request cannot be later {DateTime.Now.ToString("d")}");
                if (dueToDate.ToString("d") != "01.01.0001" && requestDate > dueToDate)
                    throw new BadModelException("Request date must be earlier than due to date");

                return requestDate;
            }

            public static DateTime ValidateDueToDate(DateTime dueToDate, DateTime requestDate)
            {
                if (dueToDate.Year < 1980)
                    throw new BadModelException("The year of the due to date cannot be less than 1980");
                if (dueToDate < requestDate)
                    throw new BadModelException("Request date must be earlier than due to date");

                return dueToDate;
            }

            public static string ValidateTransactionId(string transactionId)
            {
                if (!Regex.IsMatch(transactionId, _patternTransactionId))
                    throw new BadModelException("Bad TransactionId format. Must be: ********-** and contain only numbers");

                return transactionId;
            }
        }

        public static class Jewelry
        {
            private static string _patternCode = @"^\w{5}/\w{1}-\w{2}$";
            private static string[] _allowedMaterial = new string[] { "gold", "silver", "platinum" };
            private static string[] _allowedTypes = new string[] { "rings", "earrings ", "bracelets" };

            public static int ValidateId(int id)
            {
                if (id < 0 || id > 10000000)
                    throw new BadModelException("Id must be positive or 0 and less than 10000000");
                return id;
            }

            public static string ValidateTitle(string title)
            {
                if (String.IsNullOrWhiteSpace(title))
                    throw new BadModelException("The title can not be null or whitespace");
                if (Char.IsLower(title[0]))
                    throw new BadModelException("The title must start with a capital letter");
                return title;
            }

            public static string ValidateCode(string code)
            {
                if (!Regex.IsMatch(code, _patternCode))
                    throw new BadModelException("Bad code format. Must be: *****/*-**");
                return code;
            }

            public static string ValidateMaterial(string material)
            {
                if (_allowedMaterial.FirstOrDefault(c => c == material) is null)
                    throw new BadModelException("Incorrect material specified");
                return material;
            }

            public static string ValidateType(string type)
            {
                if (_allowedTypes.FirstOrDefault(c => c == type) is null)
                    throw new BadModelException("Incorrect type specified");
                return type;
            }

            public static DateTime ValidateDateOfCreation(DateTime dateOfCreation)
            {
                if (dateOfCreation.Year < 1980 || dateOfCreation > DateTime.Now)
                    throw new BadModelException($"Date of creation must be within 01.01.1980 and {DateTime.Now.ToString("d")}");
                return dateOfCreation;
            }

            public static decimal ValidatePrice(decimal price)
            {
                if (price < 0)
                    throw new BadModelException("The price cannot be negative");
                return price;
            }
        }

        public static class Receipt
        {
            private static string[] _allowedBanks = new string[] { "privatbank", "universal_bank" };
            private static string[] _allowedPaymentTypes = new string[] { "monthly", "yearly" };

            public static int ValidateId(int id)
            {
                if (id < 0 || id > 10000000)
                    throw new BadModelException("Id must be positive or 0 and less than 10000000");
                return id;
            }

            public static string ValidateRecipientName(string name)
            {
                if (String.IsNullOrWhiteSpace(name))
                    throw new BadModelException("The name can not be null or whitespace");
                if (Char.IsLower(name[0]))
                    throw new BadModelException("The name must start with a capital letter");
                return name;
            }

            public static string ValidateBank(string bank)
            {
                if (_allowedBanks.FirstOrDefault(c => c == bank) is null)
                    throw new BadModelException("Incorrect bank specified");
                return bank;
            }

            public static string ValidatePaymentTypes(string type)
            {
                if (_allowedPaymentTypes.FirstOrDefault(c => c == type) is null)
                    throw new BadModelException("Incorrect bank specified");
                return type;
            }

            public static decimal ValidateAmount(decimal amount)
            {
                if (amount < 0)
                    throw new BadModelException("The amount cannot be negative");
                return amount;
            }

            public static DateTime ValidatePaymentDateTime(DateTime paymentDateTime)
            {
                if (paymentDateTime.Year < 1980 || paymentDateTime > DateTime.Now)
                    throw new BadModelException($"Payment datetime must be within 01.01.1980 and {DateTime.Now.ToString("d")}");
                return paymentDateTime;
            }
        }
    }
}