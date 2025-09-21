using Dapper;
using AquaparkApp.Models;
using System.Data;

namespace AquaparkApp.DAL
{
    /// <summary>
    /// Репозиторий для работы с посещениями
    /// </summary>
    public class VisitRepository : BaseRepository<Visit>
    {
        public VisitRepository() : base("visits") { }

        protected override string GetTableName()
        {
            return "visits";
        }

        public override async Task<IEnumerable<Visit>> GetAllAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT v.*, c.full_name as ClientName, t.ticket_type as TicketType, z.zone_name as ZoneName
                FROM visits v
                JOIN clients c ON v.client_id = c.client_id
                JOIN tickets t ON v.ticket_id = t.ticket_id
                LEFT JOIN zones z ON v.zone_id = z.zone_id
                ORDER BY v.entry_time DESC";
            
            return await connection.QueryAsync<Visit>(sql);
        }

        public override async Task<Visit?> GetByIdAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT v.*, c.full_name as ClientName, t.ticket_type as TicketType, z.zone_name as ZoneName
                FROM visits v
                JOIN clients c ON v.client_id = c.client_id
                JOIN tickets t ON v.ticket_id = t.ticket_id
                LEFT JOIN zones z ON v.zone_id = z.zone_id
                WHERE v.visit_id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<Visit>(sql, new { Id = id });
        }

        public override async Task<int> CreateAsync(Visit entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                INSERT INTO visits (client_id, ticket_id, entry_time, exit_time, zone_id)
                VALUES (@ClientId, @TicketId, @EntryTime, @ExitTime, @ZoneId)
                RETURNING visit_id";
            
            return await connection.QuerySingleAsync<int>(sql, entity);
        }

        public override async Task<bool> UpdateAsync(Visit entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                UPDATE visits 
                SET client_id = @ClientId, 
                    ticket_id = @TicketId, 
                    entry_time = @EntryTime, 
                    exit_time = @ExitTime, 
                    zone_id = @ZoneId
                WHERE visit_id = @VisitId";
            
            var rowsAffected = await connection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "DELETE FROM visits WHERE visit_id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Visit>> GetByClientIdAsync(int clientId)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT v.*, c.full_name as ClientName, t.ticket_type as TicketType, z.zone_name as ZoneName
                FROM visits v
                JOIN clients c ON v.client_id = c.client_id
                JOIN tickets t ON v.ticket_id = t.ticket_id
                LEFT JOIN zones z ON v.zone_id = z.zone_id
                WHERE v.client_id = @ClientId
                ORDER BY v.entry_time DESC";
            
            return await connection.QueryAsync<Visit>(sql, new { ClientId = clientId });
        }

        public async Task<IEnumerable<Visit>> GetByZoneIdAsync(int zoneId)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT v.*, c.full_name as ClientName, t.ticket_type as TicketType, z.zone_name as ZoneName
                FROM visits v
                JOIN clients c ON v.client_id = c.client_id
                JOIN tickets t ON v.ticket_id = t.ticket_id
                LEFT JOIN zones z ON v.zone_id = z.zone_id
                WHERE v.zone_id = @ZoneId
                ORDER BY v.entry_time DESC";
            
            return await connection.QueryAsync<Visit>(sql, new { ZoneId = zoneId });
        }

        public async Task<IEnumerable<Visit>> GetActiveVisitsAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT v.*, c.full_name as ClientName, t.ticket_type as TicketType, z.zone_name as ZoneName
                FROM visits v
                JOIN clients c ON v.client_id = c.client_id
                JOIN tickets t ON v.ticket_id = t.ticket_id
                LEFT JOIN zones z ON v.zone_id = z.zone_id
                WHERE v.exit_time IS NULL
                ORDER BY v.entry_time ASC";
            
            return await connection.QueryAsync<Visit>(sql);
        }

        public async Task<IEnumerable<Visit>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT v.*, c.full_name as ClientName, t.ticket_type as TicketType, z.zone_name as ZoneName
                FROM visits v
                JOIN clients c ON v.client_id = c.client_id
                JOIN tickets t ON v.ticket_id = t.ticket_id
                LEFT JOIN zones z ON v.zone_id = z.zone_id
                WHERE v.entry_time BETWEEN @StartDate AND @EndDate
                ORDER BY v.entry_time DESC";
            
            return await connection.QueryAsync<Visit>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<bool> CheckOutVisitAsync(int visitId, DateTime exitTime)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "UPDATE visits SET exit_time = @ExitTime WHERE visit_id = @VisitId";
            var rowsAffected = await connection.ExecuteAsync(sql, new { 
                VisitId = visitId, 
                ExitTime = exitTime 
            });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<dynamic>> GetVisitStatisticsAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT 
                    z.zone_name,
                    COUNT(v.visit_id) as visit_count,
                    COUNT(DISTINCT v.client_id) as unique_clients,
                    AVG(EXTRACT(EPOCH FROM (v.exit_time - v.entry_time)) / 60) as avg_duration_minutes
                FROM visits v
                LEFT JOIN zones z ON v.zone_id = z.zone_id
                WHERE v.exit_time IS NOT NULL
                GROUP BY z.zone_name
                ORDER BY visit_count DESC";
            
            return await connection.QueryAsync(sql);
        }
    }
}
