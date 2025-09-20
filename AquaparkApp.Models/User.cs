using System;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApp.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [StringLength(50, ErrorMessage = "Имя пользователя не должно превышать 50 символов")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Пароль обязателен")]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(50, ErrorMessage = "Фамилия не должна превышать 50 символов")]
        public string LastName { get; set; } = string.Empty;
        
        [Phone(ErrorMessage = "Некорректный формат телефона")]
        public string Phone { get; set; } = string.Empty;
        
        public DateTime DateOfBirth { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Дополнительные поля для отчетов
        public int TotalOrders { get; set; } = 0;
        public decimal TotalSpent { get; set; } = 0;
        public string AvatarPath { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string PreferredLanguage { get; set; } = "ru";
        
        // Навигационные свойства
        public string FullName => $"{FirstName} {LastName}";
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
    }
}
