using System;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApp.Models
{
    /// <summary>
    /// Модель сотрудника
    /// </summary>
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        
        [Required(ErrorMessage = "ФИО обязательно")]
        [StringLength(100, ErrorMessage = "ФИО не должно превышать 100 символов")]
        public string FullName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Должность обязательна")]
        [StringLength(50, ErrorMessage = "Должность не должна превышать 50 символов")]
        public string Position { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Телефон обязателен")]
        [StringLength(20, ErrorMessage = "Телефон не должен превышать 20 символов")]
        public string Phone { get; set; } = string.Empty;
        
        public DateTime HireDate { get; set; } = DateTime.Now;
        
        public int? ZoneId { get; set; }
        
        // Навигационные свойства
        public virtual Zone? Zone { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public virtual ICollection<Inventory> ResponsibleInventory { get; set; } = new List<Inventory>();
        public virtual ICollection<InventoryRental> InventoryRentals { get; set; } = new List<InventoryRental>();
        public virtual ICollection<ClientService> ClientServices { get; set; } = new List<ClientService>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        
        // Вычисляемые свойства
        public int YearsOfService => DateTime.Now.Year - HireDate.Year;
        public string DisplayName => $"{FullName} - {Position}";
        public string ZoneDisplayName => Zone?.ZoneName ?? "Не назначена";
    }
}
