using System.Text.Json;
using Atm.Models;

namespace Atm.DAL
{
    public class UserDataStore
    {
        private const string UserFilePath = "../../../users.json";
        private List<User> _users;

        public async Task<List<User>> LoadUsersAsync()
        {
            _users = await JsonFileManager.LoadAsync<User>(UserFilePath);
            return _users;
        }

        public async Task SaveUsersAsync(List<User> users)
        {
            await JsonFileManager.SaveAsync(UserFilePath, users);
            _users = users;
        }

        public int GetNextId()
        {
            if (_users == null || _users.Count == 0)
            {
                return 1;
            }

            return _users.Max(u => u.Id) + 1;
        }

        public User GetUserByPersonalNumber(string personalNumber)
        {
            if (_users == null) return null;

            return _users.FirstOrDefault(u => u.PersonalNumber == personalNumber);
        }
    }
}