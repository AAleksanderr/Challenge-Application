using Application.DataAccess.Models;
using Application.Domain.Services;
using Application.Features;
using Moq;
using System;
using Xunit;

namespace Application.Tests
{
    public class TransferMoneyTests
	{
		[Fact]
		public void ExecuteTest()
		{
			// Arrange
			var existingFromAccountId = new Guid("233771e0-7383-4656-8bed-0998ff8de502");
			var existingToAccountId = new Guid("2e751ba4-781c-4866-b4db-d70e9ab4adba");
			var notExistingAccountId = new Guid("789b597d-efbc-404a-b992-be96614f844b");

			var mockAccountRepository = EntityHelper.GetAccountRepository(
				new Guid[] { existingFromAccountId, existingToAccountId });
			var mockNotificationService = new Mock<INotificationService>();
			var transferMoney = new TransferMoney(mockAccountRepository.Object, mockNotificationService.Object);

			var validAmount = 100m;
			var tooBigAmount = 10000000m;
			var moreThenPayInLimit = Account.PayInLimit + 1;
			var zeroAmount = 0m;
			var negativeAmount = -100m;

			InvalidOperationException ex;

			// Act
			Action successfulTransfer = 
				() => transferMoney.Execute(existingFromAccountId, existingToAccountId, validAmount);

			Action notExistingFromAccountTransfer =
				() => transferMoney.Execute(notExistingAccountId, existingToAccountId, validAmount);

			Action notExistingToAccountTransfer =
				() => transferMoney.Execute(existingFromAccountId, notExistingAccountId, validAmount);

			Action negativeAmountTransfer =
				() => transferMoney.Execute(existingFromAccountId, existingToAccountId, negativeAmount);

			Action zeroAmountTransfer =
				() => transferMoney.Execute(existingFromAccountId, existingToAccountId, zeroAmount);

			Action tooBigAmountTransfer =
				() => transferMoney.Execute(existingFromAccountId, existingToAccountId, tooBigAmount);

			Action moreThenPayInLimitTransfer =
				() => transferMoney.Execute(existingFromAccountId, existingToAccountId, moreThenPayInLimit);

			// Assert
			Assert.Null(Record.Exception(successfulTransfer));

			ex = Assert.Throws<InvalidOperationException>(notExistingFromAccountTransfer);
			Assert.Equal("Account not found", ex.Message);

			ex = Assert.Throws<InvalidOperationException>(notExistingToAccountTransfer);
			Assert.Equal("Account not found", ex.Message);

			ex = Assert.Throws<InvalidOperationException>(negativeAmountTransfer);
			Assert.Equal("The amount cannot be zero or negative", ex.Message);

			ex = Assert.Throws<InvalidOperationException>(zeroAmountTransfer);
			Assert.Equal("The amount cannot be zero or negative", ex.Message);

			ex = Assert.Throws<InvalidOperationException>(tooBigAmountTransfer);
			Assert.Equal("Insufficient funds to make withdraw", ex.Message);

			ex = Assert.Throws<InvalidOperationException>(moreThenPayInLimitTransfer);
			Assert.Equal("Account pay in limit reached", ex.Message);
		}
	}
}
