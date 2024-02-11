using Application.DataAccess;
using Application.Domain.Services;
using System;

namespace Application.Features
{
    public class WithdrawMoney
    {
        private readonly IAccountRepository _accountRepository;
        private readonly INotificationService _notificationService;

        public WithdrawMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            _accountRepository = accountRepository;
            _notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, decimal amount)
        {
			if (amount <= 0)
			{
				throw new InvalidOperationException("The amount cannot be zero or negative");
			}

			var from = _accountRepository.GetAccountById(fromAccountId)
				?? throw new InvalidOperationException("Account not found");

			var fromBalance = from.Balance - amount;
			if (fromBalance < 0m)
			{
				throw new InvalidOperationException("Insufficient funds to make withdraw");
			}

			if (fromBalance < 500m)
			{
				_notificationService.NotifyFundsLow(from.User.Email);
			}

			from.Balance -= amount;
			from.Withdrawn -= amount;

			_accountRepository.Update(from);
		}
    }
}
