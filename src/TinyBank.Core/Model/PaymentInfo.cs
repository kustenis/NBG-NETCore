﻿namespace TinyBank.Core.Model
{
    public class PaymentInfo
    {
        public string CardNumber { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public decimal Amount { get; set; }
    }
}
