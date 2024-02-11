using Application.Domain.Services;
using Application.Features;
using Moq;
using System;
using Xunit;

namespace Application.Tests
{
	public class WithdrawMoneyTests
	{
		[Fact]
		public void ExecuteTest()
		{
			// Arrange
			var existingFromAccountId = new Guid("233771e0-7383-4656-8bed-0998ff8de502");
			var notExistingAccountId = new Guid("789b597d-efbc-404a-b992-be96614f844b");

			var mockAccountRepository = EntityHelper.GetAccountRepository(new Guid[] { existingFromAccountId });
			var mockNotificationService = new Mock<INotificationService>();
			var withdrawMoney = new WithdrawMoney(mockAccountRepository.Object, mockNotificationService.Object);

			var validAmount = 100m;
			var tooBigAmount = 10000000m;
			var zeroAmount = 0m;
			var negativeAmount = -100m;

			InvalidOperationException ex;

			// Act
			Action successfulTransfer =
				() => withdrawMoney.Execute(existingFromAccountId, validAmount);

			Action notExistingFromAccountTransfer =
				() => withdrawMoney.Execute(notExistingAccountId, validAmount);

			Action negativeAmountTransfer =
				() => withdrawMoney.Execute(existingFromAccountId, negativeAmount);

			Action zeroAmountTransfer =
				() => withdrawMoney.Execute(existingFromAccountId, zeroAmount);

			Action tooBigAmountTransfer =
				() => withdrawMoney.Execute(existingFromAccountId, tooBigAmount);

			// Assert
			Assert.Null(Record.Exception(successfulTransfer));

			ex = Assert.Throws<InvalidOperationException>(notExistingFromAccountTransfer);
			Assert.Equal("Account not found", ex.Message);

			ex = Assert.Throws<InvalidOperationException>(negativeAmountTransfer);
			Assert.Equal("The amount cannot be zero or negative", ex.Message);

			ex = Assert.Throws<InvalidOperationException>(zeroAmountTransfer);
			Assert.Equal("The amount cannot be zero or negative", ex.Message);

			ex = Assert.Throws<InvalidOperationException>(tooBigAmountTransfer);
			Assert.Equal("Insufficient funds to make withdraw", ex.Message);
		}
	}
}
