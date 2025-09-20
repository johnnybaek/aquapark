using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApp.Models
{
    /// <summary>
    /// Модель корзины покупок
    /// </summary>
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        
        // Вычисляемые свойства
        public decimal SubTotal => Items.Sum(item => item.TotalPrice);
        public decimal TaxAmount => SubTotal * 0.1m; // 10% налог
        public decimal TotalAmount => SubTotal + TaxAmount;
        public int TotalItems => Items.Sum(item => item.Quantity);
        
        // Навигационные свойства
        public User? User { get; set; }
    }

    /// <summary>
    /// Элемент корзины
    /// </summary>
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int AttractionId { get; set; }
        
        [Required(ErrorMessage = "Дата посещения обязательна")]
        public DateTime VisitDate { get; set; }
        
        [Required(ErrorMessage = "Время посещения обязательно")]
        public TimeSpan VisitTime { get; set; }
        
        [Range(1, 10, ErrorMessage = "Количество должно быть от 1 до 10")]
        public int Quantity { get; set; } = 1;
        
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Навигационные свойства
        public ShoppingCart? Cart { get; set; }
        public Attraction? Attraction { get; set; }
    }

    /// <summary>
    /// Модель для применения промокода
    /// </summary>
    public class PromoCodeApplication
    {
        public string PromoCode { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public decimal FinalAmount { get; set; }
    }

    /// <summary>
    /// Модель для оформления заказа
    /// </summary>
    public class CheckoutRequest
    {
        [Required(ErrorMessage = "Способ оплаты обязателен")]
        public string PaymentMethod { get; set; } = string.Empty;
        
        public string PromoCode { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Комментарий не должен превышать 500 символов")]
        public string Notes { get; set; } = string.Empty;
        
        public bool AgreeToTerms { get; set; }
        
        // Информация о доставке (если нужна)
        public string DeliveryAddress { get; set; } = string.Empty;
        public string DeliveryPhone { get; set; } = string.Empty;
    }

    /// <summary>
    /// Модель для результата оформления заказа
    /// </summary>
    public class CheckoutResult
    {
        public bool Success { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string PaymentUrl { get; set; } = string.Empty; // Для онлайн оплаты
        public string ErrorMessage { get; set; } = string.Empty;
        public List<string> Warnings { get; set; } = new List<string>();
    }
}
