using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Model;
using Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UserRepository(IOptions<AppSettings> options) : IUserRepository
    {
        public async Task<bool> CreateUser(UserRequest user)
        {
            try
            {
                string stringConnection = options.Value.DefaultConnection;
                using var connection = new SqlConnection(stringConnection);
                await connection.OpenAsync();
                using var command = new SqlCommand("INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password)", connection);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                int rowsAffected = await command.ExecuteNonQueryAsync();
                connection.CloseAsync();
                if (rowsAffected > 0)
                {
                    return true;
                }

                throw new Exception("No se creo el usuario.");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> Login(LoginRequest user)
        {
            string stringConnection = options.Value.DefaultConnection;
            using var connection = new SqlConnection(stringConnection);
            await connection.OpenAsync();
            using var command = new SqlCommand("select Password from Users where email =@email ", connection);
            command.Parameters.AddWithValue("@Email", user.Email);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
               string password = reader["Password"].ToString();
                if (password == user.Password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ValidEmailUser(UserRequest user)
        {
            string stringConnection = options.Value.DefaultConnection;
            using var connection = new SqlConnection(stringConnection);
            await connection.OpenAsync();
            using var command = new SqlCommand("select 1 from Users where email =@email ", connection);
            command.Parameters.AddWithValue("@Email", user.Email);
            using var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
