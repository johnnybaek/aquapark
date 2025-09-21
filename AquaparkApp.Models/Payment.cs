using System;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApp.Models
{
    /// <summary>
    /// Модель оплаты
    /// </summary>
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        
        [Required]
        public int ClientId { get; set; }
        
        public int? TicketId { get; set; }
        
        public int? ServiceId { get; set; }
        
        [Required]
        public int EmployeeId { get; set; }
        
        [Required(ErrorMessage = "Сумма обязательна")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сумма должна быть больше 0")]
        public decimal Amount { get; set; }
        
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        
        [Required(ErrorMessage = "Способ оплаты обязателен")]
        [StringLength(20, ErrorMessage = "Способ оплаты не должен превышать 20 символов")]
        public string PaymentMethod { get; set; } = string.Empty;
        
        // Навигационные свойства
        public virtual Client Client { get; set; } = null!;
        public virtual Ticket? Ticket { get; set; }
        public virtual Service? Service { get; set; }
        public virtual Employee Employee { get; set; } = null!;
        
        // Вычисляемые свойства
        public string PaymentType => TicketId.HasValue ? "Билет" : ServiceId.HasValue ? "Услуга" : "Прочее";
        public string DisplayName => $"{Client?.FullName} - {Amount:C} ({PaymentDate:dd.MM.yyyy})";
        public string PaymentMethodDisplayName => PaymentMethod switch
        {
            "карта" => "💳 Карта",
            "наличные" => "💵 Наличные",
            "онлайн" => "🌐 Онлайн",
            _ => PaymentMethod
        };
        public string RelatedItemName => Ticket?.TicketType ?? Service?.Name ?? "Не указано";
    }
}
