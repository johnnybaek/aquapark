using AquaparkApp.DAL;
using AquaparkApp.Models;
using OfficeOpenXml;
using System.Drawing;
using System.IO;

namespace AquaparkApp.BLL
{
    public class ReportService
    {
        private readonly ReportRepository _reportRepository;

        public ReportService(string connectionString)
        {
            _reportRepository = new ReportRepository(connectionString);
        }

        public async Task<SalesReport> GetSalesReportAsync(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GetSalesReportAsync(startDate, endDate);
        }

        public async Task<UserReport> GetUserReportAsync()
        {
            return await _reportRepository.GetUserReportAsync();
        }

        public async Task<AttendanceReport> GetAttendanceReportAsync(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GetAttendanceReportAsync(startDate, endDate);
        }

        public async Task<string> ExportSalesReportToExcelAsync(DateTime startDate, DateTime endDate, string filePath)
        {
            var report = await GetSalesReportAsync(startDate, endDate);
            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            
            // Лист с общей статистикой
            var summarySheet = package.Workbook.Worksheets.Add("Общая статистика");
            summarySheet.Cells[1, 1].Value = "Отчет по продажам";
            summarySheet.Cells[1, 1].Style.Font.Size = 16;
            summarySheet.Cells[1, 1].Style.Font.Bold = true;
            
            summarySheet.Cells[3, 1].Value = "Период:";
            summarySheet.Cells[3, 2].Value = $"{startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}";
            
            summarySheet.Cells[4, 1].Value = "Общая выручка:";
            summarySheet.Cells[4, 2].Value = report.TotalRevenue;
            summarySheet.Cells[4, 2].Style.Numberformat.Format = "#,##0.00 ₽";
            
            summarySheet.Cells[5, 1].Value = "Количество заказов:";
            summarySheet.Cells[5, 2].Value = report.TotalOrders;
            
            summarySheet.Cells[6, 1].Value = "Количество билетов:";
            summarySheet.Cells[6, 2].Value = report.TotalTickets;
            
            summarySheet.Cells[7, 1].Value = "Средний чек:";
            summarySheet.Cells[7, 2].Value = report.AverageOrderValue;
            summarySheet.Cells[7, 2].Style.Numberformat.Format = "#,##0.00 ₽";

            // Автоподбор ширины колонок
            summarySheet.Cells.AutoFitColumns();

            await package.SaveAsAsync(new FileInfo(filePath));
            return filePath;
        }
    }
}