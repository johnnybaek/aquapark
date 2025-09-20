import BaseRepository from './BaseRepository.js';
import Ticket from '../models/Ticket.js';

/**
 * Репозиторий для работы с билетами
 */
class TicketRepository extends BaseRepository {
  constructor() {
    super('aquapark_tickets');
    this.initializeDefaultData();
  }

  /**
   * Инициализирует данные по умолчанию
   */
  initializeDefaultData() {
    if (this.data.length === 0) {
      const now = new Date();
      const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
      const tomorrow = new Date(today.getTime() + 24 * 60 * 60 * 1000);
      const nextWeek = new Date(today.getTime() + 7 * 24 * 60 * 60 * 1000);

      const defaultTickets = [
        {
          id: '1',
          type: 'adult',
          name: 'Взрослый билет',
          description: 'Полный доступ ко всем аттракционам на весь день',
          price: 1500,
          validFrom: today.toISOString(),
          validTo: nextWeek.toISOString(),
          maxVisitors: 1
        },
        {
          id: '2',
          type: 'child',
          name: 'Детский билет',
          description: 'Для детей от 3 до 12 лет. Доступ к детским зонам',
          price: 800,
          validFrom: today.toISOString(),
          validTo: nextWeek.toISOString(),
          maxVisitors: 1
        },
        {
          id: '3',
          type: 'family',
          name: 'Семейный билет',
          description: '2 взрослых + 2 детей. Экономия 20%',
          price: 3600,
          validFrom: today.toISOString(),
          validTo: nextWeek.toISOString(),
          maxVisitors: 4
        },
        {
          id: '4',
          type: 'vip',
          name: 'VIP билет',
          description: 'Приоритетный доступ, спа-зона, питание включено',
          price: 3500,
          validFrom: today.toISOString(),
          validTo: nextWeek.toISOString(),
          maxVisitors: 1
        },
        {
          id: '5',
          type: 'adult',
          name: 'Вечерний билет',
          description: 'Доступ с 18:00 до закрытия (22:00)',
          price: 800,
          validFrom: today.toISOString(),
          validTo: nextWeek.toISOString(),
          maxVisitors: 1
        },
        {
          id: '6',
          type: 'group',
          name: 'Групповой билет',
          description: 'От 10 человек. Скидка 15% на каждого',
          price: 1275,
          validFrom: today.toISOString(),
          validTo: nextWeek.toISOString(),
          maxVisitors: 50
        }
      ];

      defaultTickets.forEach(ticket => {
        this.data.push(ticket);
      });
      this.saveToStorage();
    }
  }

  /**
   * Получает все билеты
   */
  getAllTickets() {
    return this.data.map(ticket => Ticket.fromJSON(ticket));
  }

  /**
   * Получает билет по ID
   */
  getTicketById(id) {
    const data = this.getById(id);
    return data ? Ticket.fromJSON(data) : null;
  }

  /**
   * Создает новый билет
   */
  createTicket(ticketData) {
    const ticket = new Ticket(
      ticketData.id || this.generateId(),
      ticketData.type,
      ticketData.name,
      ticketData.description,
      ticketData.price,
      new Date(ticketData.validFrom),
      new Date(ticketData.validTo),
      ticketData.maxVisitors || 1
    );

    return this.create(ticket.toJSON());
  }

  /**
   * Обновляет билет
   */
  updateTicket(id, updates) {
    const updated = this.update(id, updates);
    return updated ? Ticket.fromJSON(updated) : null;
  }

  /**
   * Получает действительные билеты
   */
  getValidTickets() {
    const now = new Date();
    return this.data
      .filter(ticket => {
        const validFrom = new Date(ticket.validFrom);
        const validTo = new Date(ticket.validTo);
        return now >= validFrom && now <= validTo;
      })
      .map(ticket => Ticket.fromJSON(ticket));
  }

  /**
   * Получает билеты по типу
   */
  getTicketsByType(type) {
    return this.data
      .filter(ticket => ticket.type === type)
      .map(ticket => Ticket.fromJSON(ticket));
  }

  /**
   * Получает билеты в ценовом диапазоне
   */
  getTicketsByPriceRange(minPrice, maxPrice) {
    return this.data
      .filter(ticket => ticket.price >= minPrice && ticket.price <= maxPrice)
      .map(ticket => Ticket.fromJSON(ticket));
  }

  /**
   * Поиск билетов по названию или описанию
   */
  searchTickets(query) {
    const lowerQuery = query.toLowerCase();
    return this.data
      .filter(ticket => 
        ticket.name.toLowerCase().includes(lowerQuery) ||
        ticket.description.toLowerCase().includes(lowerQuery)
      )
      .map(ticket => Ticket.fromJSON(ticket));
  }

  /**
   * Получает статистику по билетам
   */
  getTicketsStats() {
    const total = this.data.length;
    const valid = this.getValidTickets().length;
    const byType = this.data.reduce((acc, ticket) => {
      acc[ticket.type] = (acc[ticket.type] || 0) + 1;
      return acc;
    }, {});

    const totalRevenue = this.data.reduce((sum, ticket) => sum + ticket.price, 0);
    const averagePrice = total > 0 ? Math.round(totalRevenue / total) : 0;

    return {
      total,
      valid,
      byType,
      totalRevenue,
      averagePrice
    };
  }

  /**
   * Получает самые популярные типы билетов
   */
  getPopularTicketTypes(limit = 3) {
    const stats = this.getTicketsStats();
    return Object.entries(stats.byType)
      .sort(([,a], [,b]) => b - a)
      .slice(0, limit)
      .map(([type, count]) => ({ type, count }));
  }
}

export default TicketRepository;
