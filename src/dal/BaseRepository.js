/**
 * Базовый репозиторий для работы с данными
 */
class BaseRepository {
  constructor(storageKey) {
    this.storageKey = storageKey;
    this.data = this.loadFromStorage();
  }

  /**
   * Загружает данные из localStorage
   */
  loadFromStorage() {
    try {
      const stored = localStorage.getItem(this.storageKey);
      return stored ? JSON.parse(stored) : [];
    } catch (error) {
      console.error(`Ошибка загрузки данных для ${this.storageKey}:`, error);
      return [];
    }
  }

  /**
   * Сохраняет данные в localStorage
   */
  saveToStorage() {
    try {
      localStorage.setItem(this.storageKey, JSON.stringify(this.data));
      return true;
    } catch (error) {
      console.error(`Ошибка сохранения данных для ${this.storageKey}:`, error);
      return false;
    }
  }

  /**
   * Генерирует новый ID
   */
  generateId() {
    return Date.now().toString() + Math.random().toString(36).substr(2, 9);
  }

  /**
   * Получает все записи
   */
  getAll() {
    return [...this.data];
  }

  /**
   * Получает запись по ID
   */
  getById(id) {
    return this.data.find(item => item.id === id);
  }

  /**
   * Добавляет новую запись
   */
  create(item) {
    const newItem = {
      ...item,
      id: item.id || this.generateId(),
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString()
    };
    
    this.data.push(newItem);
    this.saveToStorage();
    return newItem;
  }

  /**
   * Обновляет запись
   */
  update(id, updates) {
    const index = this.data.findIndex(item => item.id === id);
    if (index === -1) {
      return null;
    }

    this.data[index] = {
      ...this.data[index],
      ...updates,
      id: id, // ID не должен изменяться
      updatedAt: new Date().toISOString()
    };

    this.saveToStorage();
    return this.data[index];
  }

  /**
   * Удаляет запись
   */
  delete(id) {
    const index = this.data.findIndex(item => item.id === id);
    if (index === -1) {
      return false;
    }

    this.data.splice(index, 1);
    this.saveToStorage();
    return true;
  }

  /**
   * Поиск записей по условию
   */
  findBy(predicate) {
    return this.data.filter(predicate);
  }

  /**
   * Получает количество записей
   */
  count() {
    return this.data.length;
  }

  /**
   * Очищает все данные
   */
  clear() {
    this.data = [];
    this.saveToStorage();
    return true;
  }

  /**
   * Проверяет существование записи
   */
  exists(id) {
    return this.data.some(item => item.id === id);
  }
}

export default BaseRepository;
