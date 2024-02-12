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
            // TODO: Transaction required
			new WithdrawMoney(_accountRepository, _notificationService).Execute(fromAccountId, amount);
            new DepositMoney(_accountRepository, _notificationService).Execute(toAccountId, amount);
        }
    }
}
