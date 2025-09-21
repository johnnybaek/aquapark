using System;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApp.Models
{
    /// <summary>
    /// Модель услуги
    /// </summary>
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }
        
        [Required(ErrorMessage = "Название услуги обязательно")]
        [StringLength(100, ErrorMessage = "Название услуги не должно превышать 100 символов")]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Цена обязательна")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        public decimal Price { get; set; }
        
        // Навигационные свойства
        public virtual ICollection<ClientService> ClientServices { get; set; } = new List<ClientService>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        
        // Вычисляемые свойства
        public string DisplayName => $"{Name} - {Price:C}";
        public string ShortDescription => Description?.Length > 50 ? Description.Substring(0, 50) + "..." : Description ?? "";
    }
}
