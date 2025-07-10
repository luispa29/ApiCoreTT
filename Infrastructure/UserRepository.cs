using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Model;
using Model.Request;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly IOptions<AppSettings> _options;

        public UserRepository(IOptions<AppSettings> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<bool> CreateUser(UserRequest user)
        {
            try
            {
                string? stringConnection = _options.Value?.DefaultConnection;
                if (string.IsNullOrEmpty(stringConnection))
                {
                    throw new InvalidOperationException("La cadena de conexión no está configurada.");
                }

                using var connection = new SqlConnection(stringConnection);
                await connection.OpenAsync();

                using var command = new SqlCommand("INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password)", connection);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                await connection.CloseAsync();

                if (rowsAffected > 0) return true;

                throw new Exception("No se creó el usuario.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Login(LoginRequest user)
        {
            string? stringConnection = _options.Value?.DefaultConnection;

            if (string.IsNullOrEmpty(stringConnection))
            {
                throw new InvalidOperationException("La cadena de conexión no está configurada.");
            }

            using var connection = new SqlConnection(stringConnection);
            await connection.OpenAsync();

            using var command = new SqlCommand("SELECT Password FROM Users WHERE Email = @Email", connection);
            command.Parameters.AddWithValue("@Email", user.Email);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                string? password = reader["Password"]?.ToString();
                return password == null ? throw new InvalidOperationException("La contraseña no puede ser nula.") : password == user.Password;
            }
            return false;
        }

        public async Task<bool> ValidEmailUser(UserRequest user)
        {
            string? stringConnection = _options.Value?.DefaultConnection;
            if (string.IsNullOrEmpty(stringConnection))
            {
                throw new InvalidOperationException("La cadena de conexión no está configurada.");
            }

            using var connection = new SqlConnection(stringConnection);
            await connection.OpenAsync();

            using var command = new SqlCommand("SELECT 1 FROM Users WHERE Email = @Email", connection);
            command.Parameters.AddWithValue("@Email", user.Email);

            using var reader = await command.ExecuteReaderAsync();
            return reader.HasRows;
        }
    }
}
