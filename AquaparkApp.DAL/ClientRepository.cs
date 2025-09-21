using Dapper;
using AquaparkApp.Models;
using System.Data;

namespace AquaparkApp.DAL
{
    /// <summary>
    /// Репозиторий для работы с клиентами
    /// </summary>
    public class ClientRepository : BaseRepository<Client>
    {
        public ClientRepository() : base("clients") { }

        protected override string GetTableName()
        {
            return "clients";
        }

        public override async Task<IEnumerable<Client>> GetAllAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT c.*, 
                       COUNT(t.ticket_id) as TotalTickets,
                       COUNT(v.visit_id) as TotalVisits,
                       COALESCE(SUM(p.amount), 0) as TotalSpent
                FROM clients c
                LEFT JOIN tickets t ON c.client_id = t.client_id
                LEFT JOIN visits v ON c.client_id = v.client_id
                LEFT JOIN payments p ON c.client_id = p.client_id
                GROUP BY c.client_id
                ORDER BY c.full_name";
            
            return await connection.QueryAsync<Client>(sql);
        }

        public override async Task<Client?> GetByIdAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT c.*, 
                       COUNT(t.ticket_id) as TotalTickets,
                       COUNT(v.visit_id) as TotalVisits,
                       COALESCE(SUM(p.amount), 0) as TotalSpent
                FROM clients c
                LEFT JOIN tickets t ON c.client_id = t.client_id
                LEFT JOIN visits v ON c.client_id = v.client_id
                LEFT JOIN payments p ON c.client_id = p.client_id
                WHERE c.client_id = @Id
                GROUP BY c.client_id";
            
            return await connection.QueryFirstOrDefaultAsync<Client>(sql, new { Id = id });
        }

        public override async Task<int> CreateAsync(Client entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                INSERT INTO clients (full_name, phone, email, birth_date, registration_date)
                VALUES (@FullName, @Phone, @Email, @BirthDate, @RegistrationDate)
                RETURNING client_id";
            
            return await connection.QuerySingleAsync<int>(sql, entity);
        }

        public override async Task<bool> UpdateAsync(Client entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                UPDATE clients 
                SET full_name = @FullName, 
                    phone = @Phone, 
                    email = @Email, 
                    birth_date = @BirthDate
                WHERE client_id = @ClientId";
            
            var rowsAffected = await connection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "DELETE FROM clients WHERE client_id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Client>> SearchAsync(string searchTerm)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT c.*, 
                       COUNT(t.ticket_id) as TotalTickets,
                       COUNT(v.visit_id) as TotalVisits,
                       COALESCE(SUM(p.amount), 0) as TotalSpent
                FROM clients c
                LEFT JOIN tickets t ON c.client_id = t.client_id
                LEFT JOIN visits v ON c.client_id = v.client_id
                LEFT JOIN payments p ON c.client_id = p.client_id
                WHERE c.full_name ILIKE @SearchTerm 
                   OR c.phone ILIKE @SearchTerm 
                   OR c.email ILIKE @SearchTerm
                GROUP BY c.client_id
                ORDER BY c.full_name";
            
            return await connection.QueryAsync<Client>(sql, new { SearchTerm = $"%{searchTerm}%" });
        }

        public async Task<IEnumerable<Client>> GetTopSpendersAsync(int limit = 10)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT c.*, 
                       COUNT(t.ticket_id) as TotalTickets,
                       COUNT(v.visit_id) as TotalVisits,
                       COALESCE(SUM(p.amount), 0) as TotalSpent
                FROM clients c
                LEFT JOIN tickets t ON c.client_id = t.client_id
                LEFT JOIN visits v ON c.client_id = v.client_id
                LEFT JOIN payments p ON c.client_id = p.client_id
                GROUP BY c.client_id
                ORDER BY TotalSpent DESC
                LIMIT @Limit";
            
            return await connection.QueryAsync<Client>(sql, new { Limit = limit });
        }

        public async Task<IEnumerable<Client>> GetRecentClientsAsync(int days = 30)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT c.*, 
                       COUNT(t.ticket_id) as TotalTickets,
                       COUNT(v.visit_id) as TotalVisits,
                       COALESCE(SUM(p.amount), 0) as TotalSpent
                FROM clients c
                LEFT JOIN tickets t ON c.client_id = t.client_id
                LEFT JOIN visits v ON c.client_id = v.client_id
                LEFT JOIN payments p ON c.client_id = p.client_id
                WHERE c.registration_date >= @StartDate
                GROUP BY c.client_id
                ORDER BY c.registration_date DESC";
            
            return await connection.QueryAsync<Client>(sql, new { StartDate = DateTime.Now.AddDays(-days) });
        }
    }
}
