using System;
using System.Collections.Generic;

namespace AquaparkApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Paid, Completed, Cancelled
        public string PaymentMethod { get; set; } = string.Empty; // Cash, Card, Online
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Notes { get; set; } = string.Empty;
        
        // Navigation properties
        public User? User { get; set; }
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
