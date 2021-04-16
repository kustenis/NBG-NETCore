using System;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Implementation.Common;
using System.Linq;
using TinyBank.Core.Services.Options;
using Microsoft.EntityFrameworkCore;

namespace TinyBank.Core.Implementation.Services
{
    public class CardService : ICardService
    {
        private readonly ICardService _cardService;
        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;
        private readonly Data.TinyBankDbContext _dbContext;
        

        public CardService(
            ICardService cardService,
            IAccountService accountService,
            ICustomerService customerService,
            Data.TinyBankDbContext dbContext)
        {
            _cardService = cardService;
            _accountService = accountService;
            _customerService = customerService;
            _dbContext = dbContext;
        } 

        public ApiResult<Card> Create(Card cardInfo)
        {

            var validationResult = ValidateCardInfo(cardInfo);
            if (validationResult != null) return validationResult;

            cardInfo.CardId = Guid.NewGuid();
            _dbContext.Add(cardInfo);

            try {
                _dbContext.SaveChanges();
            } catch (Exception) {
                return ApiResult<Card>.CreateFailed(Constants.ApiResultCode.InternalServerError, "Could not save card");
            }

            return ApiResult<Card>.CreateSuccessful(cardInfo);
        }

        public ApiResult<Card> GetById(Guid customerId)
        {
            var card = Search(
                new SearchCardOptions() {
                    CardId = customerId
                })
                .Include(c => c.Accounts)
                .SingleOrDefault();

            if (card == null) {
                return new ApiResult<Card>() {
                    Code = Constants.ApiResultCode.NotFound,
                    ErrorText = $"Card {customerId} was not found"
                };
            }

            return new ApiResult<Card>() {
                Data = card
            };
        }

        public ApiResult<Card> GetByNumber(string cardNumber)
        {
            var card = Search(
                new SearchCardOptions() {
                    CardNumber = cardNumber
                })
                .Include(c => c.Accounts)
                .SingleOrDefault();

            if (card == null) {
                return new ApiResult<Card>() {
                    Code = Constants.ApiResultCode.NotFound,
                    ErrorText = $"Card {cardNumber} was not found"
                };
            }

            return new ApiResult<Card>() {
                Data = card
            };
        }

        public ApiResult<PaymentInfo> Payment(PaymentInfo paymentInfo)
        {
            var validationResult = ValidatePaymentInfo(paymentInfo);
            if (validationResult != null) return validationResult;

            var cardResult = _cardService.GetByNumber(paymentInfo.CardNumber);
            if (cardResult == null) {
                return ApiResult<PaymentInfo>.CreateFailed(Constants.ApiResultCode.BadRequest, "Card not found");
            }

            var validationPaymetResult = ValidatePayment(paymentInfo, cardResult.Data);
            if (validationPaymetResult != null) return validationPaymetResult;

            //cardInfo.CardId = Guid.NewGuid();
            //_dbContext.Add(cardInfo);

            //try {
            //    _dbContext.SaveChanges();
            //}
            //catch (Exception) {
            //    return ApiResult<Card>.CreateFailed(Constants.ApiResultCode.InternalServerError, "Could not save card");
            //}

            //return ApiResult<Card>.CreateSuccessful(cardInfo);

            return null;
        }


        private ApiResult<Card> ValidateCardInfo(Card cardInfo)
        {
            if (cardInfo == null) {
                return ApiResult<Card>.CreateFailed(Constants.ApiResultCode.BadRequest, $"Null {nameof(cardInfo)}");
            }

            cardInfo.CardNumber = cardInfo.CardNumber?.Trim();
            if (!ValidateCardNumber(cardInfo.CardNumber)) {
                return ApiResult<Card>.CreateFailed(Constants.ApiResultCode.BadRequest, $"Invalid {nameof(cardInfo.CardNumber)}");
            }

            return null;
        }

        private ApiResult<PaymentInfo> ValidatePaymentInfo(PaymentInfo paymentInfo)
        {
            if (paymentInfo == null) {
                return ApiResult<PaymentInfo>.CreateFailed(Constants.ApiResultCode.BadRequest, $"Null {nameof(paymentInfo)}");
            }

            paymentInfo.CardNumber = paymentInfo.CardNumber?.Trim();
            if (!ValidateCardNumber(paymentInfo.CardNumber)) {
                return ApiResult<PaymentInfo>.CreateFailed(Constants.ApiResultCode.BadRequest, $"Invalid {nameof(paymentInfo.CardNumber)}");
            }

            var isExpirationMonthValid = paymentInfo.ExpirationMonth >= 1 && paymentInfo.ExpirationMonth <= 12;
            if (!isExpirationMonthValid) {
                return ApiResult<PaymentInfo>.CreateFailed(Constants.ApiResultCode.BadRequest, $"Invalid {nameof(paymentInfo.ExpirationMonth)}");
            }

            var isExpirationYearValid = paymentInfo.ExpirationYear >= 1900 && paymentInfo.ExpirationYear <= 9999;
            if (!isExpirationYearValid) {
                return ApiResult<PaymentInfo>.CreateFailed(Constants.ApiResultCode.BadRequest, $"Invalid {nameof(paymentInfo.ExpirationYear)}");
            }

            var isCardExpired = DateTime.Today >= new DateTime(year: paymentInfo.ExpirationYear, month: paymentInfo.ExpirationMonth, 1);
            if (isCardExpired) {
                return ApiResult<PaymentInfo>.CreateFailed(Constants.ApiResultCode.BadRequest, "Expired Card");
            }

            var isAmountInvalid = (paymentInfo.Amount > 0);
            if (isAmountInvalid) {
                return ApiResult<PaymentInfo>.CreateFailed(Constants.ApiResultCode.BadRequest, "Invalid Amount");
            }

            return null;
        }

        private ApiResult<PaymentInfo> ValidatePayment(PaymentInfo paymentInfo, Card card)
        {
            if (paymentInfo == null) {
                return ApiResult<PaymentInfo>.CreateFailed(Constants.ApiResultCode.InternalServerError, $"Null {nameof(paymentInfo)}");
            }

            if (card == null) {
                return ApiResult<PaymentInfo>.CreateFailed(Constants.ApiResultCode.InternalServerError, $"Null {nameof(card)}");
            }

            if (!(card.Accounts?.Count > 0)) {
                return ApiResult<PaymentInfo>.CreateFailed(Constants.ApiResultCode.InternalServerError, $"Null {nameof(card.Accounts)}");
            }




            paymentInfo.CardNumber = paymentInfo.CardNumber?.Trim();
            if (!ValidateCardNumber(paymentInfo.CardNumber)) {
                return ApiResult<PaymentInfo>.CreateFailed(Constants.ApiResultCode.BadRequest, $"Invalid {nameof(paymentInfo.CardNumber)}");
            }

            var isExpirationMonthValid = paymentInfo.ExpirationMonth >= 1 && paymentInfo.ExpirationMonth <= 12;
            if (!isExpirationMonthValid) {
                return ApiResult<PaymentInfo>.CreateFailed(Constants.ApiResultCode.BadRequest, $"Invalid {nameof(paymentInfo.ExpirationMonth)}");
            }

            var isExpirationYearValid = paymentInfo.ExpirationYear >= 1900 && paymentInfo.ExpirationYear <= 9999;
            if (!isExpirationYearValid) {
                return ApiResult<PaymentInfo>.CreateFailed(Constants.ApiResultCode.BadRequest, $"Invalid {nameof(paymentInfo.ExpirationYear)}");
            }

            var isCardExpired = DateTime.Today >= new DateTime(year: paymentInfo.ExpirationYear, month: paymentInfo.ExpirationMonth, 1);
            if (isCardExpired) {
                return ApiResult<PaymentInfo>.CreateFailed(Constants.ApiResultCode.BadRequest, "Expired Card");
            }
            return null;
        }

        private bool ValidateCardNumber(string cardNumber)
        {
            var isCardNumberValid = cardNumber?.Length == 16;
            if (!isCardNumberValid) return false;

            return true;
        }

        private IQueryable<Card> Search(SearchCardOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            var q = _dbContext.Set<Card>().AsQueryable();

            if (options.CardId != null) {
                q = q.Where(c => c.CardId == options.CardId);
            }

            if (options.CardNumber != null) {
                q = q.Where(c => c.CardNumber == options.CardNumber);
            }

            if (options.TrackResults != null &&
              !options.TrackResults.Value) {
                q = q.AsNoTracking();
            }

            if (options.Skip != null) {
                q = q.Skip(options.Skip.Value);
            }

            q = q.Take(options.MaxResults ?? 500);

            return q;
        }

    }
}
