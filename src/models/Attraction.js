/**
 * Модель аттракциона
 */
class Attraction {
  constructor(id, name, description, type, capacity, status = 'active', currentVisitors = 0, waitTime = 0) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.type = type; // 'water_slide', 'pool', 'spa', 'kids_zone'
    this.capacity = capacity;
    this.status = status; // 'active', 'maintenance', 'closed'
    this.currentVisitors = currentVisitors;
    this.waitTime = waitTime;
    this.createdAt = new Date();
    this.updatedAt = new Date();
  }

  /**
   * Проверяет, доступен ли аттракцион
   */
  isAvailable() {
    return this.status === 'active' && this.currentVisitors < this.capacity;
  }

  /**
   * Добавляет посетителя
   */
  addVisitor() {
    if (this.isAvailable()) {
      this.currentVisitors++;
      this.updatedAt = new Date();
      return true;
    }
    return false;
  }

  /**
   * Удаляет посетителя
   */
  removeVisitor() {
    if (this.currentVisitors > 0) {
      this.currentVisitors--;
      this.updatedAt = new Date();
      return true;
    }
    return false;
  }

  /**
   * Устанавливает статус
   */
  setStatus(status) {
    this.status = status;
    this.updatedAt = new Date();
  }

  /**
   * Получает загрузку в процентах
   */
  getLoadPercentage() {
    return Math.round((this.currentVisitors / this.capacity) * 100);
  }

  /**
   * Конвертирует в JSON
   */
  toJSON() {
    return {
      id: this.id,
      name: this.name,
      description: this.description,
      type: this.type,
      capacity: this.capacity,
      status: this.status,
      currentVisitors: this.currentVisitors,
      waitTime: this.waitTime,
      createdAt: this.createdAt,
      updatedAt: this.updatedAt
    };
  }

  /**
   * Создает объект из JSON
   */
  static fromJSON(data) {
    const attraction = new Attraction(
      data.id,
      data.name,
      data.description,
      data.type,
      data.capacity,
      data.status,
      data.currentVisitors,
      data.waitTime
    );
    attraction.createdAt = new Date(data.createdAt);
    attraction.updatedAt = new Date(data.updatedAt);
    return attraction;
  }
}

export default Attraction;
