using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Web.Extensions;
using TinyBank.Web.Models;

namespace TinyBank.Web.Controllers
{
    [Route("card")]
    public class CardController : Controller
    {
        private readonly ICardService _cardService;
        private readonly ILogger<HomeController> _logger;
        private readonly TinyBankDbContext _dbContext;

        // Path: '/customer'
        public CardController(
            TinyBankDbContext dbContext,
            ILogger<HomeController> logger,
            ICardService cardService)
        {
            _logger = logger;
            _cardService = cardService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View(new CardPayment());
        }

        [HttpPost("checkout")]
        public IActionResult Checkout([FromBody] CardPayment payment)
        {
            var paymentInfo = new PaymentInfo {
                CardNumber = payment?.CardNumber,
                ExpirationMonth = payment?.ExpirationMonth ?? 0,
                ExpirationYear = payment?.ExpirationYear ?? 0,
                Amount = payment?.Amount ?? 0
            };
            var result = _cardService.Payment(paymentInfo);

            if (!result.IsSuccessful()) {
                return result.ToActionResult();
            }

            return Ok();
        }
    }
}
