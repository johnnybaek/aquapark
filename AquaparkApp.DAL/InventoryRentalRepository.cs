using Dapper;
using AquaparkApp.Models;
using System.Data;

namespace AquaparkApp.DAL
{
    /// <summary>
    /// Репозиторий для работы с арендой инвентаря
    /// </summary>
    public class InventoryRentalRepository : BaseRepository<InventoryRental>
    {
        public InventoryRentalRepository() : base("inventory_rentals") { }

        protected override string GetTableName()
        {
            return "inventory_rentals";
        }

        public override async Task<IEnumerable<InventoryRental>> GetAllAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT ir.*, c.full_name as ClientName, i.name as InventoryName, e.full_name as EmployeeName
                FROM inventory_rentals ir
                JOIN clients c ON ir.client_id = c.client_id
                JOIN inventory i ON ir.inventory_id = i.inventory_id
                JOIN employees e ON ir.employee_id = e.employee_id
                ORDER BY ir.rental_date DESC";
            
            return await connection.QueryAsync<InventoryRental>(sql);
        }

        public override async Task<InventoryRental?> GetByIdAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT ir.*, c.full_name as ClientName, i.name as InventoryName, e.full_name as EmployeeName
                FROM inventory_rentals ir
                JOIN clients c ON ir.client_id = c.client_id
                JOIN inventory i ON ir.inventory_id = i.inventory_id
                JOIN employees e ON ir.employee_id = e.employee_id
                WHERE ir.rental_id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<InventoryRental>(sql, new { Id = id });
        }

        public override async Task<int> CreateAsync(InventoryRental entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                INSERT INTO inventory_rentals (client_id, inventory_id, employee_id, rental_date, return_date, deposit_amount)
                VALUES (@ClientId, @InventoryId, @EmployeeId, @RentalDate, @ReturnDate, @DepositAmount)
                RETURNING rental_id";
            
            return await connection.QuerySingleAsync<int>(sql, entity);
        }

        public override async Task<bool> UpdateAsync(InventoryRental entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                UPDATE inventory_rentals 
                SET client_id = @ClientId, 
                    inventory_id = @InventoryId, 
                    employee_id = @EmployeeId, 
                    rental_date = @RentalDate, 
                    return_date = @ReturnDate, 
                    deposit_amount = @DepositAmount
                WHERE rental_id = @RentalId";
            
            var rowsAffected = await connection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "DELETE FROM inventory_rentals WHERE rental_id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<InventoryRental>> GetByClientIdAsync(int clientId)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT ir.*, c.full_name as ClientName, i.name as InventoryName, e.full_name as EmployeeName
                FROM inventory_rentals ir
                JOIN clients c ON ir.client_id = c.client_id
                JOIN inventory i ON ir.inventory_id = i.inventory_id
                JOIN employees e ON ir.employee_id = e.employee_id
                WHERE ir.client_id = @ClientId
                ORDER BY ir.rental_date DESC";
            
            return await connection.QueryAsync<InventoryRental>(sql, new { ClientId = clientId });
        }

        public async Task<IEnumerable<InventoryRental>> GetPendingReturnsAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT ir.*, c.full_name as ClientName, i.name as InventoryName, e.full_name as EmployeeName
                FROM inventory_rentals ir
                JOIN clients c ON ir.client_id = c.client_id
                JOIN inventory i ON ir.inventory_id = i.inventory_id
                JOIN employees e ON ir.employee_id = e.employee_id
                WHERE ir.return_date IS NULL
                ORDER BY ir.rental_date ASC";
            
            return await connection.QueryAsync<InventoryRental>(sql);
        }

        public async Task<IEnumerable<InventoryRental>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT ir.*, c.full_name as ClientName, i.name as InventoryName, e.full_name as EmployeeName
                FROM inventory_rentals ir
                JOIN clients c ON ir.client_id = c.client_id
                JOIN inventory i ON ir.inventory_id = i.inventory_id
                JOIN employees e ON ir.employee_id = e.employee_id
                WHERE ir.rental_date BETWEEN @StartDate AND @EndDate
                ORDER BY ir.rental_date DESC";
            
            return await connection.QueryAsync<InventoryRental>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<bool> ReturnInventoryAsync(int rentalId, DateTime returnDate)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "UPDATE inventory_rentals SET return_date = @ReturnDate WHERE rental_id = @RentalId";
            var rowsAffected = await connection.ExecuteAsync(sql, new { 
                RentalId = rentalId, 
                ReturnDate = returnDate 
            });
            return rowsAffected > 0;
        }

        public async Task<decimal> GetTotalDepositsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "SELECT COALESCE(SUM(deposit_amount), 0) FROM inventory_rentals WHERE 1=1";
            var parameters = new DynamicParameters();

            if (startDate.HasValue)
            {
                sql += " AND rental_date >= @StartDate";
                parameters.Add("StartDate", startDate.Value);
            }

            if (endDate.HasValue)
            {
                sql += " AND rental_date <= @EndDate";
                parameters.Add("EndDate", endDate.Value);
            }

            return await connection.QuerySingleAsync<decimal>(sql, parameters);
        }
    }
}
