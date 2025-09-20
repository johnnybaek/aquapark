using AquaparkApp.Models;
using Dapper;
using System.Data;

namespace AquaparkApp.DAL
{
    public class CartRepository : BaseRepository<ShoppingCart>
    {
        protected override string GetTableName() => "ShoppingCarts";

        public async Task<ShoppingCart?> GetByUserIdAsync(int userId)
        {
            using var connection = GetConnection();
            var cart = await connection.QueryFirstOrDefaultAsync<ShoppingCart>(
                "SELECT * FROM ShoppingCarts WHERE UserId = @UserId", 
                new { UserId = userId });

            if (cart != null)
            {
                cart.Items = await GetCartItemsAsync(cart.Id);
            }

            return cart;
        }

        public async Task<ShoppingCart> GetOrCreateCartAsync(int userId)
        {
            var cart = await GetByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                cart.Id = await CreateAsync(cart);
                cart.Items = new List<CartItem>();
            }
            return cart;
        }

        public async Task<List<CartItem>> GetCartItemsAsync(int cartId)
        {
            using var connection = GetConnection();
            var sql = @"
                SELECT ci.*, a.Name as AttractionName, a.Price as AttractionPrice
                FROM CartItems ci
                LEFT JOIN Attractions a ON ci.AttractionId = a.Id
                WHERE ci.CartId = @CartId
                ORDER BY ci.CreatedAt DESC";
            
            var items = await connection.QueryAsync<CartItem>(sql, new { CartId = cartId });
            return items.ToList();
        }

        public async Task<bool> AddItemToCartAsync(int cartId, CartItem item)
        {
            using var connection = GetConnection();
            var sql = @"
                INSERT INTO CartItems (CartId, AttractionId, VisitDate, VisitTime, Quantity, UnitPrice, CreatedAt, UpdatedAt)
                VALUES (@CartId, @AttractionId, @VisitDate, @VisitTime, @Quantity, @UnitPrice, @CreatedAt, @UpdatedAt)
                RETURNING Id";

            item.CartId = cartId;
            item.CreatedAt = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;

            var itemId = await connection.QuerySingleAsync<int>(sql, item);
            item.Id = itemId;

            // Обновляем время изменения корзины
            await UpdateCartTimestampAsync(cartId);

            return true;
        }

        public async Task<bool> UpdateCartItemAsync(CartItem item)
        {
            using var connection = GetConnection();
            var sql = @"
                UPDATE CartItems 
                SET Quantity = @Quantity, VisitDate = @VisitDate, VisitTime = @VisitTime, 
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";

            item.UpdatedAt = DateTime.UtcNow;
            var rowsAffected = await connection.ExecuteAsync(sql, item);

            if (rowsAffected > 0)
            {
                await UpdateCartTimestampAsync(item.CartId);
            }

            return rowsAffected > 0;
        }

        public async Task<bool> RemoveItemFromCartAsync(int itemId)
        {
            using var connection = GetConnection();
            var sql = "DELETE FROM CartItems WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = itemId });
            return rowsAffected > 0;
        }

        public async Task<bool> ClearCartAsync(int cartId)
        {
            using var connection = GetConnection();
            var sql = "DELETE FROM CartItems WHERE CartId = @CartId";
            var rowsAffected = await connection.ExecuteAsync(sql, new { CartId = cartId });
            
            if (rowsAffected > 0)
            {
                await UpdateCartTimestampAsync(cartId);
            }

            return rowsAffected > 0;
        }

        public async Task<bool> UpdateCartTimestampAsync(int cartId)
        {
            using var connection = GetConnection();
            var sql = "UPDATE ShoppingCarts SET UpdatedAt = @UpdatedAt WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { 
                Id = cartId, 
                UpdatedAt = DateTime.UtcNow 
            });
            return rowsAffected > 0;
        }

        public async Task<int> GetCartItemsCountAsync(int cartId)
        {
            using var connection = GetConnection();
            var sql = "SELECT COUNT(*) FROM CartItems WHERE CartId = @CartId";
            return await connection.QuerySingleAsync<int>(sql, new { CartId = cartId });
        }

        public async Task<decimal> GetCartTotalAsync(int cartId)
        {
            using var connection = GetConnection();
            var sql = "SELECT COALESCE(SUM(UnitPrice * Quantity), 0) FROM CartItems WHERE CartId = @CartId";
            return await connection.QuerySingleAsync<decimal>(sql, new { CartId = cartId });
        }
    }
}
