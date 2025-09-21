using Dapper;
using AquaparkApp.Models;
using System.Data;

namespace AquaparkApp.DAL
{
    /// <summary>
    /// Репозиторий для работы с оплатами
    /// </summary>
    public class PaymentRepository : BaseRepository<Payment>
    {
        public PaymentRepository() : base("payments") { }

        protected override string GetTableName()
        {
            return "payments";
        }

        public override async Task<IEnumerable<Payment>> GetAllAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT p.*, c.full_name as ClientName, t.ticket_type as TicketType, s.name as ServiceName, e.full_name as EmployeeName
                FROM payments p
                JOIN clients c ON p.client_id = c.client_id
                LEFT JOIN tickets t ON p.ticket_id = t.ticket_id
                LEFT JOIN services s ON p.service_id = s.service_id
                JOIN employees e ON p.employee_id = e.employee_id
                ORDER BY p.payment_date DESC";
            
            return await connection.QueryAsync<Payment>(sql);
        }

        public override async Task<Payment?> GetByIdAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT p.*, c.full_name as ClientName, t.ticket_type as TicketType, s.name as ServiceName, e.full_name as EmployeeName
                FROM payments p
                JOIN clients c ON p.client_id = c.client_id
                LEFT JOIN tickets t ON p.ticket_id = t.ticket_id
                LEFT JOIN services s ON p.service_id = s.service_id
                JOIN employees e ON p.employee_id = e.employee_id
                WHERE p.payment_id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<Payment>(sql, new { Id = id });
        }

        public override async Task<int> CreateAsync(Payment entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                INSERT INTO payments (client_id, ticket_id, service_id, employee_id, amount, payment_date, payment_method)
                VALUES (@ClientId, @TicketId, @ServiceId, @EmployeeId, @Amount, @PaymentDate, @PaymentMethod)
                RETURNING payment_id";
            
            return await connection.QuerySingleAsync<int>(sql, entity);
        }

        public override async Task<bool> UpdateAsync(Payment entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                UPDATE payments 
                SET client_id = @ClientId, 
                    ticket_id = @TicketId, 
                    service_id = @ServiceId, 
                    employee_id = @EmployeeId, 
                    amount = @Amount, 
                    payment_date = @PaymentDate, 
                    payment_method = @PaymentMethod
                WHERE payment_id = @PaymentId";
            
            var rowsAffected = await connection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "DELETE FROM payments WHERE payment_id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Payment>> GetByClientIdAsync(int clientId)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT p.*, c.full_name as ClientName, t.ticket_type as TicketType, s.name as ServiceName, e.full_name as EmployeeName
                FROM payments p
                JOIN clients c ON p.client_id = c.client_id
                LEFT JOIN tickets t ON p.ticket_id = t.ticket_id
                LEFT JOIN services s ON p.service_id = s.service_id
                JOIN employees e ON p.employee_id = e.employee_id
                WHERE p.client_id = @ClientId
                ORDER BY p.payment_date DESC";
            
            return await connection.QueryAsync<Payment>(sql, new { ClientId = clientId });
        }

        public async Task<IEnumerable<Payment>> GetByTicketIdAsync(int ticketId)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT p.*, c.full_name as ClientName, t.ticket_type as TicketType, s.name as ServiceName, e.full_name as EmployeeName
                FROM payments p
                JOIN clients c ON p.client_id = c.client_id
                LEFT JOIN tickets t ON p.ticket_id = t.ticket_id
                LEFT JOIN services s ON p.service_id = s.service_id
                JOIN employees e ON p.employee_id = e.employee_id
                WHERE p.ticket_id = @TicketId
                ORDER BY p.payment_date DESC";
            
            return await connection.QueryAsync<Payment>(sql, new { TicketId = ticketId });
        }

        public async Task<IEnumerable<Payment>> GetByServiceIdAsync(int serviceId)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT p.*, c.full_name as ClientName, t.ticket_type as TicketType, s.name as ServiceName, e.full_name as EmployeeName
                FROM payments p
                JOIN clients c ON p.client_id = c.client_id
                LEFT JOIN tickets t ON p.ticket_id = t.ticket_id
                LEFT JOIN services s ON p.service_id = s.service_id
                JOIN employees e ON p.employee_id = e.employee_id
                WHERE p.service_id = @ServiceId
                ORDER BY p.payment_date DESC";
            
            return await connection.QueryAsync<Payment>(sql, new { ServiceId = serviceId });
        }

        public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT p.*, c.full_name as ClientName, t.ticket_type as TicketType, s.name as ServiceName, e.full_name as EmployeeName
                FROM payments p
                JOIN clients c ON p.client_id = c.client_id
                LEFT JOIN tickets t ON p.ticket_id = t.ticket_id
                LEFT JOIN services s ON p.service_id = s.service_id
                JOIN employees e ON p.employee_id = e.employee_id
                WHERE p.payment_date BETWEEN @StartDate AND @EndDate
                ORDER BY p.payment_date DESC";
            
            return await connection.QueryAsync<Payment>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "SELECT COALESCE(SUM(amount), 0) FROM payments WHERE 1=1";
            var parameters = new DynamicParameters();

            if (startDate.HasValue)
            {
                sql += " AND payment_date >= @StartDate";
                parameters.Add("StartDate", startDate.Value);
            }

            if (endDate.HasValue)
            {
                sql += " AND payment_date <= @EndDate";
                parameters.Add("EndDate", endDate.Value);
            }

            return await connection.QuerySingleAsync<decimal>(sql, parameters);
        }

        public async Task<IEnumerable<dynamic>> GetPaymentMethodStatisticsAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT 
                    payment_method,
                    COUNT(*) as payment_count,
                    SUM(amount) as total_amount,
                    AVG(amount) as avg_amount
                FROM payments
                GROUP BY payment_method
                ORDER BY total_amount DESC";
            
            return await connection.QueryAsync(sql);
        }

        public async Task<IEnumerable<dynamic>> GetDailyRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT 
                    payment_date::date as date,
                    COUNT(*) as payment_count,
                    SUM(amount) as daily_revenue
                FROM payments
                WHERE 1=1";
            var parameters = new DynamicParameters();

            if (startDate.HasValue)
            {
                sql += " AND payment_date >= @StartDate";
                parameters.Add("StartDate", startDate.Value);
            }

            if (endDate.HasValue)
            {
                sql += " AND payment_date <= @EndDate";
                parameters.Add("EndDate", endDate.Value);
            }

            sql += @"
                GROUP BY payment_date::date
                ORDER BY date DESC";
            
            return await connection.QueryAsync(sql, parameters);
        }
    }
}
