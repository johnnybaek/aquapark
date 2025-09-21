using Dapper;
using AquaparkApp.Models;
using System.Data;

namespace AquaparkApp.DAL
{
    /// <summary>
    /// Репозиторий для работы с сотрудниками
    /// </summary>
    public class EmployeeRepository : BaseRepository<Employee>
    {
        public EmployeeRepository() : base("employees") { }

        protected override string GetTableName()
        {
            return "employees";
        }

        public override async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT e.*, z.zone_name as ZoneName
                FROM employees e
                LEFT JOIN zones z ON e.zone_id = z.zone_id
                ORDER BY e.full_name";
            
            return await connection.QueryAsync<Employee>(sql);
        }

        public override async Task<Employee?> GetByIdAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT e.*, z.zone_name as ZoneName
                FROM employees e
                LEFT JOIN zones z ON e.zone_id = z.zone_id
                WHERE e.employee_id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<Employee>(sql, new { Id = id });
        }

        public override async Task<int> CreateAsync(Employee entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                INSERT INTO employees (full_name, position, phone, hire_date, zone_id)
                VALUES (@FullName, @Position, @Phone, @HireDate, @ZoneId)
                RETURNING employee_id";
            
            return await connection.QuerySingleAsync<int>(sql, entity);
        }

        public override async Task<bool> UpdateAsync(Employee entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                UPDATE employees 
                SET full_name = @FullName, 
                    position = @Position, 
                    phone = @Phone, 
                    zone_id = @ZoneId
                WHERE employee_id = @EmployeeId";
            
            var rowsAffected = await connection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "DELETE FROM employees WHERE employee_id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Employee>> GetByZoneIdAsync(int zoneId)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT e.*, z.zone_name as ZoneName
                FROM employees e
                LEFT JOIN zones z ON e.zone_id = z.zone_id
                WHERE e.zone_id = @ZoneId
                ORDER BY e.full_name";
            
            return await connection.QueryAsync<Employee>(sql, new { ZoneId = zoneId });
        }

        public async Task<IEnumerable<Employee>> GetByPositionAsync(string position)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT e.*, z.zone_name as ZoneName
                FROM employees e
                LEFT JOIN zones z ON e.zone_id = z.zone_id
                WHERE e.position ILIKE @Position
                ORDER BY e.full_name";
            
            return await connection.QueryAsync<Employee>(sql, new { Position = $"%{position}%" });
        }

        public async Task<IEnumerable<dynamic>> GetPositionStatisticsAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT 
                    position,
                    COUNT(*) as count,
                    AVG(EXTRACT(YEAR FROM AGE(CURRENT_DATE, hire_date))) as avg_years_of_service
                FROM employees
                GROUP BY position
                ORDER BY count DESC";
            
            return await connection.QueryAsync(sql);
        }
    }
}
