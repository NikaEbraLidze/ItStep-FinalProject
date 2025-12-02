using Atm.DAL;
using Atm.Models;

namespace Atm.BLL
{
    public class AccountService
    {
        private readonly UserDataStore _dataStore;
        private readonly TransactionService _transactionService;

        public AccountService(UserDataStore userDataStore, TransactionService transactionService)
        {
            _dataStore = userDataStore;
            _transactionService = transactionService;
        }

        public async Task InitializeAsync()
        {
            await _dataStore.LoadUsersAsync();
        }

        public async Task<User> RegisterUserAsync(string firstName, string lastName, string personalNumber)
        {
            if (_dataStore.GetUserByPersonalNumber(personalNumber) != null)
                throw new ArgumentException($"The user is already registered with a personal number{personalNumber}.");

            var newUser = new User
            {
                Id = _dataStore.GetNextId(),
                FirstName = firstName,
                LastName = lastName,
                PersonalNumber = personalNumber,
                Password = PasswordGenerator.Generate(),
                Balance = 0m,
            };

            var users = await _dataStore.LoadUsersAsync();
            users.Add(newUser);
            await _dataStore.SaveUsersAsync(users);

            return newUser;
        }

        public User Login(string personalNumber, string password)
        {
            User user = _dataStore.GetUserByPersonalNumber(personalNumber);

            if (user == null || user.Password != password) return null;

            return user;
        }

        public async Task PerformFinancialOperationAsync(User user, TransactionType type, decimal amount)
        {
            switch (type)
            {
                case TransactionType.CheckBalance:
                    await _transactionService.CheckBalanceAsync(user);
                    return;
                case TransactionType.Deposit:
                    await _transactionService.DepositAsync(user, amount);
                    break;
                case TransactionType.Withdrawal:
                    await _transactionService.WithdrawAsync(user, amount);
                    break;
                default:
                    throw new ArgumentException("invalid operation type");
            }

            var users = await _dataStore.LoadUsersAsync();

            User userToUpdate = users.FirstOrDefault(u => u.Id == user.Id);
            if (userToUpdate != null)
            {
                userToUpdate.Balance = user.Balance;
                await _dataStore.SaveUsersAsync(users);
            }
        }

        public async Task<List<Transaction>> GetUserHistoryAsync(int userId)
        {
            return await _transactionService.GetHistoryAsync(userId);
        }
    }
}