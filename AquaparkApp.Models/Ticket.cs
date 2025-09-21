using System;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApp.Models
{
    /// <summary>
    /// Модель билета
    /// </summary>
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        
        [Required]
        public int ClientId { get; set; }
        
        [Required(ErrorMessage = "Тип билета обязателен")]
        [StringLength(50, ErrorMessage = "Тип билета не должен превышать 50 символов")]
        public string TicketType { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Цена обязательна")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        public decimal Price { get; set; }
        
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        
        [Required(ErrorMessage = "Дата действия обязательна")]
        public DateTime ValidUntil { get; set; }
        
        // Навигационные свойства
        public virtual Client Client { get; set; } = null!;
        public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        
        // Вычисляемые свойства
        public bool IsValid => DateTime.Now <= ValidUntil;
        public string Status => IsValid ? "Действителен" : "Истек";
        public int DaysUntilExpiry => (ValidUntil - DateTime.Now).Days;
    }
}