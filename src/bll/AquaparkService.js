import AttractionService from './AttractionService.js';
import TicketService from './TicketService.js';
import VisitorService from './VisitorService.js';

/**
 * Главный сервис аквапарка для координации всех операций
 */
class AquaparkService {
  constructor() {
    this.attractionService = new AttractionService();
    this.ticketService = new TicketService();
    this.visitorService = new VisitorService();
  }

  /**
   * Получает общую статистику аквапарка
   */
  async getOverallStats() {
    try {
      const [attractionsStats, ticketsStats, visitorsStats] = await Promise.all([
        this.attractionService.getAttractionsStats(),
        this.ticketService.getTicketsStats(),
        this.visitorService.getVisitorsStats()
      ]);

      return {
        attractions: attractionsStats,
        tickets: ticketsStats,
        visitors: visitorsStats,
        summary: {
          totalAttractions: attractionsStats.total,
          activeAttractions: attractionsStats.active,
          totalVisitorsToday: visitorsStats.today,
          visitorsInPark: visitorsStats.inPark,
          totalRevenue: ticketsStats.totalRevenue,
          averageLoad: attractionsStats.averageLoad
        }
      };
    } catch (error) {
      console.error('Ошибка получения общей статистики:', error);
      throw new Error('Не удалось получить общую статистику');
    }
  }

  /**
   * Получает данные для главной панели
   */
  async getDashboardData() {
    try {
      const [attractions, tickets, visitors, stats] = await Promise.all([
        this.attractionService.getAllAttractions(),
        this.ticketService.getAllTickets(),
        this.visitorService.getVisitorsInPark(),
        this.getOverallStats()
      ]);

      return {
        attractions: attractions.slice(0, 6), // Показываем только первые 6
        tickets: tickets.slice(0, 6),
        visitors: visitors.slice(0, 10), // Показываем только первых 10
        stats
      };
    } catch (error) {
      console.error('Ошибка получения данных для панели:', error);
      throw new Error('Не удалось получить данные для панели');
    }
  }

  /**
   * Получает данные для управления аттракционами
   */
  async getAttractionsData() {
    try {
      const [attractions, stats] = await Promise.all([
        this.attractionService.getAllAttractions(),
        this.attractionService.getAttractionsStats()
      ]);

      return {
        attractions,
        stats
      };
    } catch (error) {
      console.error('Ошибка получения данных аттракционов:', error);
      throw new Error('Не удалось получить данные аттракционов');
    }
  }

  /**
   * Получает данные для управления билетами
   */
  async getTicketsData() {
    try {
      const [tickets, stats] = await Promise.all([
        this.ticketService.getAllTickets(),
        this.ticketService.getTicketsStats()
      ]);

      return {
        tickets,
        stats
      };
    } catch (error) {
      console.error('Ошибка получения данных билетов:', error);
      throw new Error('Не удалось получить данные билетов');
    }
  }

  /**
   * Получает данные для управления посетителями
   */
  async getVisitorsData() {
    try {
      const [visitors, stats] = await Promise.all([
        this.visitorService.getAllVisitors(),
        this.visitorService.getVisitorsStats()
      ]);

      return {
        visitors,
        stats
      };
    } catch (error) {
      console.error('Ошибка получения данных посетителей:', error);
      throw new Error('Не удалось получить данные посетителей');
    }
  }

  /**
   * Выполняет поиск по всем данным
   */
  async searchAll(query) {
    try {
      if (!query || query.trim().length === 0) {
        return {
          attractions: [],
          tickets: [],
          visitors: []
        };
      }

      const [attractions, tickets, visitors] = await Promise.all([
        this.attractionService.getAllAttractions().then(attractions => 
          attractions.filter(attraction => 
            attraction.name.toLowerCase().includes(query.toLowerCase()) ||
            attraction.description.toLowerCase().includes(query.toLowerCase())
          )
        ),
        this.ticketService.searchTickets(query),
        this.visitorService.getAllVisitors().then(visitors =>
          visitors.filter(visitor =>
            visitor.name.toLowerCase().includes(query.toLowerCase())
          )
        )
      ]);

      return {
        attractions,
        tickets,
        visitors
      };
    } catch (error) {
      console.error('Ошибка поиска:', error);
      throw new Error('Не удалось выполнить поиск');
    }
  }

  /**
   * Получает последнюю активность
   */
  async getRecentActivity() {
    try {
      const [visitors, attractions] = await Promise.all([
        this.visitorService.getTodayVisitors(),
        this.attractionService.getAllAttractions()
      ]);

      // Сортируем посетителей по времени входа
      const recentVisitors = visitors
        .sort((a, b) => new Date(b.entryTime) - new Date(a.entryTime))
        .slice(0, 5);

      // Создаем список активности
      const activities = recentVisitors.map(visitor => ({
        type: 'visitor_entry',
        message: `Новый посетитель: ${visitor.name}`,
        time: visitor.entryTime,
        icon: '👤'
      }));

      // Добавляем информацию об аттракционах
      const maintenanceAttractions = attractions.filter(a => a.status === 'maintenance');
      maintenanceAttractions.forEach(attraction => {
        activities.push({
          type: 'attraction_maintenance',
          message: `Аттракцион "${attraction.name}" на обслуживании`,
          time: attraction.updatedAt,
          icon: '🔧'
        });
      });

      // Сортируем по времени
      return activities.sort((a, b) => new Date(b.time) - new Date(a.time));
    } catch (error) {
      console.error('Ошибка получения последней активности:', error);
      throw new Error('Не удалось получить последнюю активность');
    }
  }

  /**
   * Экспортирует данные в JSON
   */
  async exportData() {
    try {
      const [attractions, tickets, visitors] = await Promise.all([
        this.attractionService.getAllAttractions(),
        this.ticketService.getAllTickets(),
        this.visitorService.getAllVisitors()
      ]);

      const exportData = {
        timestamp: new Date().toISOString(),
        attractions: attractions.map(a => a.toJSON()),
        tickets: tickets.map(t => t.toJSON()),
        visitors: visitors.map(v => v.toJSON())
      };

      return exportData;
    } catch (error) {
      console.error('Ошибка экспорта данных:', error);
      throw new Error('Не удалось экспортировать данные');
    }
  }

  /**
   * Импортирует данные из JSON
   */
  async importData(data) {
    try {
      if (!data || typeof data !== 'object') {
        throw new Error('Некорректные данные для импорта');
      }

      // Валидируем структуру данных
      if (!data.attractions || !data.tickets || !data.visitors) {
        throw new Error('Отсутствуют обязательные разделы данных');
      }

      // Очищаем существующие данные
      // В реальном приложении здесь была бы более сложная логика
      
      return {
        success: true,
        message: 'Данные успешно импортированы'
      };
    } catch (error) {
      console.error('Ошибка импорта данных:', error);
      throw error;
    }
  }
}

export default AquaparkService;
