using System;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApp.Models
{
    /// <summary>
    /// Модель посещения
    /// </summary>
    public class Visit
    {
        [Key]
        public int VisitId { get; set; }
        
        [Required]
        public int ClientId { get; set; }
        
        [Required]
        public int TicketId { get; set; }
        
        [Required(ErrorMessage = "Время входа обязательно")]
        public DateTime EntryTime { get; set; }
        
        public DateTime? ExitTime { get; set; }
        
        public int? ZoneId { get; set; }
        
        // Навигационные свойства
        public virtual Client Client { get; set; } = null!;
        public virtual Ticket Ticket { get; set; } = null!;
        public virtual Zone? Zone { get; set; }
        
        // Вычисляемые свойства
        public bool IsActive => !ExitTime.HasValue;
        public TimeSpan? Duration => ExitTime?.Subtract(EntryTime) ?? DateTime.Now.Subtract(EntryTime);
        public string Status => IsActive ? "В аквапарке" : "Покинул";
        public string DisplayName => $"{Client?.FullName} - {EntryTime:dd.MM.yyyy HH:mm}";
        public string DurationDisplay => Duration?.TotalHours > 1 
            ? $"{Duration?.TotalHours:F1} ч." 
            : $"{Duration?.TotalMinutes:F0} мин.";
        public string ZoneDisplayName => Zone?.ZoneName ?? "Не указана";
    }
}
