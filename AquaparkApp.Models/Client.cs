using System;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApp.Models
{
    /// <summary>
    /// Модель клиента аквапарка
    /// </summary>
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        
        [Required(ErrorMessage = "ФИО обязательно")]
        [StringLength(100, ErrorMessage = "ФИО не должно превышать 100 символов")]
        public string FullName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Телефон обязателен")]
        [StringLength(20, ErrorMessage = "Телефон не должен превышать 20 символов")]
        public string Phone { get; set; } = string.Empty;
        
        [StringLength(100, ErrorMessage = "Email не должен превышать 100 символов")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Дата рождения обязательна")]
        public DateTime BirthDate { get; set; }
        
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        
        // Навигационные свойства
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
        public virtual ICollection<InventoryRental> InventoryRentals { get; set; } = new List<InventoryRental>();
        public virtual ICollection<ClientService> ClientServices { get; set; } = new List<ClientService>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        
        // Вычисляемые свойства
        public int Age => DateTime.Now.Year - BirthDate.Year;
        public string DisplayName => $"{FullName} ({Phone})";
    }
}
