using Application.DataAccess;
using Application.Domain.Services;
using System;
using Application.DataAccess.Models;

namespace Application.Features
{
    public class DepositMoney
	{
		private readonly IAccountRepository _accountRepository;
		private readonly INotificationService _notificationService;

		public DepositMoney(IAccountRepository accountRepository, INotificationService notificationService)
		{
			_accountRepository = accountRepository;
			_notificationService = notificationService;
		}

		public void Execute(Guid toAccountId, decimal amount)
		{
			if (amount <= 0)
			{
				throw new InvalidOperationException("The amount cannot be zero or negative");
			}

			var to = _accountRepository.GetAccountById(toAccountId)
				?? throw new InvalidOperationException("Account not found");

			var paidIn = to.PaidIn + amount;
			if (paidIn > Account.PayInLimit)
			{
				throw new InvalidOperationException("Account pay in limit reached");
			}

			if (Account.PayInLimit - paidIn < 500m)
			{
				_notificationService.NotifyApproachingPayInLimit(to.User.Email);
			}

			to.Balance += amount;
			to.PaidIn += amount;

			_accountRepository.Update(to);
		}
	}
}
