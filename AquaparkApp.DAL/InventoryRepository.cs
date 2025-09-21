using Dapper;
using AquaparkApp.Models;
using System.Data;

namespace AquaparkApp.DAL
{
    /// <summary>
    /// Репозиторий для работы с инвентарем
    /// </summary>
    public class InventoryRepository : BaseRepository<Inventory>
    {
        public InventoryRepository() : base("inventory") { }

        protected override string GetTableName()
        {
            return "inventory";
        }

        public override async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT i.*, z.zone_name as ZoneName, e.full_name as ResponsibleEmployeeName
                FROM inventory i
                LEFT JOIN zones z ON i.zone_id = z.zone_id
                LEFT JOIN employees e ON i.responsible_employee_id = e.employee_id
                ORDER BY i.name";
            
            return await connection.QueryAsync<Inventory>(sql);
        }

        public override async Task<Inventory?> GetByIdAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT i.*, z.zone_name as ZoneName, e.full_name as ResponsibleEmployeeName
                FROM inventory i
                LEFT JOIN zones z ON i.zone_id = z.zone_id
                LEFT JOIN employees e ON i.responsible_employee_id = e.employee_id
                WHERE i.inventory_id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<Inventory>(sql, new { Id = id });
        }

        public override async Task<int> CreateAsync(Inventory entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                INSERT INTO inventory (name, quantity, status, zone_id, responsible_employee_id)
                VALUES (@Name, @Quantity, @Status, @ZoneId, @ResponsibleEmployeeId)
                RETURNING inventory_id";
            
            return await connection.QuerySingleAsync<int>(sql, entity);
        }

        public override async Task<bool> UpdateAsync(Inventory entity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                UPDATE inventory 
                SET name = @Name, 
                    quantity = @Quantity, 
                    status = @Status, 
                    zone_id = @ZoneId, 
                    responsible_employee_id = @ResponsibleEmployeeId
                WHERE inventory_id = @InventoryId";
            
            var rowsAffected = await connection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "DELETE FROM inventory WHERE inventory_id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Inventory>> GetByZoneIdAsync(int zoneId)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT i.*, z.zone_name as ZoneName, e.full_name as ResponsibleEmployeeName
                FROM inventory i
                LEFT JOIN zones z ON i.zone_id = z.zone_id
                LEFT JOIN employees e ON i.responsible_employee_id = e.employee_id
                WHERE i.zone_id = @ZoneId
                ORDER BY i.name";
            
            return await connection.QueryAsync<Inventory>(sql, new { ZoneId = zoneId });
        }

        public async Task<IEnumerable<Inventory>> GetByStatusAsync(string status)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT i.*, z.zone_name as ZoneName, e.full_name as ResponsibleEmployeeName
                FROM inventory i
                LEFT JOIN zones z ON i.zone_id = z.zone_id
                LEFT JOIN employees e ON i.responsible_employee_id = e.employee_id
                WHERE i.status = @Status
                ORDER BY i.name";
            
            return await connection.QueryAsync<Inventory>(sql, new { Status = status });
        }

        public async Task<IEnumerable<Inventory>> GetAvailableInventoryAsync()
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = @"
                SELECT i.*, z.zone_name as ZoneName, e.full_name as ResponsibleEmployeeName
                FROM inventory i
                LEFT JOIN zones z ON i.zone_id = z.zone_id
                LEFT JOIN employees e ON i.responsible_employee_id = e.employee_id
                WHERE i.status = 'исправно' AND i.quantity > 0
                ORDER BY i.name";
            
            return await connection.QueryAsync<Inventory>(sql);
        }

        public async Task<bool> UpdateQuantityAsync(int inventoryId, int newQuantity)
        {
            using var connection = DatabaseConnection.GetConnection();
            var sql = "UPDATE inventory SET quantity = @Quantity WHERE inventory_id = @InventoryId";
            var rowsAffected = await connection.ExecuteAsync(sql, new { 
                InventoryId = inventoryId, 
                Quantity = newQuantity 
            });
            return rowsAffected > 0;
        }
    }
}
