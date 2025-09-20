using AquaparkApp.Models;
using Dapper;

namespace AquaparkApp.DAL
{
    public class UserRepository : BaseRepository<User>
    {
        protected override string GetTableName() => "Users";

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Username = @Username AND IsActive = true", 
                new { Username = username });
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Email = @Email AND IsActive = true", 
                new { Email = email });
        }

        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            using var connection = GetConnection();
            var sql = "UPDATE Users SET LastLoginAt = @LastLoginAt WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { 
                Id = userId, 
                LastLoginAt = DateTime.UtcNow 
            });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<User>(
                "SELECT * FROM Users WHERE IsActive = true ORDER BY CreatedAt DESC");
        }

        public async Task<bool> UpdateUserStatsAsync(int userId, int totalOrders, decimal totalSpent)
        {
            using var connection = GetConnection();
            var sql = "UPDATE Users SET TotalOrders = @TotalOrders, TotalSpent = @TotalSpent WHERE Id = @UserId";
            var rowsAffected = await connection.ExecuteAsync(sql, new { UserId = userId, TotalOrders = totalOrders, TotalSpent = totalSpent });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<User>> GetTopUsersBySpendingAsync(int limit = 10)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM Users WHERE TotalSpent > 0 ORDER BY TotalSpent DESC LIMIT @Limit";
            return await connection.QueryAsync<User>(sql, new { Limit = limit });
        }

        public async Task<IEnumerable<User>> GetUsersByRegistrationDateAsync(DateTime startDate, DateTime endDate)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM Users WHERE CreatedAt BETWEEN @StartDate AND @EndDate ORDER BY CreatedAt DESC";
            return await connection.QueryAsync<User>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<int> GetActiveUsersCountAsync()
        {
            using var connection = GetConnection();
            var sql = "SELECT COUNT(*) FROM Users WHERE IsActive = true";
            return await connection.QuerySingleAsync<int>(sql);
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            using var connection = GetConnection();
            var sql = "UPDATE Users SET IsActive = false WHERE Id = @UserId";
            var rowsAffected = await connection.ExecuteAsync(sql, new { UserId = userId });
            return rowsAffected > 0;
        }
    }
}
