/**
 * Модель посетителя
 */
class Visitor {
  constructor(id, name, age, ticketId, entryTime, phone = null, email = null) {
    this.id = id;
    this.name = name;
    this.age = age;
    this.ticketId = ticketId;
    this.entryTime = entryTime;
    this.exitTime = null;
    this.phone = phone;
    this.email = email;
    this.currentAttractions = []; // ID аттракционов, где сейчас находится
    this.visitedAttractions = []; // ID аттракционов, которые посетил
    this.createdAt = new Date();
    this.updatedAt = new Date();
  }

  /**
   * Проверяет, является ли посетитель ребенком
   */
  isChild() {
    return this.age < 12;
  }

  /**
   * Проверяет, является ли посетитель взрослым
   */
  isAdult() {
    return this.age >= 18;
  }

  /**
   * Проверяет, является ли посетитель подростком
   */
  isTeenager() {
    return this.age >= 12 && this.age < 18;
  }

  /**
   * Добавляет аттракцион в список текущих
   */
  enterAttraction(attractionId) {
    if (!this.currentAttractions.includes(attractionId)) {
      this.currentAttractions.push(attractionId);
      this.updatedAt = new Date();
      return true;
    }
    return false;
  }

  /**
   * Удаляет аттракцион из списка текущих
   */
  exitAttraction(attractionId) {
    const index = this.currentAttractions.indexOf(attractionId);
    if (index > -1) {
      this.currentAttractions.splice(index, 1);
      if (!this.visitedAttractions.includes(attractionId)) {
        this.visitedAttractions.push(attractionId);
      }
      this.updatedAt = new Date();
      return true;
    }
    return false;
  }

  /**
   * Завершает посещение
   */
  finishVisit() {
    this.exitTime = new Date();
    this.currentAttractions = [];
    this.updatedAt = new Date();
  }

  /**
   * Получает время пребывания в минутах
   */
  getVisitDuration() {
    const endTime = this.exitTime || new Date();
    return Math.round((endTime - this.entryTime) / (1000 * 60));
  }

  /**
   * Получает количество посещенных аттракционов
   */
  getVisitedAttractionsCount() {
    return this.visitedAttractions.length;
  }

  /**
   * Проверяет, находится ли посетитель в аквапарке
   */
  isInPark() {
    return this.exitTime === null;
  }

  /**
   * Получает возрастную категорию
   */
  getAgeCategory() {
    if (this.isChild()) return 'child';
    if (this.isTeenager()) return 'teenager';
    if (this.isAdult()) return 'adult';
    return 'unknown';
  }

  /**
   * Конвертирует в JSON
   */
  toJSON() {
    return {
      id: this.id,
      name: this.name,
      age: this.age,
      ticketId: this.ticketId,
      entryTime: this.entryTime,
      exitTime: this.exitTime,
      phone: this.phone,
      email: this.email,
      currentAttractions: this.currentAttractions,
      visitedAttractions: this.visitedAttractions,
      createdAt: this.createdAt,
      updatedAt: this.updatedAt
    };
  }

  /**
   * Создает объект из JSON
   */
  static fromJSON(data) {
    const visitor = new Visitor(
      data.id,
      data.name,
      data.age,
      data.ticketId,
      new Date(data.entryTime),
      data.phone,
      data.email
    );
    visitor.exitTime = data.exitTime ? new Date(data.exitTime) : null;
    visitor.currentAttractions = data.currentAttractions || [];
    visitor.visitedAttractions = data.visitedAttractions || [];
    visitor.createdAt = new Date(data.createdAt);
    visitor.updatedAt = new Date(data.updatedAt);
    return visitor;
  }
}

export default Visitor;
