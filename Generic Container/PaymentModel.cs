namespace Generic_Container
{
    using System;
    using static Validation.Payment;

    class PaymentModel
    {
        private int _id;
        private decimal _amount;
        private string _currency;
        private string _payerEmail;
        private DateTime _requestDate;
        private DateTime _dueToDate;
        private string _transactionId;

        public int Id               { get => _id;               set { _id = ValidateId(value);                                  }}
        public decimal Amount       { get => _amount;           set { _amount = ValidateAmount(value);                          }}
        public string Currency      { get => _currency;         set { _currency = ValidateCurrency(value);                      }}
        public string PayerEmail    { get => _payerEmail;       set { _payerEmail = ValidateEmail(value);                       }}
        public DateTime RequestDate { get => _requestDate;      set { _requestDate = ValidateRequestDate(value, _dueToDate);    }}
        public DateTime DueToDate   { get => _dueToDate;        set { _dueToDate = ValidateDueToDate(value, _requestDate);      }}
        public string TransactionId { get => _transactionId;    set { _transactionId = ValidateTransactionId(value);            }}

        public override string ToString()
        {
            var properties = Services<PaymentModel>.GetPropertiesClassService();

            string payments = String.Empty;

            foreach (var property in properties)
                payments += $"{property.Name}: {property.GetValue(this)}\n";

            return payments;
        }
    }
}