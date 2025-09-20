using AquaparkApp.DAL;
using AquaparkApp.Models;

namespace AquaparkApp.BLL
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;
        private readonly AttractionRepository _attractionRepository;

        public CartService(string connectionString)
        {
            _cartRepository = new CartRepository();
            _attractionRepository = new AttractionRepository();
        }

        public async Task<ShoppingCart> GetOrCreateCartAsync(int userId)
        {
            return await _cartRepository.GetOrCreateCartAsync(userId);
        }

        public async Task<ShoppingCart?> GetCartAsync(int userId)
        {
            return await _cartRepository.GetByUserIdAsync(userId);
        }

        public async Task<bool> AddItemToCartAsync(int userId, int attractionId, DateTime visitDate, TimeSpan visitTime, int quantity)
        {
            var attraction = await _attractionRepository.GetByIdAsync(attractionId);
            if (attraction == null || !attraction.IsActive)
            {
                throw new ArgumentException("Аттракцион не найден или неактивен");
            }

            var cart = await _cartRepository.GetOrCreateCartAsync(userId);

            var cartItem = new CartItem
            {
                AttractionId = attractionId,
                VisitDate = visitDate,
                VisitTime = visitTime,
                Quantity = quantity,
                UnitPrice = attraction.Price,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _cartRepository.AddItemToCartAsync(cart.Id, cartItem);
        }

        public async Task<bool> UpdateCartItemAsync(int userId, int itemId, int quantity, DateTime? visitDate = null, TimeSpan? visitTime = null)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null) return false;

            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null) return false;

            item.Quantity = quantity;
            if (visitDate.HasValue) item.VisitDate = visitDate.Value;
            if (visitTime.HasValue) item.VisitTime = visitTime.Value;

            return await _cartRepository.UpdateCartItemAsync(item);
        }

        public async Task<bool> RemoveItemFromCartAsync(int userId, int itemId)
        {
            return await _cartRepository.RemoveItemFromCartAsync(itemId);
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null) return false;

            return await _cartRepository.ClearCartAsync(cart.Id);
        }

        public async Task<int> GetCartItemsCountAsync(int userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null) return 0;

            return await _cartRepository.GetCartItemsCountAsync(cart.Id);
        }

        public async Task<decimal> GetCartTotalAsync(int userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null) return 0;

            return await _cartRepository.GetCartTotalAsync(cart.Id);
        }

        public async Task<bool> ValidateCartAsync(int userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null || !cart.Items.Any()) return false;

            foreach (var item in cart.Items)
            {
                var attraction = await _attractionRepository.GetByIdAsync(item.AttractionId);
                if (attraction == null || !attraction.IsActive)
                {
                    return false;
                }

                // Проверяем возрастные ограничения
                // Здесь можно добавить логику проверки возраста пользователя
                // и сравнения с MinAge/MaxAge аттракциона

                // Проверяем, что дата посещения не в прошлом
                if (item.VisitDate < DateTime.Today)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<PromoCodeApplication> ApplyPromoCodeAsync(int userId, string promoCode)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null || !cart.Items.Any())
            {
                return new PromoCodeApplication
                {
                    IsValid = false,
                    ErrorMessage = "Корзина пуста"
                };
            }

            // Здесь должна быть логика проверки промокода
            // Пока возвращаем заглушку
            var result = new PromoCodeApplication
            {
                PromoCode = promoCode,
                IsValid = false,
                ErrorMessage = "Промокод не найден или недействителен"
            };

            // Пример применения скидки 10%
            if (promoCode == "WELCOME10")
            {
                result.IsValid = true;
                result.DiscountPercent = 10;
                result.FinalAmount = cart.TotalAmount * 0.9m;
                result.ErrorMessage = string.Empty;
            }

            return result;
        }
    }
}
