using Dapper;
using System.Data;

namespace AquaparkApp.DAL
{
    public abstract class BaseRepository<T> where T : class
    {
        protected IDbConnection GetConnection()
        {
            return DatabaseConnection.GetConnection();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using var connection = GetConnection();
            return await connection.QueryAsync<T>($"SELECT * FROM {GetTableName()}");
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(
                $"SELECT * FROM {GetTableName()} WHERE Id = @Id", 
                new { Id = id });
        }

        public virtual async Task<int> CreateAsync(T entity)
        {
            using var connection = GetConnection();
            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "Id" && p.CanWrite)
                .Select(p => p.Name);
            
            var columns = string.Join(", ", properties);
            var values = string.Join(", ", properties.Select(p => $"@{p}"));
            
            var sql = $"INSERT INTO {GetTableName()} ({columns}) VALUES ({values}) RETURNING Id";
            return await connection.QuerySingleAsync<int>(sql, entity);
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            using var connection = GetConnection();
            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "Id" && p.CanWrite)
                .Select(p => $"{p.Name} = @{p.Name}");
            
            var setClause = string.Join(", ", properties);
            var sql = $"UPDATE {GetTableName()} SET {setClause} WHERE Id = @Id";
            
            var rowsAffected = await connection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            using var connection = GetConnection();
            var sql = $"DELETE FROM {GetTableName()} WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            using var connection = GetConnection();
            var offset = (pageNumber - 1) * pageSize;
            var sql = $"SELECT * FROM {GetTableName()} ORDER BY Id OFFSET @Offset LIMIT @PageSize";
            return await connection.QueryAsync<T>(sql, new { Offset = offset, PageSize = pageSize });
        }

        public virtual async Task<int> GetCountAsync()
        {
            using var connection = GetConnection();
            var sql = $"SELECT COUNT(*) FROM {GetTableName()}";
            return await connection.QuerySingleAsync<int>(sql);
        }

        protected abstract string GetTableName();
    }
}
