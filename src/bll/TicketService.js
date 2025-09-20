import TicketRepository from '../dal/TicketRepository.js';
import VisitorRepository from '../dal/VisitorRepository.js';

/**
 * Сервис для работы с билетами
 */
class TicketService {
  constructor() {
    this.ticketRepository = new TicketRepository();
    this.visitorRepository = new VisitorRepository();
  }

  /**
   * Получает все билеты
   */
  async getAllTickets() {
    try {
      return this.ticketRepository.getAllTickets();
    } catch (error) {
      console.error('Ошибка получения билетов:', error);
      throw new Error('Не удалось получить список билетов');
    }
  }

  /**
   * Получает билет по ID
   */
  async getTicketById(id) {
    try {
      const ticket = this.ticketRepository.getTicketById(id);
      if (!ticket) {
        throw new Error('Билет не найден');
      }
      return ticket;
    } catch (error) {
      console.error('Ошибка получения билета:', error);
      throw error;
    }
  }

  /**
   * Создает новый билет
   */
  async createTicket(ticketData) {
    try {
      // Валидация данных
      this.validateTicketData(ticketData);
      
      const ticket = this.ticketRepository.createTicket(ticketData);
      return ticket;
    } catch (error) {
      console.error('Ошибка создания билета:', error);
      throw error;
    }
  }

  /**
   * Обновляет билет
   */
  async updateTicket(id, updates) {
    try {
      const existingTicket = this.ticketRepository.getTicketById(id);
      if (!existingTicket) {
        throw new Error('Билет не найден');
      }

      // Валидация обновлений
      if (updates.price !== undefined && updates.price < 0) {
        throw new Error('Цена не может быть отрицательной');
      }

      const updatedTicket = this.ticketRepository.updateTicket(id, updates);
      return updatedTicket;
    } catch (error) {
      console.error('Ошибка обновления билета:', error);
      throw error;
    }
  }

  /**
   * Удаляет билет
   */
  async deleteTicket(id) {
    try {
      const ticket = this.ticketRepository.getTicketById(id);
      if (!ticket) {
        throw new Error('Билет не найден');
      }

      // Проверяем, есть ли посетители с этим типом билета
      const visitorsWithTicket = this.visitorRepository.getVisitorsByTicketType(id);
      if (visitorsWithTicket.length > 0) {
        throw new Error('Нельзя удалить билет, который используется посетителями');
      }

      const deleted = this.ticketRepository.delete(id);
      if (!deleted) {
        throw new Error('Не удалось удалить билет');
      }

      return true;
    } catch (error) {
      console.error('Ошибка удаления билета:', error);
      throw error;
    }
  }

  /**
   * Получает действительные билеты
   */
  async getValidTickets() {
    try {
      return this.ticketRepository.getValidTickets();
    } catch (error) {
      console.error('Ошибка получения действительных билетов:', error);
      throw new Error('Не удалось получить действительные билеты');
    }
  }

  /**
   * Получает билеты по типу
   */
  async getTicketsByType(type) {
    try {
      return this.ticketRepository.getTicketsByType(type);
    } catch (error) {
      console.error('Ошибка получения билетов по типу:', error);
      throw new Error('Не удалось получить билеты по типу');
    }
  }

  /**
   * Поиск билетов
   */
  async searchTickets(query) {
    try {
      if (!query || query.trim().length === 0) {
        return this.getAllTickets();
      }
      return this.ticketRepository.searchTickets(query);
    } catch (error) {
      console.error('Ошибка поиска билетов:', error);
      throw new Error('Не удалось выполнить поиск билетов');
    }
  }

  /**
   * Получает билеты в ценовом диапазоне
   */
  async getTicketsByPriceRange(minPrice, maxPrice) {
    try {
      if (minPrice < 0 || maxPrice < 0) {
        throw new Error('Цена не может быть отрицательной');
      }
      if (minPrice > maxPrice) {
        throw new Error('Минимальная цена не может быть больше максимальной');
      }
      return this.ticketRepository.getTicketsByPriceRange(minPrice, maxPrice);
    } catch (error) {
      console.error('Ошибка получения билетов по цене:', error);
      throw error;
    }
  }

  /**
   * Продает билет
   */
  async sellTicket(ticketId, visitorData) {
    try {
      const ticket = this.ticketRepository.getTicketById(ticketId);
      if (!ticket) {
        throw new Error('Билет не найден');
      }

      if (!ticket.isValidNow()) {
        throw new Error('Билет недействителен');
      }

      // Валидация данных посетителя
      this.validateVisitorData(visitorData);

      // Проверяем, подходит ли билет для количества посетителей
      if (!ticket.canAccommodate(visitorData.visitorCount || 1)) {
        throw new Error('Билет не подходит для указанного количества посетителей');
      }

      // Создаем посетителя
      const visitor = this.visitorRepository.createVisitor({
        name: visitorData.name,
        age: visitorData.age,
        ticketId: ticketId,
        phone: visitorData.phone,
        email: visitorData.email
      });

      return {
        visitor,
        ticket,
        totalPrice: ticket.getFinalPrice() * (visitorData.visitorCount || 1)
      };
    } catch (error) {
      console.error('Ошибка продажи билета:', error);
      throw error;
    }
  }

  /**
   * Получает статистику по билетам
   */
  async getTicketsStats() {
    try {
      return this.ticketRepository.getTicketsStats();
    } catch (error) {
      console.error('Ошибка получения статистики билетов:', error);
      throw new Error('Не удалось получить статистику билетов');
    }
  }

  /**
   * Получает самые популярные типы билетов
   */
  async getPopularTicketTypes(limit = 3) {
    try {
      return this.ticketRepository.getPopularTicketTypes(limit);
    } catch (error) {
      console.error('Ошибка получения популярных типов билетов:', error);
      throw new Error('Не удалось получить популярные типы билетов');
    }
  }

  /**
   * Валидирует данные билета
   */
  validateTicketData(data) {
    if (!data.name || data.name.trim().length === 0) {
      throw new Error('Название билета обязательно');
    }

    if (!data.description || data.description.trim().length === 0) {
      throw new Error('Описание билета обязательно');
    }

    if (!data.type || !['adult', 'child', 'family', 'vip', 'group'].includes(data.type)) {
      throw new Error('Недопустимый тип билета');
    }

    if (!data.price || data.price < 0) {
      throw new Error('Цена должна быть больше или равна 0');
    }

    if (data.price > 100000) {
      throw new Error('Цена не может превышать 100,000 рублей');
    }

    if (!data.validFrom || !data.validTo) {
      throw new Error('Период действия билета обязателен');
    }

    const validFrom = new Date(data.validFrom);
    const validTo = new Date(data.validTo);

    if (validFrom >= validTo) {
      throw new Error('Дата начала действия должна быть раньше даты окончания');
    }

    if (data.maxVisitors && (data.maxVisitors < 1 || data.maxVisitors > 100)) {
      throw new Error('Максимальное количество посетителей должно быть от 1 до 100');
    }
  }

  /**
   * Валидирует данные посетителя
   */
  validateVisitorData(data) {
    if (!data.name || data.name.trim().length === 0) {
      throw new Error('Имя посетителя обязательно');
    }

    if (!data.age || data.age < 0 || data.age > 120) {
      throw new Error('Возраст должен быть от 0 до 120 лет');
    }

    if (data.phone && !/^[\+]?[0-9\s\-\(\)]{10,}$/.test(data.phone)) {
      throw new Error('Некорректный номер телефона');
    }

    if (data.email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(data.email)) {
      throw new Error('Некорректный email адрес');
    }
  }
}

export default TicketService;
