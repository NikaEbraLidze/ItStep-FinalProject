using Atm.BLL;
using Atm.Models;

namespace Atm.App
{
    public class MenuHandler
    {
        private readonly AccountService _accountService;

        public MenuHandler(AccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task MainMenuAsync()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== ATM main menu ===");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Registration");
                Console.WriteLine("3. Exit");
                Console.Write("Choose operation (1-3): ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await HandleLoginAsync();
                            break;
                        case "2":
                            await HandleRegistrationAsync();
                            break;
                        case "3":
                            running = false;
                            Console.WriteLine("Thank you for your interest. Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid Input");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private async Task HandleRegistrationAsync()
        {
            Console.Clear();
            Console.WriteLine("=== New user registration ===");

            string firstName;
            string lastName;
            string personalNumber;

            while (true)
            {
                Console.Write("First name: ");
                firstName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(firstName))
                {
                    Console.WriteLine("First name cannot be empty. Try again.\n");
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.Write("Last name: ");
                lastName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(lastName))
                {
                    Console.WriteLine("Last name cannot be empty. Try again.\n");
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.Write("Personal number (11 digits): ");
                personalNumber = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(personalNumber))
                {
                    Console.WriteLine("Personal number cannot be empty. Try again.\n");
                    continue;
                }

                if (personalNumber.Length != 11)
                {
                    Console.WriteLine("Personal number must be exactly 11 digits. Try again.\n");
                    continue;
                }

                if (!personalNumber.All(char.IsDigit))
                {
                    Console.WriteLine("Personal number can contain digits only. Try again.\n");
                    continue;
                }

                break;
            }

            var newUser = await _accountService.RegisterUserAsync(firstName, lastName, personalNumber);

            Console.WriteLine("\nRegistration completed successfully!");
            Console.WriteLine($"Your password: {newUser.Password}");
            Console.WriteLine("Press any key to log in...");
            Console.ReadKey();
        }



        private async Task HandleLoginAsync()
        {
            Console.Clear();
            Console.WriteLine("=== LogIn ===");

            Console.Write("Personal number: ");
            string personalNumber = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            User loggedInUser = _accountService.Login(personalNumber, password);

            if (loggedInUser != null)
            {
                Console.WriteLine("\n Successful log in!");
                Console.ReadKey();

                await AtmMenuAsync(loggedInUser);
            }
            else
            {
                Console.WriteLine("\nInvalid creditials!");
                Console.ReadKey();
            }
        }

        private async Task AtmMenuAsync(User user)
        {
            bool loggedIn = true;
            while (loggedIn)
            {
                Console.Clear();
                Console.WriteLine($"=== Hello, {user.FirstName} {user.LastName} ===");
                Console.WriteLine("1. check balance");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdrawal");
                Console.WriteLine("4. Operation history");
                Console.WriteLine("5. Logout");
                Console.Write("Choose operation(1-5): ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await HandleCheckBalanceAsync(user);
                            break;
                        case "2":
                            await HandleFinancialOperationAsync(user, TransactionType.Deposit);
                            break;
                        case "3":
                            await HandleFinancialOperationAsync(user, TransactionType.Withdrawal);
                            break;
                        case "4":
                            await HandleViewHistoryAsync(user);
                            break;
                        case "5":
                            loggedIn = false;
                            Console.WriteLine("You are logged out.");
                            Console.ReadKey();
                            break;
                        default:
                            Console.WriteLine("Invalid Input.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] {ex.Message}");
                    Console.WriteLine("Press any key...");
                    Console.ReadKey();
                }
            }
        }

        private async Task HandleCheckBalanceAsync(User user)
        {
            await _accountService.PerformFinancialOperationAsync(user, TransactionType.CheckBalance, 0);

            Console.Clear();
            Console.WriteLine("=== Check balance ===");
            Console.WriteLine($"Your balance: {user.Balance} GEL");

            Console.WriteLine($"> User name: {user.FirstName} {user.LastName} - Check the balance at : {DateTime.Now:dd.MM.yyyy} ");
            Console.ReadKey();
        }

        private async Task HandleFinancialOperationAsync(User user, TransactionType type)
        {
            string operationName = type == TransactionType.Deposit ? "Deposit" : "Withdrawal";

            Console.Clear();
            Console.WriteLine($"=== Amount {operationName} ===");
            Console.Write($"Enter amount: ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                throw new ArgumentException("The amount entered is invalid or negative.");
            }

            await _accountService.PerformFinancialOperationAsync(user, type, amount);

            Console.WriteLine($"\nThe operation was successful. New balance: {user.Balance} GEL");

            if (type == TransactionType.Deposit)
            {
                Console.WriteLine($"> User name: {user.FirstName} {user.LastName} - topped up the balance {amount} GEL at : {DateTime.Now:dd.MM.yyyy}. Its current balance is {user.Balance} GEL");
            }
            else
            {
                Console.WriteLine($"> User name: {user.FirstName} {user.LastName} - withdrawal {amount} GEL at : {DateTime.Now:dd.MM.yyyy}. Its current balance is {user.Balance} GEL");
            }

            Console.ReadKey();
        }

        private async Task HandleViewHistoryAsync(User user)
        {
            Console.Clear();
            Console.WriteLine("=== Operation history ===");

            var history = await _accountService.GetUserHistoryAsync(user.Id);

            if (history.Count == 0)
            {
                Console.WriteLine("History not found.");
                Console.ReadKey();
                return;
            }

            foreach (var t in history.OrderByDescending(t => t.DateTime))
            {
                string action = "";
                string amountInfo = t.Amount > 0 ? $" ({t.Amount})" : "";

                switch (t.Type)
                {
                    case TransactionType.Deposit:
                        action = "Deposit";
                        break;
                    case TransactionType.Withdrawal:
                        action = "Withdrawal";
                        break;
                    case TransactionType.CheckBalance:
                        action = "Check balance";
                        amountInfo = "";
                        break;
                    case TransactionType.Registration:
                        action = "Registered";
                        amountInfo = "";
                        break;
                }

                Console.WriteLine($"[{t.DateTime:yyyy-MM-dd HH:mm:ss}] - {action}{amountInfo}. New balance: {t.NewBalance} GEL.");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}