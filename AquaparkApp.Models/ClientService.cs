using System;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApp.Models
{
    /// <summary>
    /// Модель обслуживания клиентов
    /// </summary>
    public class ClientService
    {
        [Key]
        public int ServiceLogId { get; set; }
        
        [Required]
        public int ClientId { get; set; }
        
        [Required]
        public int ServiceId { get; set; }
        
        [Required]
        public int EmployeeId { get; set; }
        
        public DateTime ServiceDate { get; set; } = DateTime.Now;
        
        [Required(ErrorMessage = "Количество обязательно")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 0")]
        public int Quantity { get; set; } = 1;
        
        // Навигационные свойства
        public virtual Client Client { get; set; } = null!;
        public virtual Service Service { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
        
        // Вычисляемые свойства
        public decimal TotalAmount => Service?.Price * Quantity ?? 0;
        public string DisplayName => $"{Client?.FullName} - {Service?.Name} ({ServiceDate:dd.MM.yyyy})";
        public string ServiceDisplayName => $"{Service?.Name} x{Quantity}";
        public string EmployeeDisplayName => Employee?.FullName ?? "Не указан";
    }
}
