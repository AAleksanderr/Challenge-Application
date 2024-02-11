using Application.DataAccess;
using Application.Domain;
using Moq;
using System;

namespace Application.Tests
{
	public static class EntityHelper
	{
		public static Mock<IAccountRepository> GetAccountRepository(Guid[] accountIds)
		{
			var mockAccountRepository = new Mock<IAccountRepository>();
			foreach (var accountId in accountIds)
			{ 
				mockAccountRepository.Setup(x => x.GetAccountById(accountId))
					.Returns(new Account
					{
						Id = accountId,
						Balance = 20000m,
						PaidIn = 0m,
						Withdrawn = 0m,
						User = new User
						{
							Id = Guid.NewGuid(),
							Email = "test@test.test",
							Name = "test"
						}
					});
			}
			return mockAccountRepository;
		}
	}
}
