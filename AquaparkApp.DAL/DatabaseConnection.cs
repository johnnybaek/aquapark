using Npgsql;
using System.Data;
using Dapper;

namespace AquaparkApp.DAL
{
    public class DatabaseConnection
    {
        private static readonly string ConnectionString = "Host=localhost;Database=aquapark_db;Username=postgres;Password=123;Port=5432;";

        static DatabaseConnection()
        {
            // Включаем сопоставление snake_case колонок (client_id) с PascalCase свойствами (ClientId)
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public static IDbConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public static async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = GetConnection() as NpgsqlConnection;
                if (connection != null)
                {
                    await connection.OpenAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                // Логируем ошибку для отладки
                System.Diagnostics.Debug.WriteLine($"Database connection error: {ex.Message}");
                return false;
            }
        }
    }
}
