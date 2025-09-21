using System;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApp.Models
{
    /// <summary>
    /// Модель зоны аквапарка
    /// </summary>
    public class Zone
    {
        [Key]
        public int ZoneId { get; set; }
        
        [Required(ErrorMessage = "Название зоны обязательно")]
        [StringLength(100, ErrorMessage = "Название зоны не должно превышать 100 символов")]
        public string ZoneName { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Вместимость обязательна")]
        [Range(1, int.MaxValue, ErrorMessage = "Вместимость должна быть больше 0")]
        public int Capacity { get; set; }
        
        // Навигационные свойства
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public virtual ICollection<Inventory> Inventory { get; set; } = new List<Inventory>();
        public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
        
        // Вычисляемые свойства
        public string DisplayName => $"{ZoneName} (вместимость: {Capacity})";
        public string ShortDescription => Description?.Length > 50 ? Description.Substring(0, 50) + "..." : Description ?? "";
    }
}
