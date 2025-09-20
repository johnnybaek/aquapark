using AquaparkApp.Models;
using Dapper;

namespace AquaparkApp.DAL
{
    public class OrderRepository : BaseRepository<Order>
    {
        protected override string GetTableName() => "Orders";

        public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
        {
            using var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<Order>(
                "SELECT * FROM Orders WHERE OrderNumber = @OrderNumber", 
                new { OrderNumber = orderNumber });
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<Order>(
                @"SELECT o.*, u.Username, u.Email, u.FirstName, u.LastName
                  FROM Orders o
                  LEFT JOIN Users u ON o.UserId = u.Id
                  WHERE o.UserId = @UserId
                  ORDER BY o.CreatedAt DESC", 
                new { UserId = userId });
        }

        public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<Order>(
                @"SELECT o.*, u.Username, u.Email, u.FirstName, u.LastName
                  FROM Orders o
                  LEFT JOIN Users u ON o.UserId = u.Id
                  WHERE o.CreatedAt BETWEEN @StartDate AND @EndDate
                  ORDER BY o.CreatedAt DESC", 
                new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<bool> UpdateStatusAsync(int orderId, string status)
        {
            using var connection = GetConnection();
            var sql = "UPDATE Orders SET Status = @Status WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { 
                Id = orderId, 
                Status = status 
            });
            return rowsAffected > 0;
        }

        public async Task<bool> MarkAsPaidAsync(int orderId)
        {
            using var connection = GetConnection();
            var sql = "UPDATE Orders SET Status = 'Paid', PaidAt = @PaidAt WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { 
                Id = orderId, 
                PaidAt = DateTime.UtcNow 
            });
            return rowsAffected > 0;
        }

        public async Task<string> GenerateOrderNumberAsync()
        {
            using var connection = GetConnection();
            var count = await connection.QuerySingleAsync<int>(
                "SELECT COUNT(*) FROM Orders WHERE DATE(CreatedAt) = @Today", 
                new { Today = DateTime.Today });
            
            return $"ORD{DateTime.Now:yyyyMMdd}{count + 1:D4}";
        }
    }
}
