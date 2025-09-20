using System;

namespace AquaparkApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AttractionId { get; set; }
        public DateTime VisitDate { get; set; }
        public TimeSpan VisitTime { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Used, Cancelled
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public string QrCode { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        
        // Navigation properties
        public User? User { get; set; }
        public Attraction? Attraction { get; set; }
    }
}
