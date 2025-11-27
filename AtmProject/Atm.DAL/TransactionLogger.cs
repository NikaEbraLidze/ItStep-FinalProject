using System.Text.Json;
using Atm.Models;

namespace Atm.DAL
{
    public class TransactionLogger
    {
        private const string LogFilePath = "../../../transactions.json";

        public async Task LogAsync(Transaction transaction)
        {
            var logs = await JsonFileManager.LoadAsync<Transaction>(LogFilePath);

            logs.Add(transaction);

            await JsonFileManager.SaveAsync(LogFilePath, logs);
        }

        public async Task<List<Transaction>> LoadUserHistoryAsync(int userId)
        {
            var logs = await JsonFileManager.LoadAsync<Transaction>(LogFilePath);

            return logs.Where(t => t.UserId == userId)
                        .OrderBy(t => t.DateTime)
                        .ToList();
        }
    }
}