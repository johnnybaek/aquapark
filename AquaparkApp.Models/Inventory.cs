using System;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApp.Models
{
    /// <summary>
    /// Модель инвентаря
    /// </summary>
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        
        [Required(ErrorMessage = "Название инвентаря обязательно")]
        [StringLength(100, ErrorMessage = "Название инвентаря не должно превышать 100 символов")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Количество обязательно")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество не может быть отрицательным")]
        public int Quantity { get; set; }
        
        [Required(ErrorMessage = "Статус обязателен")]
        [StringLength(50, ErrorMessage = "Статус не должен превышать 50 символов")]
        public string Status { get; set; } = string.Empty;
        
        public int? ZoneId { get; set; }
        
        public int? ResponsibleEmployeeId { get; set; }
        
        // Навигационные свойства
        public virtual Zone? Zone { get; set; }
        public virtual Employee? ResponsibleEmployee { get; set; }
        public virtual ICollection<InventoryRental> InventoryRentals { get; set; } = new List<InventoryRental>();
        
        // Вычисляемые свойства
        public string DisplayName => $"{Name} (количество: {Quantity})";
        public string StatusDisplayName => Status switch
        {
            "исправно" => "✅ Исправно",
            "на ремонте" => "🔧 На ремонте",
            "неисправно" => "❌ Неисправно",
            _ => Status
        };
        public string ZoneDisplayName => Zone?.ZoneName ?? "Не назначена";
        public string ResponsibleEmployeeDisplayName => ResponsibleEmployee?.FullName ?? "Не назначен";
        public bool IsAvailable => Status == "исправно" && Quantity > 0;
    }
}
