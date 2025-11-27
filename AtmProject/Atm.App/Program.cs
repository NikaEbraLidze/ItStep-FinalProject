using Atm.BLL;
using Atm.DAL;

namespace Atm.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var userDataStore = new UserDataStore();
            var transactionLogger = new TransactionLogger();

            var transactionService = new TransactionService(transactionLogger);
            var accountService = new AccountService(userDataStore, transactionService);

            var menuHandler = new MenuHandler(accountService);

            await accountService.InitializeAsync();

            await menuHandler.MainMenuAsync();
        }
    }
}