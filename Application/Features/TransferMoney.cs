using Application.Domain;
using Application.DataAccess;
using Application.Domain.Services;
using System;

namespace Application.Features
{
	public class TransferMoney
    {
        private readonly IAccountRepository _accountRepository;
        private readonly INotificationService _notificationService;

        public TransferMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            _accountRepository = accountRepository;
            _notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            if (amount <= 0)
            {
				throw new InvalidOperationException("The amount cannot be zero or negative");
			}

            var from = _accountRepository.GetAccountById(fromAccountId)
				?? throw new InvalidOperationException("Account not found");
            var to = _accountRepository.GetAccountById(toAccountId)
				?? throw new InvalidOperationException("Account not found");

            var fromBalance = from.Balance - amount;
            if (fromBalance < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            if (fromBalance < 500m)
            {
                _notificationService.NotifyFundsLow(from.User.Email);
            }

            var paidIn = to.PaidIn + amount;
            if (paidIn > Account.PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            if (Account.PayInLimit - paidIn < 500m)
            {
                _notificationService.NotifyApproachingPayInLimit(to.User.Email);
            }

            from.Balance -= amount;
            from.Withdrawn -= amount;

            to.Balance += amount;
            to.PaidIn += amount;

            // TODO: Transaction required
			_accountRepository.Update(from);
            _accountRepository.Update(to);
        }
    }
}
