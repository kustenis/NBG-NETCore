using System;

namespace TinyBank.Core.Services.Options
{
    public class CreateCardOptions
    {
        public Guid AccountId { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
    }
}
