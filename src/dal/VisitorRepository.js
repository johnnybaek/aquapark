import BaseRepository from './BaseRepository.js';
import Visitor from '../models/Visitor.js';

/**
 * Репозиторий для работы с посетителями
 */
class VisitorRepository extends BaseRepository {
  constructor() {
    super('aquapark_visitors');
  }

  /**
   * Получает всех посетителей
   */
  getAllVisitors() {
    return this.data.map(visitor => Visitor.fromJSON(visitor));
  }

  /**
   * Получает посетителя по ID
   */
  getVisitorById(id) {
    const data = this.getById(id);
    return data ? Visitor.fromJSON(data) : null;
  }

  /**
   * Создает нового посетителя
   */
  createVisitor(visitorData) {
    const visitor = new Visitor(
      visitorData.id || this.generateId(),
      visitorData.name,
      visitorData.age,
      visitorData.ticketId,
      new Date(visitorData.entryTime || Date.now()),
      visitorData.phone,
      visitorData.email
    );

    return this.create(visitor.toJSON());
  }

  /**
   * Обновляет посетителя
   */
  updateVisitor(id, updates) {
    const updated = this.update(id, updates);
    return updated ? Visitor.fromJSON(updated) : null;
  }

  /**
   * Получает посетителей, находящихся в парке
   */
  getVisitorsInPark() {
    return this.data
      .filter(visitor => visitor.exitTime === null)
      .map(visitor => Visitor.fromJSON(visitor));
  }

  /**
   * Получает посетителей по возрастной категории
   */
  getVisitorsByAgeCategory(category) {
    return this.data
      .filter(visitor => {
        const age = visitor.age;
        switch (category) {
          case 'child':
            return age < 12;
          case 'teenager':
            return age >= 12 && age < 18;
          case 'adult':
            return age >= 18;
          default:
            return false;
        }
      })
      .map(visitor => Visitor.fromJSON(visitor));
  }

  /**
   * Получает посетителей по типу билета
   */
  getVisitorsByTicketType(ticketType) {
    return this.data
      .filter(visitor => visitor.ticketId === ticketType)
      .map(visitor => Visitor.fromJSON(visitor));
  }

  /**
   * Получает посетителей за период
   */
  getVisitorsByPeriod(startDate, endDate) {
    const start = new Date(startDate);
    const end = new Date(endDate);
    
    return this.data
      .filter(visitor => {
        const entryTime = new Date(visitor.entryTime);
        return entryTime >= start && entryTime <= end;
      })
      .map(visitor => Visitor.fromJSON(visitor));
  }

  /**
   * Получает посетителей за сегодня
   */
  getTodayVisitors() {
    const today = new Date();
    const startOfDay = new Date(today.getFullYear(), today.getMonth(), today.getDate());
    const endOfDay = new Date(startOfDay.getTime() + 24 * 60 * 60 * 1000);
    
    return this.getVisitorsByPeriod(startOfDay, endOfDay);
  }

  /**
   * Добавляет посетителя к аттракциону
   */
  addVisitorToAttraction(visitorId, attractionId) {
    const visitor = this.getById(visitorId);
    if (!visitor) return false;

    if (!visitor.currentAttractions.includes(attractionId)) {
      visitor.currentAttractions.push(attractionId);
      visitor.updatedAt = new Date().toISOString();
      this.saveToStorage();
      return true;
    }
    return false;
  }

  /**
   * Удаляет посетителя с аттракциона
   */
  removeVisitorFromAttraction(visitorId, attractionId) {
    const visitor = this.getById(visitorId);
    if (!visitor) return false;

    const index = visitor.currentAttractions.indexOf(attractionId);
    if (index > -1) {
      visitor.currentAttractions.splice(index, 1);
      if (!visitor.visitedAttractions.includes(attractionId)) {
        visitor.visitedAttractions.push(attractionId);
      }
      visitor.updatedAt = new Date().toISOString();
      this.saveToStorage();
      return true;
    }
    return false;
  }

  /**
   * Завершает посещение
   */
  finishVisit(visitorId) {
    const visitor = this.getById(visitorId);
    if (!visitor) return false;

    visitor.exitTime = new Date().toISOString();
    visitor.currentAttractions = [];
    visitor.updatedAt = new Date().toISOString();
    this.saveToStorage();
    return true;
  }

  /**
   * Получает статистику по посетителям
   */
  getVisitorsStats() {
    const total = this.data.length;
    const inPark = this.getVisitorsInPark().length;
    const today = this.getTodayVisitors().length;
    
    const byAgeCategory = {
      child: this.getVisitorsByAgeCategory('child').length,
      teenager: this.getVisitorsByAgeCategory('teenager').length,
      adult: this.getVisitorsByAgeCategory('adult').length
    };

    const averageAge = total > 0 ? 
      Math.round(this.data.reduce((sum, v) => sum + v.age, 0) / total) : 0;

    const averageVisitDuration = total > 0 ?
      Math.round(this.data.reduce((sum, v) => {
        const entry = new Date(v.entryTime);
        const exit = v.exitTime ? new Date(v.exitTime) : new Date();
        return sum + (exit - entry) / (1000 * 60); // в минутах
      }, 0) / total) : 0;

    return {
      total,
      inPark,
      today,
      byAgeCategory,
      averageAge,
      averageVisitDuration
    };
  }

  /**
   * Получает самых активных посетителей
   */
  getMostActiveVisitors(limit = 5) {
    return this.data
      .map(visitor => ({
        ...visitor,
        visitedCount: visitor.visitedAttractions ? visitor.visitedAttractions.length : 0
      }))
      .sort((a, b) => b.visitedCount - a.visitedCount)
      .slice(0, limit)
      .map(visitor => Visitor.fromJSON(visitor));
  }
}

export default VisitorRepository;
