using AquaparkApp.DAL;
using AquaparkApp.Models;

namespace AquaparkApp.BLL
{
    public class TicketService
    {
        private readonly TicketRepository _ticketRepository;
        private readonly ClientRepository _clientRepository;

        public TicketService()
        {
            _ticketRepository = new TicketRepository();
            _clientRepository = new ClientRepository();
        }

        public async Task<IEnumerable<Ticket>> GetClientTicketsAsync(int clientId)
        {
            return await _ticketRepository.GetByClientIdAsync(clientId);
        }

        public async Task<IEnumerable<Ticket>> GetValidTicketsAsync()
        {
            return await _ticketRepository.GetValidTicketsAsync();
        }

        public async Task<IEnumerable<Ticket>> GetExpiredTicketsAsync()
        {
            return await _ticketRepository.GetExpiredTicketsAsync();
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await _ticketRepository.GetByIdAsync(id);
        }

        public async Task<bool> CreateTicketAsync(Ticket ticket)
        {
            // Проверяем, что клиент существует
            var client = await _clientRepository.GetByIdAsync(ticket.ClientId);
            if (client == null)
                return false;

            // Устанавливаем дату покупки
            ticket.PurchaseDate = DateTime.Now;

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

        public async Task<bool> UpdateTicketAsync(Ticket ticket)
        {
            try
            {
                return await _ticketRepository.UpdateAsync(ticket);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTicketAsync(int ticketId)
        {
            try
            {
                return await _ticketRepository.DeleteAsync(ticketId);
            }
            catch
            {
                return false;
            }
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            return await _ticketRepository.GetTotalRevenueAsync(startDate, endDate);
        }

        public async Task<IEnumerable<dynamic>> GetTicketTypeStatisticsAsync()
        {
            return await _ticketRepository.GetTicketTypeStatisticsAsync();
        }

        public async Task<bool> ValidateTicketAsync(int ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
                return false;

            return ticket.IsValid;
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return await _ticketRepository.GetAllAsync();
        }
    }
}
