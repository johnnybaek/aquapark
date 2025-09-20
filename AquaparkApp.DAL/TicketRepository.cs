using AquaparkApp.Models;
using Dapper;

namespace AquaparkApp.DAL
{
    public class TicketRepository : BaseRepository<Ticket>
    {
        protected override string GetTableName() => "Tickets";

        public async Task<IEnumerable<Ticket>> GetByUserIdAsync(int userId)
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<Ticket>(
                @"SELECT t.*, u.Username, u.Email, u.FirstName, u.LastName, 
                         a.Name as AttractionName, a.Description as AttractionDescription
                  FROM Tickets t
                  LEFT JOIN Users u ON t.UserId = u.Id
                  LEFT JOIN Attractions a ON t.AttractionId = a.Id
                  WHERE t.UserId = @UserId
                  ORDER BY t.CreatedAt DESC", 
                new { UserId = userId });
        }

        public async Task<IEnumerable<Ticket>> GetByOrderIdAsync(int orderId)
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<Ticket>(
                @"SELECT t.*, u.Username, u.Email, u.FirstName, u.LastName, 
                         a.Name as AttractionName, a.Description as AttractionDescription
                  FROM Tickets t
                  LEFT JOIN Users u ON t.UserId = u.Id
                  LEFT JOIN Attractions a ON t.AttractionId = a.Id
                  WHERE t.OrderId = @OrderId
                  ORDER BY t.CreatedAt", 
                new { OrderId = orderId });
        }

        public async Task<IEnumerable<Ticket>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<Ticket>(
                @"SELECT t.*, u.Username, u.Email, u.FirstName, u.LastName, 
                         a.Name as AttractionName, a.Description as AttractionDescription
                  FROM Tickets t
                  LEFT JOIN Users u ON t.UserId = u.Id
                  LEFT JOIN Attractions a ON t.AttractionId = a.Id
                  WHERE t.VisitDate BETWEEN @StartDate AND @EndDate
                  ORDER BY t.VisitDate, t.VisitTime", 
                new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<bool> UpdateStatusAsync(int ticketId, string status)
        {
            using var connection = GetConnection();
            var sql = "UPDATE Tickets SET Status = @Status WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { 
                Id = ticketId, 
                Status = status 
            });
            return rowsAffected > 0;
        }

        public async Task<bool> MarkAsUsedAsync(int ticketId)
        {
            using var connection = GetConnection();
            var sql = "UPDATE Tickets SET Status = 'Used', UsedAt = @UsedAt WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { 
                Id = ticketId, 
                UsedAt = DateTime.UtcNow 
            });
            return rowsAffected > 0;
        }
    }
}
