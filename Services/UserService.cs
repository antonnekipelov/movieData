using RecomendMovie.Models;
using System.Data.SQLite;
using System.IO;
using Newtonsoft.Json;
namespace RecomendMovie.Services
{
    public class UserService
    {
        private readonly string _filePath = "users.json";

        public UserService()
        {
            if (!File.Exists(_filePath))
            {
                // Создаем пустой JSON-файл, если его не существует
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(new List<User>()));
            }
        }

        public User Register(string username, string password)
        {
            var users = LoadUsers();
            // Проверка, существует ли пользователь с таким же логином
            if (users.Exists(u => u.Username == username))
                return null; // Пользователь с таким логином уже существует
            var newUser = new User { Username = username, PasswordHash = password };
            users.Add(newUser);
            SaveUsers(users);
            return newUser;
        }

        public User Authenticate(string username, string password)
        {
            var users = LoadUsers();

            return users.Find(u => u.Username == username && u.PasswordHash == password);
        }
        private List<User> LoadUsers()
        {
            if (!File.Exists(_filePath))
            {
                return new List<User>();
            }

            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<User>>(json);
        }
        private void SaveUsers(List<User> users)
        {
            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}