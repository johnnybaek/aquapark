using Dapper;
using AquaparkApp.Models;
using System.Data;

namespace AquaparkApp.DAL
{
    /// <summary>
    /// Репозиторий для работы с зонами аквапарка
    /// </summary>
    public class ZoneRepository : BaseRepository<Zone>
    {
        public ZoneRepository() : base("zones") { }

        protected override string GetTableName()
        {
            return "zones";
        }

        public override async Task<IEnumerable<Zone>> GetAllAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT z.*, 
                       COUNT(DISTINCT e.employee_id) as EmployeeCount,
                       COUNT(DISTINCT v.visit_id) as VisitCount,
                       COUNT(DISTINCT i.inventory_id) as InventoryCount
                FROM zones z
                LEFT JOIN employees e ON z.zone_id = e.zone_id
                LEFT JOIN visits v ON z.zone_id = v.zone_id
                LEFT JOIN inventory i ON z.zone_id = i.zone_id
                GROUP BY z.zone_id
                ORDER BY z.zone_name";
            
            return await connection.QueryAsync<Zone>(sql);
        }

        public override async Task<Zone?> GetByIdAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT z.*, 
                       COUNT(DISTINCT e.employee_id) as EmployeeCount,
                       COUNT(DISTINCT v.visit_id) as VisitCount,
                       COUNT(DISTINCT i.inventory_id) as InventoryCount
                FROM zones z
                LEFT JOIN employees e ON z.zone_id = e.zone_id
                LEFT JOIN visits v ON z.zone_id = v.zone_id
                LEFT JOIN inventory i ON z.zone_id = i.zone_id
                WHERE z.zone_id = @Id
                GROUP BY z.zone_id";
            
            return await connection.QueryFirstOrDefaultAsync<Zone>(sql, new { Id = id });
        }

        public override async Task<int> CreateAsync(Zone entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                INSERT INTO zones (zone_name, description, capacity)
                VALUES (@ZoneName, @Description, @Capacity)
                RETURNING zone_id";
            
            return await connection.QuerySingleAsync<int>(sql, entity);
        }

        public override async Task<bool> UpdateAsync(Zone entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                UPDATE zones 
                SET zone_name = @ZoneName, 
                    description = @Description, 
                    capacity = @Capacity
                WHERE zone_id = @ZoneId";
            
            var rowsAffected = await connection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "DELETE FROM zones WHERE zone_id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Zone>> GetPopularZonesAsync(int limit = 10)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT z.*, 
                       COUNT(DISTINCT e.employee_id) as EmployeeCount,
                       COUNT(DISTINCT v.visit_id) as VisitCount,
                       COUNT(DISTINCT i.inventory_id) as InventoryCount
                FROM zones z
                LEFT JOIN employees e ON z.zone_id = e.zone_id
                LEFT JOIN visits v ON z.zone_id = v.zone_id
                LEFT JOIN inventory i ON z.zone_id = i.zone_id
                GROUP BY z.zone_id
                ORDER BY VisitCount DESC
                LIMIT @Limit";
            
            return await connection.QueryAsync<Zone>(sql, new { Limit = limit });
        }

        public async Task<IEnumerable<dynamic>> GetZoneStatisticsAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT 
                    z.zone_name,
                    z.capacity,
                    COUNT(DISTINCT e.employee_id) as employee_count,
                    COUNT(DISTINCT v.visit_id) as visit_count,
                    COUNT(DISTINCT i.inventory_id) as inventory_count,
                    ROUND(AVG(EXTRACT(EPOCH FROM (v.exit_time - v.entry_time)) / 60), 2) as avg_visit_duration_minutes
                FROM zones z
                LEFT JOIN employees e ON z.zone_id = e.zone_id
                LEFT JOIN visits v ON z.zone_id = v.zone_id
                LEFT JOIN inventory i ON z.zone_id = i.zone_id
                GROUP BY z.zone_id, z.zone_name, z.capacity
                ORDER BY visit_count DESC";
            
            return await connection.QueryAsync(sql);
        }

        public async Task<IEnumerable<Zone>> SearchAsync(string searchTerm)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT z.*, 
                       COUNT(DISTINCT e.employee_id) as EmployeeCount,
                       COUNT(DISTINCT v.visit_id) as VisitCount,
                       COUNT(DISTINCT i.inventory_id) as InventoryCount
                FROM zones z
                LEFT JOIN employees e ON z.zone_id = e.zone_id
                LEFT JOIN visits v ON z.zone_id = v.zone_id
                LEFT JOIN inventory i ON z.zone_id = i.zone_id
                WHERE z.zone_name ILIKE @SearchTerm 
                   OR z.description ILIKE @SearchTerm
                GROUP BY z.zone_id
                ORDER BY z.zone_name";
            
            return await connection.QueryAsync<Zone>(sql, new { SearchTerm = $"%{searchTerm}%" });
        }
    }
}
