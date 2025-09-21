using Dapper;
using AquaparkApp.Models;
using System.Data;

namespace AquaparkApp.DAL
{
    /// <summary>
    /// Репозиторий для работы с билетами
    /// </summary>
    public class TicketRepository : BaseRepository<Ticket>
    {
        public TicketRepository() : base("tickets") { }

        protected override string GetTableName()
        {
            return "tickets";
        }

        public override async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT t.*, c.full_name as ClientName
                FROM tickets t
                JOIN clients c ON t.client_id = c.client_id
                ORDER BY t.purchase_date DESC";
            
            return await connection.QueryAsync<Ticket>(sql);
        }

        public override async Task<Ticket?> GetByIdAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT t.*, c.full_name as ClientName
                FROM tickets t
                JOIN clients c ON t.client_id = c.client_id
                WHERE t.ticket_id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<Ticket>(sql, new { Id = id });
        }

        public override async Task<int> CreateAsync(Ticket entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                INSERT INTO tickets (client_id, ticket_type, price, purchase_date, valid_until)
                VALUES (@ClientId, @TicketType, @Price, @PurchaseDate, @ValidUntil)
                RETURNING ticket_id";
            
            return await connection.QuerySingleAsync<int>(sql, entity);
        }

        public override async Task<bool> UpdateAsync(Ticket entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                UPDATE tickets 
                SET client_id = @ClientId, 
                    ticket_type = @TicketType, 
                    price = @Price, 
                    valid_until = @ValidUntil
                WHERE ticket_id = @TicketId";
            
            var rowsAffected = await connection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "DELETE FROM tickets WHERE ticket_id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Ticket>> GetByClientIdAsync(int clientId)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT t.*, c.full_name as ClientName
                FROM tickets t
                JOIN clients c ON t.client_id = c.client_id
                WHERE t.client_id = @ClientId
                ORDER BY t.purchase_date DESC";
            
            return await connection.QueryAsync<Ticket>(sql, new { ClientId = clientId });
        }

        public async Task<IEnumerable<Ticket>> GetValidTicketsAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT t.*, c.full_name as ClientName
                FROM tickets t
                JOIN clients c ON t.client_id = c.client_id
                WHERE t.valid_until >= CURRENT_DATE
                ORDER BY t.valid_until ASC";
            
            return await connection.QueryAsync<Ticket>(sql);
        }

        public async Task<IEnumerable<Ticket>> GetExpiredTicketsAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT t.*, c.full_name as ClientName
                FROM tickets t
                JOIN clients c ON t.client_id = c.client_id
                WHERE t.valid_until < CURRENT_DATE
                ORDER BY t.valid_until DESC";
            
            return await connection.QueryAsync<Ticket>(sql);
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "SELECT COALESCE(SUM(price), 0) FROM tickets WHERE 1=1";
            var parameters = new DynamicParameters();

            if (startDate.HasValue)
            {
                sql += " AND purchase_date >= @StartDate";
                parameters.Add("StartDate", startDate.Value);
            }

            if (endDate.HasValue)
            {
                sql += " AND purchase_date <= @EndDate";
                parameters.Add("EndDate", endDate.Value);
            }

            return await connection.QuerySingleAsync<decimal>(sql, parameters);
        }

        public async Task<IEnumerable<dynamic>> GetTicketTypeStatisticsAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT 
                    ticket_type,
                    COUNT(*) as count,
                    SUM(price) as total_revenue,
                    AVG(price) as avg_price
                FROM tickets
                GROUP BY ticket_type
                ORDER BY count DESC";
            
            return await connection.QueryAsync(sql);
        }
    }
}