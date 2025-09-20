using AquaparkApp.Models;
using Dapper;

namespace AquaparkApp.DAL
{
    public class PromotionRepository : BaseRepository<Promotion>
    {
        protected override string GetTableName() => "Promotions";

        public async Task<IEnumerable<Promotion>> GetActivePromotionsAsync()
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<Promotion>(
                @"SELECT * FROM Promotions 
                  WHERE IsActive = true 
                  AND StartDate <= @Now 
                  AND EndDate >= @Now
                  ORDER BY StartDate", 
                new { Now = DateTime.UtcNow });
        }

        public async Task<Promotion?> GetByPromoCodeAsync(string promoCode)
        {
            using var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<Promotion>(
                @"SELECT * FROM Promotions 
                  WHERE PromoCode = @PromoCode 
                  AND IsActive = true 
                  AND StartDate <= @Now 
                  AND EndDate >= @Now
                  AND (UsageLimit = 0 OR UsedCount < UsageLimit)", 
                new { PromoCode = promoCode, Now = DateTime.UtcNow });
        }

        public async Task<bool> IncrementUsageCountAsync(int promotionId)
        {
            using var connection = GetConnection();
            var sql = "UPDATE Promotions SET UsedCount = UsedCount + 1 WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = promotionId });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Promotion>> GetExpiredPromotionsAsync()
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<Promotion>(
                "SELECT * FROM Promotions WHERE EndDate < @Now ORDER BY EndDate DESC", 
                new { Now = DateTime.UtcNow });
        }
    }
}
