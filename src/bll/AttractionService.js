import AttractionRepository from '../dal/AttractionRepository.js';
import VisitorRepository from '../dal/VisitorRepository.js';

/**
 * Сервис для работы с аттракционами
 */
class AttractionService {
  constructor() {
    this.attractionRepository = new AttractionRepository();
    this.visitorRepository = new VisitorRepository();
  }

  /**
   * Получает все аттракционы
   */
  async getAllAttractions() {
    try {
      return this.attractionRepository.getAllAttractions();
    } catch (error) {
      console.error('Ошибка получения аттракционов:', error);
      throw new Error('Не удалось получить список аттракционов');
    }
  }

  /**
   * Получает аттракцион по ID
   */
  async getAttractionById(id) {
    try {
      const attraction = this.attractionRepository.getAttractionById(id);
      if (!attraction) {
        throw new Error('Аттракцион не найден');
      }
      return attraction;
    } catch (error) {
      console.error('Ошибка получения аттракциона:', error);
      throw error;
    }
  }

  /**
   * Создает новый аттракцион
   */
  async createAttraction(attractionData) {
    try {
      // Валидация данных
      this.validateAttractionData(attractionData);
      
      const attraction = this.attractionRepository.createAttraction(attractionData);
      return attraction;
    } catch (error) {
      console.error('Ошибка создания аттракциона:', error);
      throw error;
    }
  }

  /**
   * Обновляет аттракцион
   */
  async updateAttraction(id, updates) {
    try {
      const existingAttraction = this.attractionRepository.getAttractionById(id);
      if (!existingAttraction) {
        throw new Error('Аттракцион не найден');
      }

      // Валидация обновлений
      if (updates.capacity && updates.capacity < existingAttraction.currentVisitors) {
        throw new Error('Новая вместимость не может быть меньше текущего количества посетителей');
      }

      const updatedAttraction = this.attractionRepository.updateAttraction(id, updates);
      return updatedAttraction;
    } catch (error) {
      console.error('Ошибка обновления аттракциона:', error);
      throw error;
    }
  }

  /**
   * Удаляет аттракцион
   */
  async deleteAttraction(id) {
    try {
      const attraction = this.attractionRepository.getAttractionById(id);
      if (!attraction) {
        throw new Error('Аттракцион не найден');
      }

      if (attraction.currentVisitors > 0) {
        throw new Error('Нельзя удалить аттракцион с посетителями');
      }

      const deleted = this.attractionRepository.delete(id);
      if (!deleted) {
        throw new Error('Не удалось удалить аттракцион');
      }

      return true;
    } catch (error) {
      console.error('Ошибка удаления аттракциона:', error);
      throw error;
    }
  }

  /**
   * Получает активные аттракционы
   */
  async getActiveAttractions() {
    try {
      return this.attractionRepository.getActiveAttractions();
    } catch (error) {
      console.error('Ошибка получения активных аттракционов:', error);
      throw new Error('Не удалось получить активные аттракционы');
    }
  }

  /**
   * Получает доступные аттракционы
   */
  async getAvailableAttractions() {
    try {
      return this.attractionRepository.getAvailableAttractions();
    } catch (error) {
      console.error('Ошибка получения доступных аттракционов:', error);
      throw new Error('Не удалось получить доступные аттракционы');
    }
  }

  /**
   * Добавляет посетителя к аттракциону
   */
  async addVisitorToAttraction(attractionId, visitorId) {
    try {
      const attraction = this.attractionRepository.getAttractionById(attractionId);
      if (!attraction) {
        throw new Error('Аттракцион не найден');
      }

      if (!attraction.isAvailable()) {
        throw new Error('Аттракцион недоступен');
      }

      const visitor = this.visitorRepository.getVisitorById(visitorId);
      if (!visitor) {
        throw new Error('Посетитель не найден');
      }

      if (!visitor.isInPark()) {
        throw new Error('Посетитель не находится в парке');
      }

      // Добавляем посетителя к аттракциону
      const success = this.attractionRepository.addVisitorToAttraction(attractionId);
      if (success) {
        this.visitorRepository.addVisitorToAttraction(visitorId, attractionId);
      }

      return success;
    } catch (error) {
      console.error('Ошибка добавления посетителя к аттракциону:', error);
      throw error;
    }
  }

  /**
   * Удаляет посетителя с аттракциона
   */
  async removeVisitorFromAttraction(attractionId, visitorId) {
    try {
      const attraction = this.attractionRepository.getAttractionById(attractionId);
      if (!attraction) {
        throw new Error('Аттракцион не найден');
      }

      const visitor = this.visitorRepository.getVisitorById(visitorId);
      if (!visitor) {
        throw new Error('Посетитель не найден');
      }

      // Удаляем посетителя с аттракциона
      const success = this.attractionRepository.removeVisitorFromAttraction(attractionId);
      if (success) {
        this.visitorRepository.removeVisitorFromAttraction(visitorId, attractionId);
      }

      return success;
    } catch (error) {
      console.error('Ошибка удаления посетителя с аттракциона:', error);
      throw error;
    }
  }

  /**
   * Изменяет статус аттракциона
   */
  async changeAttractionStatus(id, status) {
    try {
      const validStatuses = ['active', 'maintenance', 'closed'];
      if (!validStatuses.includes(status)) {
        throw new Error('Недопустимый статус аттракциона');
      }

      const attraction = this.attractionRepository.getAttractionById(id);
      if (!attraction) {
        throw new Error('Аттракцион не найден');
      }

      if (status === 'closed' && attraction.currentVisitors > 0) {
        throw new Error('Нельзя закрыть аттракцион с посетителями');
      }

      const updatedAttraction = this.attractionRepository.updateAttraction(id, { status });
      return updatedAttraction;
    } catch (error) {
      console.error('Ошибка изменения статуса аттракциона:', error);
      throw error;
    }
  }

  /**
   * Получает статистику по аттракционам
   */
  async getAttractionsStats() {
    try {
      return this.attractionRepository.getAttractionsStats();
    } catch (error) {
      console.error('Ошибка получения статистики аттракционов:', error);
      throw new Error('Не удалось получить статистику аттракционов');
    }
  }

  /**
   * Валидирует данные аттракциона
   */
  validateAttractionData(data) {
    if (!data.name || data.name.trim().length === 0) {
      throw new Error('Название аттракциона обязательно');
    }

    if (!data.description || data.description.trim().length === 0) {
      throw new Error('Описание аттракциона обязательно');
    }

    if (!data.type || !['water_slide', 'pool', 'spa', 'kids_zone'].includes(data.type)) {
      throw new Error('Недопустимый тип аттракциона');
    }

    if (!data.capacity || data.capacity <= 0) {
      throw new Error('Вместимость должна быть больше 0');
    }

    if (data.capacity > 1000) {
      throw new Error('Вместимость не может превышать 1000 человек');
    }
  }
}

export default AttractionService;
