using Microsoft.AspNetCore.Mvc;
using Stripe;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    [HttpPost("create-payment-intent")]
    public ActionResult CreatePaymentIntent()
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = 1000, // $10 
            Currency = "usd",
            PaymentMethodTypes = new List<string> { "card" }
        };

        var service = new PaymentIntentService();
        var intent = service.Create(options);

        return Ok(new { clientSecret = intent.ClientSecret });
    }
}
