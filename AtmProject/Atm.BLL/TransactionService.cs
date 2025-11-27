using Atm.DAL;
using Atm.Models;

namespace Atm.BLL
{
    public class TransactionService
    {
        private readonly TransactionLogger _logger;

        public TransactionService(TransactionLogger logger)
        {
            _logger = logger;
        }

        public async Task LogTransactionAsync(User user, TransactionType type, decimal amount)
        {
            var log = new Transaction
            {
                UserId = user.Id,
                Type = type,
                Amount = amount,
                NewBalance = user.Balance,
                DateTime = DateTime.Now
            };

            await _logger.LogAsync(log);
        }

        public async Task CheckBalanceAsync(User user)
        {
            await LogTransactionAsync(user, TransactionType.CheckBalance, 0);
        }

        public async Task DepositAsync(User user, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("The amount entered must be positive.");

            user.Balance += amount;

            await LogTransactionAsync(user, TransactionType.Deposit, amount);
        }

        public async Task WithdrawAsync(User user, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("The withdrawal amount must be positive.");

            if (user.Balance < amount)
                throw new ArgumentException("There are not enough funds in the account.");

            user.Balance -= amount;

            await LogTransactionAsync(user, TransactionType.Withdrawal, amount);
        }

        public async Task<List<Transaction>> GetHistoryAsync(int userId)
        {
            return await _logger.LoadUserHistoryAsync(userId);
        }
    }
}