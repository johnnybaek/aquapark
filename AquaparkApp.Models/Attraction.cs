using System;

namespace AquaparkApp.Models
{
    public class Attraction
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int MinHeight { get; set; } // в см
        public int MaxHeight { get; set; } // в см
        public int Capacity { get; set; } // максимальная вместимость
        public int Duration { get; set; } // продолжительность в минутах
        public string ImagePath { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Category { get; set; } = string.Empty; // Водные горки, Бассейны, СПА и т.д.
        public string DifficultyLevel { get; set; } = string.Empty; // Легкий, Средний, Сложный
    }
}
