using AquaparkApp.DAL;
using AquaparkApp.Models;

namespace AquaparkApp.BLL
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly TicketRepository _ticketRepository;
        private readonly PromotionRepository _promotionRepository;

        public OrderService()
        {
            _orderRepository = new OrderRepository();
            _ticketRepository = new TicketRepository();
            _promotionRepository = new PromotionRepository();
        }

        public async Task<Order?> CreateOrderAsync(int userId, List<Ticket> tickets, string? promoCode = null)
        {
            if (!tickets.Any()) return null;

            var order = new Order
            {
                UserId = userId,
                OrderNumber = await _orderRepository.GenerateOrderNumberAsync(),
                TotalAmount = tickets.Sum(t => t.TotalPrice),
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            // Применяем промокод если указан
            if (!string.IsNullOrEmpty(promoCode))
            {
                var promotion = await _promotionRepository.GetByPromoCodeAsync(promoCode);
                if (promotion != null)
                {
                    order.DiscountAmount = CalculateDiscount(promotion, order.TotalAmount);
                    order.FinalAmount = order.TotalAmount - order.DiscountAmount;
                    await _promotionRepository.IncrementUsageCountAsync(promotion.Id);
                }
                else
                {
                    order.FinalAmount = order.TotalAmount;
                }
            }
            else
            {
                order.FinalAmount = order.TotalAmount;
            }

            try
            {
                var orderId = await _orderRepository.CreateAsync(order);
                order.Id = orderId;

                // Обновляем билеты с ID заказа
                foreach (var ticket in tickets)
                {
                    // В реальном приложении здесь нужно обновить билеты с OrderId
                    await _ticketRepository.UpdateAsync(ticket);
                }

                return order;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _orderRepository.GetByUserIdAsync(userId);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<Order?> GetOrderByNumberAsync(string orderNumber)
        {
            return await _orderRepository.GetByOrderNumberAsync(orderNumber);
        }

        public async Task<bool> ProcessPaymentAsync(int orderId, string paymentMethod)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;

            order.Status = "Paid";
            order.PaymentMethod = paymentMethod;
            order.PaidAt = DateTime.UtcNow;

            // Подтверждаем все билеты в заказе
            var tickets = await _ticketRepository.GetByOrderIdAsync(orderId);
            foreach (var ticket in tickets)
            {
                await _ticketRepository.UpdateStatusAsync(ticket.Id, "Confirmed");
            }

            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> CompleteOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;

            order.Status = "Completed";
            order.CompletedAt = DateTime.UtcNow;

            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;

            order.Status = "Cancelled";

            // Отменяем все билеты в заказе
            var tickets = await _ticketRepository.GetByOrderIdAsync(orderId);
            foreach (var ticket in tickets)
            {
                await _ticketRepository.UpdateStatusAsync(ticket.Id, "Cancelled");
            }

            return await _orderRepository.UpdateAsync(order);
        }

        private decimal CalculateDiscount(Promotion promotion, decimal totalAmount)
        {
            if (totalAmount < promotion.MinOrderAmount)
                return 0;

            if (promotion.FixedDiscountAmount.HasValue)
                return Math.Min(promotion.FixedDiscountAmount.Value, totalAmount);

            return totalAmount * (promotion.DiscountPercent / 100);
        }
    }
}
