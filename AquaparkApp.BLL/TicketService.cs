using AquaparkApp.DAL;
using AquaparkApp.Models;

namespace AquaparkApp.BLL
{
    public class TicketService
    {
        private readonly TicketRepository _ticketRepository;
        private readonly AttractionRepository _attractionRepository;

        public TicketService()
        {
            _ticketRepository = new TicketRepository();
            _attractionRepository = new AttractionRepository();
        }

        public TicketService(string connectionString)
        {
            _ticketRepository = new TicketRepository(connectionString);
            _attractionRepository = new AttractionRepository(connectionString);
        }

        public async Task<IEnumerable<Ticket>> GetUserTicketsAsync(int userId)
        {
            return await _ticketRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _ticketRepository.GetByDateRangeAsync(startDate, endDate);
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await _ticketRepository.GetByIdAsync(id);
        }

        public async Task<bool> CreateTicketAsync(Ticket ticket)
        {
            // Проверяем, что аттракцион существует и активен
            var attraction = await _attractionRepository.GetByIdAsync(ticket.AttractionId);
            if (attraction == null || !attraction.IsActive)
                return false;

            // Устанавливаем цену из аттракциона
            ticket.Price = attraction.Price;
            ticket.TotalPrice = ticket.Price * ticket.Quantity;
            ticket.Status = "Pending";
            ticket.CreatedAt = DateTime.UtcNow;
            ticket.QrCode = GenerateQrCode();

            try
            {
                await _ticketRepository.CreateAsync(ticket);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ConfirmTicketAsync(int ticketId)
        {
            return await _ticketRepository.UpdateStatusAsync(ticketId, "Confirmed");
        }

        public async Task<bool> UseTicketAsync(int ticketId)
        {
            return await _ticketRepository.MarkAsUsedAsync(ticketId);
        }

        public async Task<bool> CancelTicketAsync(int ticketId)
        {
            return await _ticketRepository.UpdateStatusAsync(ticketId, "Cancelled");
        }

        public async Task<decimal> CalculateTotalPriceAsync(List<Ticket> tickets)
        {
            return tickets.Sum(t => t.TotalPrice);
        }

        public async Task<bool> ValidateTicketAsync(int ticketId, int userId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null || ticket.UserId != userId)
                return false;

            return ticket.Status == "Confirmed" && ticket.VisitDate.Date >= DateTime.Today;
        }

        private string GenerateQrCode()
        {
            // Простая генерация QR кода (в реальном приложении используйте библиотеку QR кодов)
            return $"TICKET_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }
    }
}
