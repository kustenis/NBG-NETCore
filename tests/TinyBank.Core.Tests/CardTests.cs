using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

using TinyBank.Core.Model;
using TinyBank.Core.Implementation.Data;

using Xunit;

namespace TinyBank.Core.Tests
{
    public class CardTests : IClassFixture<TinyBankFixture>
    {
        private readonly TinyBankDbContext _dbContext;
        private readonly CustomerServiceTests _customerTests;

        public CardTests(TinyBankFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _customerTests = new CustomerServiceTests(fixture);
        }

        [Fact]
        public void Card_Register_Success()
        {
            var customer = _customerTests.RegisterCustomer_Success(
                Constants.Country.GreekCountryCode);

            var account = new Account() {
                Balance = 1000M,
                CurrencyCode = "EUR",
                State = Constants.AccountState.Active,
                AccountId = $"GR{DateTimeOffset.Now:ssfffffff}"
            };

            customer.Accounts.Add(account);
            _dbContext.SaveChanges();

            var card = new Card() {
                Active = true,
                CardNumber = $"4111111111111{DateTimeOffset.Now:fff}",
                CardType = Constants.CardType.Debit
            };

            account.Cards.Add(card);

            _dbContext.Add(card);
            _dbContext.SaveChanges();

            var customerFromDb = _dbContext.Set<Customer>()
                .Where(c => c.VatNumber == customer.VatNumber)
                .Include(c => c.Accounts)
                .ThenInclude(a => a.Cards)
                .SingleOrDefault();

            var customerCard = customerFromDb.Accounts
                .SelectMany(a => a.Cards)
                .Where(c => c.CardNumber == card.CardNumber)
                .SingleOrDefault();

            Assert.NotNull(customerCard);
            Assert.Equal(Constants.CardType.Debit, customerCard.CardType);
            Assert.True(customerCard.Active);
        }
    }
}
