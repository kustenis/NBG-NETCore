using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Services;
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
            return Ok(payment);
        }
    }
}
