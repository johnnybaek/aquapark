using Npgsql;
using System.Data;

namespace AquaparkApp.DAL
{
    public class DatabaseConnection
    {
        private static readonly string ConnectionString = "Host=localhost;Database=aquapark_db;Username=postgres;Password=postgres;Port=5432;";

        public static IDbConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public static async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = GetConnection();
                await connection.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
