using System;
using System.Collections.Generic;

namespace AquaparkApp.Models
{
    /// <summary>
    /// Модель для отчета по продажам
    /// </summary>
    public class SalesReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalTickets { get; set; }
        public decimal AverageOrderValue { get; set; }
        public List<DailySales> DailySales { get; set; } = new List<DailySales>();
        public List<AttractionSales> AttractionSales { get; set; } = new List<AttractionSales>();
        public List<PaymentMethodStats> PaymentMethods { get; set; } = new List<PaymentMethodStats>();
    }

    /// <summary>
    /// Модель для ежедневных продаж
    /// </summary>
    public class DailySales
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int OrdersCount { get; set; }
        public int TicketsCount { get; set; }
    }

    /// <summary>
    /// Модель для продаж по аттракционам
    /// </summary>
    public class AttractionSales
    {
        public int AttractionId { get; set; }
        public string AttractionName { get; set; } = string.Empty;
        public int TicketsSold { get; set; }
        public decimal Revenue { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Модель для статистики по способам оплаты
    /// </summary>
    public class PaymentMethodStats
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public int OrdersCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Модель для отчета по пользователям
    /// </summary>
    public class UserReport
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public decimal AverageSpendingPerUser { get; set; }
        public List<TopUser> TopSpenders { get; set; } = new List<TopUser>();
        public List<UserRegistrationStats> RegistrationStats { get; set; } = new List<UserRegistrationStats>();
    }

    /// <summary>
    /// Модель для топ-пользователей по тратам
    /// </summary>
    public class TopUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal TotalSpent { get; set; }
        public int OrdersCount { get; set; }
    }

    /// <summary>
    /// Модель для статистики регистраций
    /// </summary>
    public class UserRegistrationStats
    {
        public DateTime Date { get; set; }
        public int RegistrationsCount { get; set; }
    }

    /// <summary>
    /// Модель для отчета по посещаемости
    /// </summary>
    public class AttendanceReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalVisitors { get; set; }
        public int PeakDayVisitors { get; set; }
        public DateTime PeakDay { get; set; }
        public List<HourlyAttendance> HourlyStats { get; set; } = new List<HourlyAttendance>();
        public List<AgeGroupStats> AgeGroupStats { get; set; } = new List<AgeGroupStats>();
    }

    /// <summary>
    /// Модель для почасовой посещаемости
    /// </summary>
    public class HourlyAttendance
    {
        public int Hour { get; set; }
        public int VisitorsCount { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Модель для статистики по возрастным группам
    /// </summary>
    public class AgeGroupStats
    {
        public string AgeGroup { get; set; } = string.Empty;
        public int VisitorsCount { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Модель для экспорта данных
    /// </summary>
    public class ExportData
    {
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public int RecordsCount { get; set; }
        public string ExportType { get; set; } = string.Empty; // Excel, PDF, CSV
    }
}
