using Microsoft.AspNetCore.Mvc;
using StaffProjectAPI.Data;
using StaffProjectAPI.Responses;
using StaffProjectAPI.Services;

namespace StaffProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public List<Payment> GetAllPayments()
        {
            return _paymentService.GetAllPayments();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Payment> GetPaymentById(int id)
        {
            Payment payment = _paymentService.GetPaymentById(id).Value;

            if (payment is null)
                return NotFound();

            return _paymentService.GetPaymentById(id);
        }

        [HttpPost]
        public ActionResult<PaymentResponse> Create(Payment payment)
        {
            var result = _paymentService
                .AddPayment(payment.Amount, payment.Currency, payment.PayerEmail, payment.DueToDate, payment.TransactionId);

            if (result.Succeeded)
                return Created("", result.Payment);
            else
                return BadRequest(result.Errors);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<PaymentResponse> Update(int id, Payment payment)
        {
            var result = _paymentService
                .UpdatePayment(id, payment.Amount, payment.Currency, payment.PayerEmail, payment.DueToDate, payment.TransactionId);

            if (result.Succeeded)
                return Ok(result.Payment);
            else if (result.Payment is null)
                return NotFound();
            else
                return BadRequest(result.Errors);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult<PaymentResponse> Delete(int id)
        {
            var result = _paymentService.DeletePayment(id);

            if (result.Succeeded == false)
                return NotFound();
            else
                return Ok(result.Payment);
        }
    }
}
