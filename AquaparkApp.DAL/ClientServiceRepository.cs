using Dapper;
using AquaparkApp.Models;
using System.Data;

namespace AquaparkApp.DAL
{
    /// <summary>
    /// Репозиторий для работы с обслуживанием клиентов
    /// </summary>
    public class ClientServiceRepository : BaseRepository<ClientService>
    {
        public ClientServiceRepository() : base("client_services") { }

        protected override string GetTableName()
        {
            return "client_services";
        }

        public override async Task<IEnumerable<ClientService>> GetAllAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT cs.*, c.full_name as ClientName, s.name as ServiceName, e.full_name as EmployeeName
                FROM client_services cs
                JOIN clients c ON cs.client_id = c.client_id
                JOIN services s ON cs.service_id = s.service_id
                JOIN employees e ON cs.employee_id = e.employee_id
                ORDER BY cs.service_date DESC";
            
            return await connection.QueryAsync<ClientService>(sql);
        }

        public override async Task<ClientService?> GetByIdAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT cs.*, c.full_name as ClientName, s.name as ServiceName, e.full_name as EmployeeName
                FROM client_services cs
                JOIN clients c ON cs.client_id = c.client_id
                JOIN services s ON cs.service_id = s.service_id
                JOIN employees e ON cs.employee_id = e.employee_id
                WHERE cs.service_log_id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<ClientService>(sql, new { Id = id });
        }

        public override async Task<int> CreateAsync(ClientService entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                INSERT INTO client_services (client_id, service_id, employee_id, service_date, quantity)
                VALUES (@ClientId, @ServiceId, @EmployeeId, @ServiceDate, @Quantity)
                RETURNING service_log_id";
            
            return await connection.QuerySingleAsync<int>(sql, entity);
        }

        public override async Task<bool> UpdateAsync(ClientService entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                UPDATE client_services 
                SET client_id = @ClientId, 
                    service_id = @ServiceId, 
                    employee_id = @EmployeeId, 
                    service_date = @ServiceDate, 
                    quantity = @Quantity
                WHERE service_log_id = @ServiceLogId";
            
            var rowsAffected = await connection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "DELETE FROM client_services WHERE service_log_id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<ClientService>> GetByClientIdAsync(int clientId)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT cs.*, c.full_name as ClientName, s.name as ServiceName, e.full_name as EmployeeName
                FROM client_services cs
                JOIN clients c ON cs.client_id = c.client_id
                JOIN services s ON cs.service_id = s.service_id
                JOIN employees e ON cs.employee_id = e.employee_id
                WHERE cs.client_id = @ClientId
                ORDER BY cs.service_date DESC";
            
            return await connection.QueryAsync<ClientService>(sql, new { ClientId = clientId });
        }

        public async Task<IEnumerable<ClientService>> GetByServiceIdAsync(int serviceId)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT cs.*, c.full_name as ClientName, s.name as ServiceName, e.full_name as EmployeeName
                FROM client_services cs
                JOIN clients c ON cs.client_id = c.client_id
                JOIN services s ON cs.service_id = s.service_id
                JOIN employees e ON cs.employee_id = e.employee_id
                WHERE cs.service_id = @ServiceId
                ORDER BY cs.service_date DESC";
            
            return await connection.QueryAsync<ClientService>(sql, new { ServiceId = serviceId });
        }

        public async Task<IEnumerable<ClientService>> GetByEmployeeIdAsync(int employeeId)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT cs.*, c.full_name as ClientName, s.name as ServiceName, e.full_name as EmployeeName
                FROM client_services cs
                JOIN clients c ON cs.client_id = c.client_id
                JOIN services s ON cs.service_id = s.service_id
                JOIN employees e ON cs.employee_id = e.employee_id
                WHERE cs.employee_id = @EmployeeId
                ORDER BY cs.service_date DESC";
            
            return await connection.QueryAsync<ClientService>(sql, new { EmployeeId = employeeId });
        }

        public async Task<IEnumerable<ClientService>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT cs.*, c.full_name as ClientName, s.name as ServiceName, e.full_name as EmployeeName
                FROM client_services cs
                JOIN clients c ON cs.client_id = c.client_id
                JOIN services s ON cs.service_id = s.service_id
                JOIN employees e ON cs.employee_id = e.employee_id
                WHERE cs.service_date BETWEEN @StartDate AND @EndDate
                ORDER BY cs.service_date DESC";
            
            return await connection.QueryAsync<ClientService>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<IEnumerable<dynamic>> GetServiceStatisticsAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT 
                    s.name as service_name,
                    COUNT(cs.service_log_id) as total_orders,
                    SUM(cs.quantity) as total_quantity,
                    SUM(cs.quantity * s.price) as total_revenue
                FROM client_services cs
                JOIN services s ON cs.service_id = s.service_id
                GROUP BY s.service_id, s.name
                ORDER BY total_revenue DESC";
            
            return await connection.QueryAsync(sql);
        }
    }
}
