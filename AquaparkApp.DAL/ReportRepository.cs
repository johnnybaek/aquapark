using AquaparkApp.Models;
using Dapper;
using System.Data;

namespace AquaparkApp.DAL
{
    public class ReportRepository
    {
        private readonly string _connectionString;

        public ReportRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection GetConnection()
        {
            return DatabaseConnection.GetConnection();
        }

        public async Task<SalesReport> GetSalesReportAsync(DateTime startDate, DateTime endDate)
        {
            using var connection = GetConnection();
            
            var report = new SalesReport
            {
                StartDate = startDate,
                EndDate = endDate
            };

            // Общая статистика
            var statsSql = @"
                SELECT 
                    COUNT(DISTINCT o.Id) as TotalOrders,
                    COUNT(t.Id) as TotalTickets,
                    COALESCE(SUM(o.FinalAmount), 0) as TotalRevenue,
                    COALESCE(AVG(o.FinalAmount), 0) as AverageOrderValue
                FROM Orders o
                LEFT JOIN Tickets t ON o.Id = t.OrderId
                WHERE o.CreatedAt BETWEEN @StartDate AND @EndDate
                AND o.Status IN ('Paid', 'Completed')";

            var stats = await connection.QueryFirstAsync(statsSql, new { StartDate = startDate, EndDate = endDate });
            report.TotalOrders = stats.TotalOrders;
            report.TotalTickets = stats.TotalTickets;
            report.TotalRevenue = stats.TotalRevenue;
            report.AverageOrderValue = stats.AverageOrderValue;

            // Ежедневные продажи
            var dailySalesSql = @"
                SELECT 
                    DATE(o.CreatedAt) as Date,
                    COALESCE(SUM(o.FinalAmount), 0) as Revenue,
                    COUNT(DISTINCT o.Id) as OrdersCount,
                    COUNT(t.Id) as TicketsCount
                FROM Orders o
                LEFT JOIN Tickets t ON o.Id = t.OrderId
                WHERE o.CreatedAt BETWEEN @StartDate AND @EndDate
                AND o.Status IN ('Paid', 'Completed')
                GROUP BY DATE(o.CreatedAt)
                ORDER BY DATE(o.CreatedAt)";

            report.DailySales = (await connection.QueryAsync<DailySales>(dailySalesSql, new { StartDate = startDate, EndDate = endDate })).ToList();

            // Продажи по аттракционам
            var attractionSalesSql = @"
                SELECT 
                    a.Id as AttractionId,
                    a.Name as AttractionName,
                    COUNT(t.Id) as TicketsSold,
                    COALESCE(SUM(t.TotalPrice), 0) as Revenue
                FROM Attractions a
                LEFT JOIN Tickets t ON a.Id = t.AttractionId
                LEFT JOIN Orders o ON t.OrderId = o.Id
                WHERE o.CreatedAt BETWEEN @StartDate AND @EndDate
                AND o.Status IN ('Paid', 'Completed')
                GROUP BY a.Id, a.Name
                ORDER BY Revenue DESC";

            var attractionSales = await connection.QueryAsync<AttractionSales>(attractionSalesSql, new { StartDate = startDate, EndDate = endDate });
            report.AttractionSales = attractionSales.ToList();

            // Процентное соотношение для аттракционов
            if (report.TotalRevenue > 0)
            {
                foreach (var attraction in report.AttractionSales)
                {
                    attraction.Percentage = (attraction.Revenue / report.TotalRevenue) * 100;
                }
            }

            // Статистика по способам оплаты
            var paymentMethodsSql = @"
                SELECT 
                    o.PaymentMethod,
                    COUNT(o.Id) as OrdersCount,
                    COALESCE(SUM(o.FinalAmount), 0) as TotalAmount
                FROM Orders o
                WHERE o.CreatedAt BETWEEN @StartDate AND @EndDate
                AND o.Status IN ('Paid', 'Completed')
                GROUP BY o.PaymentMethod
                ORDER BY TotalAmount DESC";

            var paymentMethods = await connection.QueryAsync<PaymentMethodStats>(paymentMethodsSql, new { StartDate = startDate, EndDate = endDate });
            report.PaymentMethods = paymentMethods.ToList();

            // Процентное соотношение для способов оплаты
            foreach (var method in report.PaymentMethods)
            {
                method.Percentage = (method.TotalAmount / report.TotalRevenue) * 100;
            }

            return report;
        }

        public async Task<UserReport> GetUserReportAsync()
        {
            using var connection = GetConnection();
            
            var report = new UserReport();

            // Общая статистика пользователей
            var statsSql = @"
                SELECT 
                    COUNT(*) as TotalUsers,
                    COUNT(CASE WHEN IsActive = true THEN 1 END) as ActiveUsers,
                    COUNT(CASE WHEN CreatedAt >= DATE_TRUNC('month', CURRENT_DATE) THEN 1 END) as NewUsersThisMonth,
                    COALESCE(AVG(TotalSpent), 0) as AverageSpendingPerUser
                FROM Users";

            var stats = await connection.QueryFirstAsync(statsSql);
            report.TotalUsers = stats.TotalUsers;
            report.ActiveUsers = stats.ActiveUsers;
            report.NewUsersThisMonth = stats.NewUsersThisMonth;
            report.AverageSpendingPerUser = stats.AverageSpendingPerUser;

            // Топ пользователей по тратам
            var topUsersSql = @"
                SELECT 
                    u.Id as UserId,
                    CONCAT(u.FirstName, ' ', u.LastName) as UserName,
                    u.Email,
                    u.TotalSpent,
                    u.TotalOrders as OrdersCount
                FROM Users u
                WHERE u.TotalSpent > 0
                ORDER BY u.TotalSpent DESC
                LIMIT 10";

            report.TopSpenders = (await connection.QueryAsync<TopUser>(topUsersSql)).ToList();

            // Статистика регистраций по дням (последние 30 дней)
            var registrationStatsSql = @"
                SELECT 
                    DATE(CreatedAt) as Date,
                    COUNT(*) as RegistrationsCount
                FROM Users
                WHERE CreatedAt >= CURRENT_DATE - INTERVAL '30 days'
                GROUP BY DATE(CreatedAt)
                ORDER BY DATE(CreatedAt)";

            report.RegistrationStats = (await connection.QueryAsync<UserRegistrationStats>(registrationStatsSql)).ToList();

            return report;
        }

        public async Task<AttendanceReport> GetAttendanceReportAsync(DateTime startDate, DateTime endDate)
        {
            using var connection = GetConnection();
            
            var report = new AttendanceReport
            {
                StartDate = startDate,
                EndDate = endDate
            };

            // Общая статистика посещаемости
            var statsSql = @"
                SELECT 
                    COUNT(DISTINCT t.UserId) as TotalVisitors,
                    MAX(daily_visitors.visitor_count) as PeakDayVisitors,
                    MAX(daily_visitors.visit_date) as PeakDay
                FROM Tickets t
                LEFT JOIN (
                    SELECT 
                        t2.VisitDate as visit_date,
                        COUNT(DISTINCT t2.UserId) as visitor_count
                    FROM Tickets t2
                    WHERE t2.VisitDate BETWEEN @StartDate AND @EndDate
                    AND t2.Status IN ('Confirmed', 'Used')
                    GROUP BY t2.VisitDate
                ) daily_visitors ON 1=1
                WHERE t.VisitDate BETWEEN @StartDate AND @EndDate
                AND t.Status IN ('Confirmed', 'Used')";

            var stats = await connection.QueryFirstAsync(statsSql, new { StartDate = startDate, EndDate = endDate });
            report.TotalVisitors = stats.TotalVisitors;
            report.PeakDayVisitors = stats.PeakDayVisitors;
            report.PeakDay = stats.PeakDay;

            // Почасовая посещаемость
            var hourlySql = @"
                SELECT 
                    EXTRACT(HOUR FROM t.VisitTime) as Hour,
                    COUNT(DISTINCT t.UserId) as VisitorsCount
                FROM Tickets t
                WHERE t.VisitDate BETWEEN @StartDate AND @EndDate
                AND t.Status IN ('Confirmed', 'Used')
                GROUP BY EXTRACT(HOUR FROM t.VisitTime)
                ORDER BY Hour";

            var hourlyData = await connection.QueryAsync(hourlySql, new { StartDate = startDate, EndDate = endDate });
            report.HourlyStats = hourlyData.Select(h => new HourlyAttendance
            {
                Hour = (int)h.Hour,
                VisitorsCount = h.VisitorsCount,
                Percentage = report.TotalVisitors > 0 ? (h.VisitorsCount * 100.0m / report.TotalVisitors) : 0
            }).ToList();

            // Статистика по возрастным группам
            var ageGroupSql = @"
                SELECT 
                    CASE 
                        WHEN EXTRACT(YEAR FROM AGE(u.DateOfBirth)) < 18 THEN 'Дети (до 18)'
                        WHEN EXTRACT(YEAR FROM AGE(u.DateOfBirth)) BETWEEN 18 AND 30 THEN 'Молодежь (18-30)'
                        WHEN EXTRACT(YEAR FROM AGE(u.DateOfBirth)) BETWEEN 31 AND 50 THEN 'Взрослые (31-50)'
                        ELSE 'Пожилые (50+)'
                    END as AgeGroup,
                    COUNT(DISTINCT t.UserId) as VisitorsCount
                FROM Tickets t
                JOIN Users u ON t.UserId = u.Id
                WHERE t.VisitDate BETWEEN @StartDate AND @EndDate
                AND t.Status IN ('Confirmed', 'Used')
                GROUP BY 
                    CASE 
                        WHEN EXTRACT(YEAR FROM AGE(u.DateOfBirth)) < 18 THEN 'Дети (до 18)'
                        WHEN EXTRACT(YEAR FROM AGE(u.DateOfBirth)) BETWEEN 18 AND 30 THEN 'Молодежь (18-30)'
                        WHEN EXTRACT(YEAR FROM AGE(u.DateOfBirth)) BETWEEN 31 AND 50 THEN 'Взрослые (31-50)'
                        ELSE 'Пожилые (50+)'
                    END
                ORDER BY VisitorsCount DESC";

            var ageGroupData = await connection.QueryAsync(ageGroupSql, new { StartDate = startDate, EndDate = endDate });
            report.AgeGroupStats = ageGroupData.Select(a => new AgeGroupStats
            {
                AgeGroup = a.AgeGroup,
                VisitorsCount = a.VisitorsCount,
                Percentage = report.TotalVisitors > 0 ? (a.VisitorsCount * 100.0m / report.TotalVisitors) : 0
            }).ToList();

            return report;
        }
    }
}
