using RecomendMovie.Models;
using System.IO;
using Newtonsoft.Json;
namespace RecomendMovie.Services
{
    public class UserService
    {
        private readonly string _filePath = "users.json";
        private User _currentUser;

        public UserService()
        {
            if (!File.Exists(_filePath))
                // Создаем пустой JSON-файл, если его не существует
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(new List<User>()));
        }
        public User Register(string username, string password)
        {
            var _users = LoadUsers();
            if (_users.Exists(u => u.Username == username))
                return null; // Пользователь с таким логином уже существует
            var user = new User { Username = username, PasswordHash = password };
            _users.Add(user);
            _currentUser = user;
            SaveUsers(_users);
            return user;
        }
        public User GetCurrentUser()
        {
            return _currentUser;
        }

        public User Authenticate(string username, string password)
        {
            var _users = LoadUsers();
            _currentUser = _users.Find(u => u.Username == username && u.PasswordHash == password);
            return _users.Find(u => u.Username == username && u.PasswordHash == password); ;
        }
        private List<User> LoadUsers()
        {
            if (!File.Exists(_filePath))
                return new List<User>();
            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<User>>(json);
        }
        private void SaveUsers(List<User> _users)
        {
            var json = JsonConvert.SerializeObject(_users, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}