import BaseRepository from './BaseRepository.js';
import Attraction from '../models/Attraction.js';

/**
 * Репозиторий для работы с аттракционами
 */
class AttractionRepository extends BaseRepository {
  constructor() {
    super('aquapark_attractions');
    this.initializeDefaultData();
  }

  /**
   * Инициализирует данные по умолчанию
   */
  initializeDefaultData() {
    if (this.data.length === 0) {
      const defaultAttractions = [
        {
          id: '1',
          name: "Водная горка 'Торнадо'",
          description: "Экстремальная горка высотой 25 метров с крутыми поворотами",
          type: "water_slide",
          capacity: 120,
          status: "active",
          currentVisitors: 85,
          waitTime: 15
        },
        {
          id: '2',
          name: "Ленивая река",
          description: "Спокойное течение для расслабляющего отдыха",
          type: "pool",
          capacity: 200,
          status: "active",
          currentVisitors: 45,
          waitTime: 0
        },
        {
          id: '3',
          name: "Волновой бассейн",
          description: "Искусственные волны каждые 10 минут",
          type: "pool",
          capacity: 300,
          status: "maintenance",
          currentVisitors: 0,
          waitTime: 0
        },
        {
          id: '4',
          name: "Детский городок",
          description: "Безопасная зона для детей до 12 лет",
          type: "kids_zone",
          capacity: 80,
          status: "active",
          currentVisitors: 32,
          waitTime: 5
        },
        {
          id: '5',
          name: "Спа-зона",
          description: "Термальные ванны и джакузи",
          type: "spa",
          capacity: 50,
          status: "active",
          currentVisitors: 28,
          waitTime: 20
        },
        {
          id: '6',
          name: "Экстремальная воронка",
          description: "Воронка диаметром 15 метров с водопадом",
          type: "water_slide",
          capacity: 60,
          status: "closed",
          currentVisitors: 0,
          waitTime: 0
        }
      ];

      defaultAttractions.forEach(attraction => {
        this.data.push(attraction);
      });
      this.saveToStorage();
    }
  }

  /**
   * Получает все аттракционы
   */
  getAllAttractions() {
    return this.data.map(attraction => Attraction.fromJSON(attraction));
  }

  /**
   * Получает аттракцион по ID
   */
  getAttractionById(id) {
    const data = this.getById(id);
    return data ? Attraction.fromJSON(data) : null;
  }

  /**
   * Создает новый аттракцион
   */
  createAttraction(attractionData) {
    const attraction = new Attraction(
      attractionData.id || this.generateId(),
      attractionData.name,
      attractionData.description,
      attractionData.type,
      attractionData.capacity,
      attractionData.status || 'active',
      attractionData.currentVisitors || 0,
      attractionData.waitTime || 0
    );

    return this.create(attraction.toJSON());
  }

  /**
   * Обновляет аттракцион
   */
  updateAttraction(id, updates) {
    const updated = this.update(id, updates);
    return updated ? Attraction.fromJSON(updated) : null;
  }

  /**
   * Получает активные аттракционы
   */
  getActiveAttractions() {
    return this.data
      .filter(attraction => attraction.status === 'active')
      .map(attraction => Attraction.fromJSON(attraction));
  }

  /**
   * Получает аттракционы по типу
   */
  getAttractionsByType(type) {
    return this.data
      .filter(attraction => attraction.type === type)
      .map(attraction => Attraction.fromJSON(attraction));
  }

  /**
   * Получает аттракционы с доступными местами
   */
  getAvailableAttractions() {
    return this.data
      .filter(attraction => 
        attraction.status === 'active' && 
        attraction.currentVisitors < attraction.capacity
      )
      .map(attraction => Attraction.fromJSON(attraction));
  }

  /**
   * Добавляет посетителя к аттракциону
   */
  addVisitorToAttraction(attractionId) {
    const attraction = this.getById(attractionId);
    if (!attraction) return false;

    if (attraction.status === 'active' && attraction.currentVisitors < attraction.capacity) {
      attraction.currentVisitors++;
      attraction.updatedAt = new Date().toISOString();
      this.saveToStorage();
      return true;
    }
    return false;
  }

  /**
   * Удаляет посетителя с аттракциона
   */
  removeVisitorFromAttraction(attractionId) {
    const attraction = this.getById(attractionId);
    if (!attraction) return false;

    if (attraction.currentVisitors > 0) {
      attraction.currentVisitors--;
      attraction.updatedAt = new Date().toISOString();
      this.saveToStorage();
      return true;
    }
    return false;
  }

  /**
   * Получает статистику по аттракционам
   */
  getAttractionsStats() {
    const total = this.data.length;
    const active = this.data.filter(a => a.status === 'active').length;
    const maintenance = this.data.filter(a => a.status === 'maintenance').length;
    const closed = this.data.filter(a => a.status === 'closed').length;
    const totalVisitors = this.data.reduce((sum, a) => sum + a.currentVisitors, 0);
    const totalCapacity = this.data.reduce((sum, a) => sum + a.capacity, 0);

    return {
      total,
      active,
      maintenance,
      closed,
      totalVisitors,
      totalCapacity,
      averageLoad: totalCapacity > 0 ? Math.round((totalVisitors / totalCapacity) * 100) : 0
    };
  }
}

export default AttractionRepository;
