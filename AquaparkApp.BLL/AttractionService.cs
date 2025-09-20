using AquaparkApp.DAL;
using AquaparkApp.Models;

namespace AquaparkApp.BLL
{
    public class AttractionService
    {
        private readonly AttractionRepository _attractionRepository;

        public AttractionService()
        {
            _attractionRepository = new AttractionRepository();
        }

        public AttractionService(string connectionString)
        {
            _attractionRepository = new AttractionRepository(connectionString);
        }

        public async Task<IEnumerable<Attraction>> GetAllAttractionsAsync()
        {
            return await _attractionRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Attraction>> GetActiveAttractionsAsync()
        {
            return await _attractionRepository.GetActiveAttractionsAsync();
        }

        public async Task<Attraction?> GetAttractionByIdAsync(int id)
        {
            return await _attractionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Attraction>> GetAttractionsByCategoryAsync(string category)
        {
            return await _attractionRepository.GetByCategoryAsync(category);
        }

        public async Task<IEnumerable<Attraction>> SearchAttractionsAsync(string searchTerm)
        {
            return await _attractionRepository.SearchByNameAsync(searchTerm);
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            return await _attractionRepository.GetCategoriesAsync();
        }

        public async Task<bool> CreateAttractionAsync(Attraction attraction)
        {
            attraction.CreatedAt = DateTime.UtcNow;
            attraction.IsActive = true;
            
            try
            {
                await _attractionRepository.CreateAsync(attraction);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAttractionAsync(Attraction attraction)
        {
            attraction.UpdatedAt = DateTime.UtcNow;
            return await _attractionRepository.UpdateAsync(attraction);
        }

        public async Task<bool> DeleteAttractionAsync(int id)
        {
            return await _attractionRepository.DeleteAsync(id);
        }

        public async Task<bool> DeactivateAttractionAsync(int id)
        {
            var attraction = await _attractionRepository.GetByIdAsync(id);
            if (attraction == null) return false;

            attraction.IsActive = false;
            attraction.UpdatedAt = DateTime.UtcNow;
            return await _attractionRepository.UpdateAsync(attraction);
        }
    }
}
