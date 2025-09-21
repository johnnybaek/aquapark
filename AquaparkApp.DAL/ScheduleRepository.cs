using Dapper;
using AquaparkApp.Models;
using System.Data;

namespace AquaparkApp.DAL
{
    /// <summary>
    /// Репозиторий для работы с расписанием
    /// </summary>
    public class ScheduleRepository : BaseRepository<Schedule>
    {
        public ScheduleRepository() : base("schedule") { }

        protected override string GetTableName()
        {
            return "schedule";
        }

        public override async Task<IEnumerable<Schedule>> GetAllAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT s.*, e.full_name as EmployeeName, z.zone_name as ZoneName
                FROM schedule s
                JOIN employees e ON s.employee_id = e.employee_id
                JOIN zones z ON s.zone_id = z.zone_id
                ORDER BY s.work_date DESC, s.shift_start";
            
            return await connection.QueryAsync<Schedule>(sql);
        }

        public override async Task<Schedule?> GetByIdAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT s.*, e.full_name as EmployeeName, z.zone_name as ZoneName
                FROM schedule s
                JOIN employees e ON s.employee_id = e.employee_id
                JOIN zones z ON s.zone_id = z.zone_id
                WHERE s.schedule_id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<Schedule>(sql, new { Id = id });
        }

        public override async Task<int> CreateAsync(Schedule entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                INSERT INTO schedule (employee_id, zone_id, work_date, shift_start, shift_end)
                VALUES (@EmployeeId, @ZoneId, @WorkDate, @ShiftStart, @ShiftEnd)
                RETURNING schedule_id";
            
            return await connection.QuerySingleAsync<int>(sql, entity);
        }

        public override async Task<bool> UpdateAsync(Schedule entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                UPDATE schedule 
                SET employee_id = @EmployeeId, 
                    zone_id = @ZoneId, 
                    work_date = @WorkDate, 
                    shift_start = @ShiftStart, 
                    shift_end = @ShiftEnd
                WHERE schedule_id = @ScheduleId";
            
            var rowsAffected = await connection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "DELETE FROM schedule WHERE schedule_id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Schedule>> GetByEmployeeIdAsync(int employeeId)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT s.*, e.full_name as EmployeeName, z.zone_name as ZoneName
                FROM schedule s
                JOIN employees e ON s.employee_id = e.employee_id
                JOIN zones z ON s.zone_id = z.zone_id
                WHERE s.employee_id = @EmployeeId
                ORDER BY s.work_date DESC, s.shift_start";
            
            return await connection.QueryAsync<Schedule>(sql, new { EmployeeId = employeeId });
        }

        public async Task<IEnumerable<Schedule>> GetByZoneIdAsync(int zoneId)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT s.*, e.full_name as EmployeeName, z.zone_name as ZoneName
                FROM schedule s
                JOIN employees e ON s.employee_id = e.employee_id
                JOIN zones z ON s.zone_id = z.zone_id
                WHERE s.zone_id = @ZoneId
                ORDER BY s.work_date DESC, s.shift_start";
            
            return await connection.QueryAsync<Schedule>(sql, new { ZoneId = zoneId });
        }

        public async Task<IEnumerable<Schedule>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT s.*, e.full_name as EmployeeName, z.zone_name as ZoneName
                FROM schedule s
                JOIN employees e ON s.employee_id = e.employee_id
                JOIN zones z ON s.zone_id = z.zone_id
                WHERE s.work_date BETWEEN @StartDate AND @EndDate
                ORDER BY s.work_date, s.shift_start";
            
            return await connection.QueryAsync<Schedule>(sql, new { StartDate = startDate, EndDate = endDate });
        }
    }
}
