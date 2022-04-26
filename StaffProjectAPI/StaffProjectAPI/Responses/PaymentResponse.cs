using StaffProjectAPI.Data;

namespace StaffProjectAPI.Responses
{
    public class PaymentResponse
    {
        public bool Succeeded { get; set; }
        public List<Error> Errors { get; set; }
        public Payment Payment { get; set; }

        public static implicit operator PaymentResponse(bool succeeded)
            => new PaymentResponse { Succeeded = succeeded };

        public static implicit operator PaymentResponse(List<Error> errors)
            => new PaymentResponse { Errors = errors };

        public static implicit operator PaymentResponse(Payment payment)
            => new PaymentResponse { Payment = payment };
    }
}
