namespace Generic_Container
{
    using System;
    using static Validation.Receipt;

    class ReceiptModel
    {
        private int _id;
        private string _recipientName;
        private string _bank;
        private string _paymentType;
        private decimal _amount;
        private DateTime _paymentDatetime;

        public int Id                   { get => _id;               set { _id = ValidateId(value);                              }}
        public string RecipientName     { get => _recipientName;    set { _recipientName = ValidateRecipientName(value);        }}
        public string Bank              { get => _bank;             set { _bank = ValidateBank(value);                          }}
        public string PaymentType       { get => _paymentType;      set { _paymentType = ValidatePaymentTypes(value);           }}
        public decimal Amount           { get => _amount;           set { _amount = ValidateAmount(value);                      }}
        public DateTime PaymentDateTime { get => _paymentDatetime;  set { _paymentDatetime = ValidatePaymentDateTime(value);    }}

        public override string ToString()
        {
            var properties = Services<ReceiptModel>.GetPropertiesClassService();

            string receipt = String.Empty;

            foreach (var property in properties)
                receipt += $"{property.Name}: {property.GetValue(this)}\n";

            return receipt;
        }
    }
}