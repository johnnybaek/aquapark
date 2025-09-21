using System;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApp.Models
{
    /// <summary>
    /// Модель расписания работы
    /// </summary>
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        
        [Required]
        public int EmployeeId { get; set; }
        
        [Required]
        public int ZoneId { get; set; }
        
        [Required(ErrorMessage = "Дата работы обязательна")]
        public DateTime WorkDate { get; set; }
        
        [Required(ErrorMessage = "Время начала смены обязательно")]
        public TimeSpan ShiftStart { get; set; }
        
        [Required(ErrorMessage = "Время окончания смены обязательно")]
        public TimeSpan ShiftEnd { get; set; }
        
        // Навигационные свойства
        public virtual Employee Employee { get; set; } = null!;
        public virtual Zone Zone { get; set; } = null!;
        
        // Вычисляемые свойства
        public TimeSpan Duration => ShiftEnd - ShiftStart;
        public string DisplayName => $"{Employee?.FullName} - {Zone?.ZoneName} ({WorkDate:dd.MM.yyyy})";
        public string TimeRange => $"{ShiftStart:hh\\:mm} - {ShiftEnd:hh\\:mm}";
        public bool IsCurrentShift => DateTime.Now.Date == WorkDate.Date && 
                                     DateTime.Now.TimeOfDay >= ShiftStart && 
                                     DateTime.Now.TimeOfDay <= ShiftEnd;
    }
}
