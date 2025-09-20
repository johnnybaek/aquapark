import VisitorRepository from '../dal/VisitorRepository.js';
import AttractionService from './AttractionService.js';

/**
 * Сервис для работы с посетителями
 */
class VisitorService {
  constructor() {
    this.visitorRepository = new VisitorRepository();
    this.attractionService = new AttractionService();
  }

  /**
   * Получает всех посетителей
   */
  async getAllVisitors() {
    try {
      return this.visitorRepository.getAllVisitors();
    } catch (error) {
      console.error('Ошибка получения посетителей:', error);
      throw new Error('Не удалось получить список посетителей');
    }
  }

  /**
   * Получает посетителя по ID
   */
  async getVisitorById(id) {
    try {
      const visitor = this.visitorRepository.getVisitorById(id);
      if (!visitor) {
        throw new Error('Посетитель не найден');
      }
      return visitor;
    } catch (error) {
      console.error('Ошибка получения посетителя:', error);
      throw error;
    }
  }

  /**
   * Создает нового посетителя
   */
  async createVisitor(visitorData) {
    try {
      // Валидация данных
      this.validateVisitorData(visitorData);
      
      const visitor = this.visitorRepository.createVisitor(visitorData);
      return visitor;
    } catch (error) {
      console.error('Ошибка создания посетителя:', error);
      throw error;
    }
  }

  /**
   * Обновляет посетителя
   */
  async updateVisitor(id, updates) {
    try {
      const existingVisitor = this.visitorRepository.getVisitorById(id);
      if (!existingVisitor) {
        throw new Error('Посетитель не найден');
      }

      // Валидация обновлений
      if (updates.age !== undefined && (updates.age < 0 || updates.age > 120)) {
        throw new Error('Возраст должен быть от 0 до 120 лет');
      }

      const updatedVisitor = this.visitorRepository.updateVisitor(id, updates);
      return updatedVisitor;
    } catch (error) {
      console.error('Ошибка обновления посетителя:', error);
      throw error;
    }
  }

  /**
   * Удаляет посетителя
   */
  async deleteVisitor(id) {
    try {
      const visitor = this.visitorRepository.getVisitorById(id);
      if (!visitor) {
        throw new Error('Посетитель не найден');
      }

      if (visitor.isInPark()) {
        throw new Error('Нельзя удалить посетителя, который находится в парке');
      }

      const deleted = this.visitorRepository.delete(id);
      if (!deleted) {
        throw new Error('Не удалось удалить посетителя');
      }

      return true;
    } catch (error) {
      console.error('Ошибка удаления посетителя:', error);
      throw error;
    }
  }

  /**
   * Получает посетителей, находящихся в парке
   */
  async getVisitorsInPark() {
    try {
      return this.visitorRepository.getVisitorsInPark();
    } catch (error) {
      console.error('Ошибка получения посетителей в парке:', error);
      throw new Error('Не удалось получить посетителей в парке');
    }
  }

  /**
   * Получает посетителей по возрастной категории
   */
  async getVisitorsByAgeCategory(category) {
    try {
      const validCategories = ['child', 'teenager', 'adult'];
      if (!validCategories.includes(category)) {
        throw new Error('Недопустимая возрастная категория');
      }
      return this.visitorRepository.getVisitorsByAgeCategory(category);
    } catch (error) {
      console.error('Ошибка получения посетителей по возрастной категории:', error);
      throw error;
    }
  }

  /**
   * Получает посетителей за период
   */
  async getVisitorsByPeriod(startDate, endDate) {
    try {
      const start = new Date(startDate);
      const end = new Date(endDate);

      if (start > end) {
        throw new Error('Дата начала не может быть позже даты окончания');
      }

      return this.visitorRepository.getVisitorsByPeriod(startDate, endDate);
    } catch (error) {
      console.error('Ошибка получения посетителей за период:', error);
      throw error;
    }
  }

  /**
   * Получает посетителей за сегодня
   */
  async getTodayVisitors() {
    try {
      return this.visitorRepository.getTodayVisitors();
    } catch (error) {
      console.error('Ошибка получения посетителей за сегодня:', error);
      throw new Error('Не удалось получить посетителей за сегодня');
    }
  }

  /**
   * Добавляет посетителя к аттракциону
   */
  async addVisitorToAttraction(visitorId, attractionId) {
    try {
      const visitor = this.visitorRepository.getVisitorById(visitorId);
      if (!visitor) {
        throw new Error('Посетитель не найден');
      }

      if (!visitor.isInPark()) {
        throw new Error('Посетитель не находится в парке');
      }

      // Проверяем, что посетитель не находится уже на этом аттракционе
      if (visitor.currentAttractions.includes(attractionId)) {
        throw new Error('Посетитель уже находится на этом аттракционе');
      }

      // Используем AttractionService для добавления посетителя
      const success = await this.attractionService.addVisitorToAttraction(attractionId, visitorId);
      return success;
    } catch (error) {
      console.error('Ошибка добавления посетителя к аттракциону:', error);
      throw error;
    }
  }

  /**
   * Удаляет посетителя с аттракциона
   */
  async removeVisitorFromAttraction(visitorId, attractionId) {
    try {
      const visitor = this.visitorRepository.getVisitorById(visitorId);
      if (!visitor) {
        throw new Error('Посетитель не найден');
      }

      if (!visitor.currentAttractions.includes(attractionId)) {
        throw new Error('Посетитель не находится на этом аттракционе');
      }

      // Используем AttractionService для удаления посетителя
      const success = await this.attractionService.removeVisitorFromAttraction(attractionId, visitorId);
      return success;
    } catch (error) {
      console.error('Ошибка удаления посетителя с аттракциона:', error);
      throw error;
    }
  }

  /**
   * Завершает посещение
   */
  async finishVisit(visitorId) {
    try {
      const visitor = this.visitorRepository.getVisitorById(visitorId);
      if (!visitor) {
        throw new Error('Посетитель не найден');
      }

      if (!visitor.isInPark()) {
        throw new Error('Посетитель уже покинул парк');
      }

      // Удаляем посетителя со всех аттракционов
      for (const attractionId of visitor.currentAttractions) {
        await this.attractionService.removeVisitorFromAttraction(attractionId, visitorId);
      }

      // Завершаем посещение
      const success = this.visitorRepository.finishVisit(visitorId);
      return success;
    } catch (error) {
      console.error('Ошибка завершения посещения:', error);
      throw error;
    }
  }

  /**
   * Получает статистику по посетителям
   */
  async getVisitorsStats() {
    try {
      return this.visitorRepository.getVisitorsStats();
    } catch (error) {
      console.error('Ошибка получения статистики посетителей:', error);
      throw new Error('Не удалось получить статистику посетителей');
    }
  }

  /**
   * Получает самых активных посетителей
   */
  async getMostActiveVisitors(limit = 5) {
    try {
      return this.visitorRepository.getMostActiveVisitors(limit);
    } catch (error) {
      console.error('Ошибка получения самых активных посетителей:', error);
      throw new Error('Не удалось получить самых активных посетителей');
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

    if (!data.ticketId) {
      throw new Error('ID билета обязателен');
    }

    if (data.phone && !/^[\+]?[0-9\s\-\(\)]{10,}$/.test(data.phone)) {
      throw new Error('Некорректный номер телефона');
    }

    if (data.email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(data.email)) {
      throw new Error('Некорректный email адрес');
    }
  }
}

export default VisitorService;
