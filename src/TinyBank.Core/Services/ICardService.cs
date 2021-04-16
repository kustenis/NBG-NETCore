using System;
using TinyBank.Core.Model;

namespace TinyBank.Core.Services
{
    public interface ICardService
    {
        public ApiResult<Card> Create(Card cardInfo);
        public ApiResult<Card> GetById(Guid cardId);
        public ApiResult<Card> GetByNumber(string cardNumber);
        public ApiResult<PaymentInfo> Payment(PaymentInfo paymentInfo);
    }
}
