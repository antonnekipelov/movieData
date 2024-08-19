using RecomendMovie.Models;
using RecomendMovie.Services;
using System.Data.SQLite;

public class UserService : IUserService
{
    private readonly string _connectionString = "Data Source=users.db;Version=3;";

    public UserService()
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Users (Username TEXT PRIMARY KEY, Password TEXT)", connection);
            command.ExecuteNonQuery();
        }
    }

    public User Register(string username, string password)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var command = new SQLiteCommand("INSERT INTO Users (Username, Password) VALUES (@username, @password)", connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.ExecuteNonQuery();
            connection.Close();
        }
        return new User { Username = username, PasswordHash = password };
    }

    public User Authenticate(string username, string password)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var command = new SQLiteCommand("SELECT Username, Password FROM Users WHERE Username = @username AND Password = @password", connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new User { Username = reader["Username"].ToString(), PasswordHash = reader["Password"].ToString() };
                }
            }
            connection.Close();
        }
        return null;
    }
}