using AquaparkApp.Models;
using Dapper;

namespace AquaparkApp.DAL
{
    public class AttractionRepository : BaseRepository<Attraction>
    {
        protected override string GetTableName() => "Attractions";

        public async Task<IEnumerable<Attraction>> GetActiveAttractionsAsync()
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<Attraction>(
                "SELECT * FROM Attractions WHERE IsActive = true ORDER BY Name");
        }

        public async Task<IEnumerable<Attraction>> GetByCategoryAsync(string category)
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<Attraction>(
                "SELECT * FROM Attractions WHERE Category = @Category AND IsActive = true ORDER BY Name", 
                new { Category = category });
        }

        public async Task<IEnumerable<Attraction>> SearchByNameAsync(string searchTerm)
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<Attraction>(
                "SELECT * FROM Attractions WHERE Name ILIKE @SearchTerm AND IsActive = true ORDER BY Name", 
                new { SearchTerm = $"%{searchTerm}%" });
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<string>(
                "SELECT DISTINCT Category FROM Attractions WHERE IsActive = true ORDER BY Category");
        }
    }
}
