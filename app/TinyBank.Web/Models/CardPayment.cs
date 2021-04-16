using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TinyBank.Web.Models
{
    public class CardPayment
    {
        public string CardNumber { get; set; }
        public byte ExpirationMonth { get; set; }
        public short ExpirationYear { get; set; }
        public decimal Amount { get; set; }
    }
}
