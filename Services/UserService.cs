using RecomendMovie.Models;
using System.IO;
using Newtonsoft.Json;
using System.Windows;

namespace RecomendMovie.Services
{
    public class UserService
    {
        private const string _filePath = "users.json";
        private User _currentUser;

        public UserService()
        {
            try
            {
                if (!File.Exists(_filePath))
                    File.WriteAllText(_filePath, JsonConvert.SerializeObject(new List<User>()));
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error initializing user file: {ex.Message}");
                throw; 
            }
        }

        public User Register(string username, string password)
        {
            try
            {
                var _users = LoadUsers();

                if (_users.Exists(u => u.Username == username))
                {
                    MessageBox.Show("User already exists.");
                    return null;
                }

                var user = new User { Username = username, PasswordHash = password };
                _users.Add(user);
                _currentUser = user;

                SaveUsers(_users);
                return user;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during registration: {ex.Message}");
                return null;
            }
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }

        public User Authenticate(string username, string password)
        {
            try
            {
                var _users = LoadUsers();
                _currentUser = _users.Find(u => u.Username == username && u.PasswordHash == password);

                if (_currentUser == null)
                    MessageBox.Show("Authentication failed: Invalid username or password.");
                return _currentUser;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during authentication: {ex.Message}");
                return null;
            }
        }

        private List<User> LoadUsers()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<User>();
                var json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error reading user file: {ex.Message}");
                return new List<User>(); // Возвращаем пустой список в случае ошибки
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing user data: {ex.Message}");
                return new List<User>(); // Возвращаем пустой список в случае ошибки
            }
        }
        private void SaveUsers(List<User> _users)
        {
            try
            {
                var json = JsonConvert.SerializeObject(_users, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error saving user data: {ex.Message}");
            }
        }
    }
}
