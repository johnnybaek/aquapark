using System;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApp.Models
{
    /// <summary>
    /// Модель аренды инвентаря
    /// </summary>
    public class InventoryRental
    {
        [Key]
        public int RentalId { get; set; }
        
        [Required]
        public int ClientId { get; set; }
        
        [Required]
        public int InventoryId { get; set; }
        
        [Required]
        public int EmployeeId { get; set; }
        
        public DateTime RentalDate { get; set; } = DateTime.Now;
        
        public DateTime? ReturnDate { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Сумма залога не может быть отрицательной")]
        public decimal? DepositAmount { get; set; }
        
        // Навигационные свойства
        public virtual Client Client { get; set; } = null!;
        public virtual Inventory Inventory { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
        
        // Вычисляемые свойства
        public bool IsReturned => ReturnDate.HasValue;
        public TimeSpan? RentalDuration => ReturnDate?.Subtract(RentalDate) ?? DateTime.Now.Subtract(RentalDate);
        public string Status => IsReturned ? "Возвращен" : "В аренде";
        public string DisplayName => $"{Client?.FullName} - {Inventory?.Name} ({RentalDate:dd.MM.yyyy})";
        public string DurationDisplay => RentalDuration?.TotalHours > 1 
            ? $"{RentalDuration?.TotalHours:F1} ч." 
            : $"{RentalDuration?.TotalMinutes:F0} мин.";
    }
}
