using AquaparkApp.DAL;
using AquaparkApp.Models;

namespace AquaparkApp.BLL
{
    public class PaymentService
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly TicketRepository _ticketRepository;
        private readonly ClientRepository _clientRepository;

        public PaymentService()
        {
            _paymentRepository = new PaymentRepository();
            _ticketRepository = new TicketRepository();
            _clientRepository = new ClientRepository();
        }

        public async Task<bool> ProcessTicketPurchaseAsync(int clientId, List<Ticket> tickets, int employeeId)
        {
            if (!tickets.Any()) return false;

            try
            {
                // Создаем билеты
                foreach (var ticket in tickets)
                {
                    ticket.ClientId = clientId;
                    ticket.PurchaseDate = DateTime.Now;
                    await _ticketRepository.CreateAsync(ticket);
                }

                // Создаем запись об оплате
                var totalAmount = tickets.Sum(t => t.Price);
                var payment = new Payment
                {
                    ClientId = clientId,
                    TicketId = tickets.First().TicketId, // Связываем с первым билетом
                    EmployeeId = employeeId,
                    Amount = totalAmount,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "карта" // По умолчанию карта
                };

                await _paymentRepository.CreateAsync(payment);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<decimal> CalculateTotalAmountAsync(List<Ticket> tickets)
        {
            return tickets.Sum(t => t.Price);
        }

        public async Task<IEnumerable<Payment>> GetClientPaymentsAsync(int clientId)
        {
            return await _paymentRepository.GetByClientIdAsync(clientId);
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            return await _paymentRepository.GetTotalRevenueAsync(startDate, endDate);
        }

        public async Task<bool> CreatePaymentAsync(Payment payment)
        {
            try
            {
                await _paymentRepository.CreateAsync(payment);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdatePaymentAsync(Payment payment)
        {
            try
            {
                return await _paymentRepository.UpdateAsync(payment);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeletePaymentAsync(int paymentId)
        {
            try
            {
                return await _paymentRepository.DeleteAsync(paymentId);
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<dynamic>> GetPaymentMethodStatisticsAsync()
        {
            return await _paymentRepository.GetPaymentMethodStatisticsAsync();
        }

        public async Task<IEnumerable<dynamic>> GetDailyRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            return await _paymentRepository.GetDailyRevenueAsync(startDate, endDate);
        }
    }
}
