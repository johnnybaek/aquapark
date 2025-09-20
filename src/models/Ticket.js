/**
 * Модель билета
 */
class Ticket {
  constructor(id, type, name, description, price, validFrom, validTo, maxVisitors = 1) {
    this.id = id;
    this.type = type; // 'adult', 'child', 'family', 'vip', 'group'
    this.name = name;
    this.description = description;
    this.price = price;
    this.validFrom = validFrom; // время начала действия
    this.validTo = validTo; // время окончания действия
    this.maxVisitors = maxVisitors;
    this.createdAt = new Date();
    this.updatedAt = new Date();
  }

  /**
   * Проверяет, действителен ли билет в указанное время
   */
  isValidAt(date = new Date()) {
    return date >= this.validFrom && date <= this.validTo;
  }

  /**
   * Проверяет, действителен ли билет сейчас
   */
  isValidNow() {
    return this.isValidAt(new Date());
  }

  /**
   * Получает скидку в процентах
   */
  getDiscountPercentage() {
    switch (this.type) {
      case 'family':
        return 20;
      case 'group':
        return 15;
      case 'vip':
        return 0; // VIP без скидки, но с дополнительными услугами
      default:
        return 0;
    }
  }

  /**
   * Получает финальную цену со скидкой
   */
  getFinalPrice() {
    const discount = this.getDiscountPercentage();
    return Math.round(this.price * (1 - discount / 100));
  }

  /**
   * Проверяет, подходит ли билет для указанного количества посетителей
   */
  canAccommodate(visitorCount) {
    return visitorCount <= this.maxVisitors;
  }

  /**
   * Получает тип билета на русском языке
   */
  getTypeInRussian() {
    const types = {
      'adult': 'Взрослый',
      'child': 'Детский',
      'family': 'Семейный',
      'vip': 'VIP',
      'group': 'Групповой'
    };
    return types[this.type] || 'Неизвестный';
  }

  /**
   * Конвертирует в JSON
   */
  toJSON() {
    return {
      id: this.id,
      type: this.type,
      name: this.name,
      description: this.description,
      price: this.price,
      validFrom: this.validFrom,
      validTo: this.validTo,
      maxVisitors: this.maxVisitors,
      createdAt: this.createdAt,
      updatedAt: this.updatedAt
    };
  }

  /**
   * Создает объект из JSON
   */
  static fromJSON(data) {
    const ticket = new Ticket(
      data.id,
      data.type,
      data.name,
      data.description,
      data.price,
      new Date(data.validFrom),
      new Date(data.validTo),
      data.maxVisitors
    );
    ticket.createdAt = new Date(data.createdAt);
    ticket.updatedAt = new Date(data.updatedAt);
    return ticket;
  }
}

export default Ticket;
