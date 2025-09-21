using System;
using System.Threading.Tasks;
using AquaparkApp.DAL;

namespace AquaparkApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("🌊 Тестирование подключения к базе данных аквапарка");
            Console.WriteLine("=================================================");

            try
            {
                // Тестируем подключение
                var isConnected = await DatabaseConnection.TestConnectionAsync();
                
                if (isConnected)
                {
                    Console.WriteLine("✅ Подключение к базе данных успешно!");
                    
                    // Тестируем репозитории
                    await TestRepositories();
                }
                else
                {
                    Console.WriteLine("❌ Ошибка подключения к базе данных");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static async Task TestRepositories()
        {
            Console.WriteLine("\n📊 Тестирование репозиториев...");

            try
            {
                // Тест репозитория клиентов
                var clientRepo = new ClientRepository();
                var clients = await clientRepo.GetAllAsync();
                Console.WriteLine($"✅ Клиенты: {clients.Count()} записей");

                // Тест репозитория зон
                var zoneRepo = new ZoneRepository();
                var zones = await zoneRepo.GetAllAsync();
                Console.WriteLine($"✅ Зоны: {zones.Count()} записей");

                // Тест репозитория сотрудников
                var employeeRepo = new EmployeeRepository();
                var employees = await employeeRepo.GetAllAsync();
                Console.WriteLine($"✅ Сотрудники: {employees.Count()} записей");

                // Тест репозитория услуг
                var serviceRepo = new ServiceRepository();
                var services = await serviceRepo.GetAllAsync();
                Console.WriteLine($"✅ Услуги: {services.Count()} записей");

                // Тест репозитория билетов
                var ticketRepo = new TicketRepository();
                var tickets = await ticketRepo.GetAllAsync();
                Console.WriteLine($"✅ Билеты: {tickets.Count()} записей");

                Console.WriteLine("\n🎉 Все репозитории работают корректно!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка тестирования репозиториев: {ex.Message}");
            }
        }
    }
}
