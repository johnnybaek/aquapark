using Dapper;
using AquaparkApp.Models;
using System.Data;

namespace AquaparkApp.DAL
{
    /// <summary>
    /// Репозиторий для работы с услугами
    /// </summary>
    public class ServiceRepository : BaseRepository<Service>
    {
        public ServiceRepository() : base("services") { }

        protected override string GetTableName()
        {
            return "services";
        }

        public override async Task<IEnumerable<Service>> GetAllAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT s.*, 
                       COUNT(cs.service_log_id) as TotalOrders,
                       COALESCE(SUM(cs.quantity), 0) as TotalQuantity
                FROM services s
                LEFT JOIN client_services cs ON s.service_id = cs.service_id
                GROUP BY s.service_id
                ORDER BY s.name";
            
            return await connection.QueryAsync<Service>(sql);
        }

        public override async Task<Service?> GetByIdAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT s.*, 
                       COUNT(cs.service_log_id) as TotalOrders,
                       COALESCE(SUM(cs.quantity), 0) as TotalQuantity
                FROM services s
                LEFT JOIN client_services cs ON s.service_id = cs.service_id
                WHERE s.service_id = @Id
                GROUP BY s.service_id";
            
            return await connection.QueryFirstOrDefaultAsync<Service>(sql, new { Id = id });
        }

        public override async Task<int> CreateAsync(Service entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                INSERT INTO services (name, description, price)
                VALUES (@Name, @Description, @Price)
                RETURNING service_id";
            
            return await connection.QuerySingleAsync<int>(sql, entity);
        }

        public override async Task<bool> UpdateAsync(Service entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                UPDATE services 
                SET name = @Name, 
                    description = @Description, 
                    price = @Price
                WHERE service_id = @ServiceId";
            
            var rowsAffected = await connection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "DELETE FROM services WHERE service_id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Service>> SearchAsync(string searchTerm)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT s.*, 
                       COUNT(cs.service_log_id) as TotalOrders,
                       COALESCE(SUM(cs.quantity), 0) as TotalQuantity
                FROM services s
                LEFT JOIN client_services cs ON s.service_id = cs.service_id
                WHERE s.name ILIKE @SearchTerm 
                   OR s.description ILIKE @SearchTerm
                GROUP BY s.service_id
                ORDER BY s.name";
            
            return await connection.QueryAsync<Service>(sql, new { SearchTerm = $"%{searchTerm}%" });
        }

        public async Task<IEnumerable<Service>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT s.*, 
                       COUNT(cs.service_log_id) as TotalOrders,
                       COALESCE(SUM(cs.quantity), 0) as TotalQuantity
                FROM services s
                LEFT JOIN client_services cs ON s.service_id = cs.service_id
                WHERE s.price BETWEEN @MinPrice AND @MaxPrice
                GROUP BY s.service_id
                ORDER BY s.price";
            
            return await connection.QueryAsync<Service>(sql, new { MinPrice = minPrice, MaxPrice = maxPrice });
        }

        public async Task<IEnumerable<dynamic>> GetPopularServicesAsync(int limit = 10)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT 
                    s.name,
                    s.price,
                    COUNT(cs.service_log_id) as order_count,
                    SUM(cs.quantity) as total_quantity,
                    SUM(cs.quantity * s.price) as total_revenue
                FROM services s
                LEFT JOIN client_services cs ON s.service_id = cs.service_id
                GROUP BY s.service_id, s.name, s.price
                ORDER BY order_count DESC
                LIMIT @Limit";
            
            return await connection.QueryAsync(sql, new { Limit = limit });
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT COALESCE(SUM(cs.quantity * s.price), 0)
                FROM services s
                JOIN client_services cs ON s.service_id = cs.service_id
                WHERE 1=1";
            var parameters = new DynamicParameters();

            if (startDate.HasValue)
            {
                sql += " AND cs.service_date >= @StartDate";
                parameters.Add("StartDate", startDate.Value);
            }

            if (endDate.HasValue)
            {
                sql += " AND cs.service_date <= @EndDate";
                parameters.Add("EndDate", endDate.Value);
            }

            return await connection.QuerySingleAsync<decimal>(sql, parameters);
        }
    }
}
