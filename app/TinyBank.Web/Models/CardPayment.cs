using System.Runtime.Serialization;


namespace TinyBank.Web.Models
{
    [DataContract]
    public class CardPayment
    {
        [DataMember(Name = "cardNumber")]
        public string CardNumber { get; set; }

        [DataMember(Name = "expirationMonth")]
        public int? ExpirationMonth { get; set; }

        [DataMember(Name = "expirationYear")]
        public int? ExpirationYear { get; set; }

        [DataMember(Name = "amount")]
        public decimal? Amount { get; set; }
    }
}
