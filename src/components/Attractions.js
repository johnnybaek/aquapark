import React, { useState, useEffect } from 'react';
import AttractionService from '../bll/AttractionService.js';
import './Attractions.css';

function Attractions() {
  const [attractions, setAttractions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedStatus, setSelectedStatus] = useState('all');

  useEffect(() => {
    loadAttractions();
  }, []);

  const loadAttractions = async () => {
    try {
      setLoading(true);
      const attractionService = new AttractionService();
      const data = await attractionService.getAttractionsData();
      setAttractions(data.attractions);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleStatusChange = async (attractionId, newStatus) => {
    try {
      const attractionService = new AttractionService();
      await attractionService.changeAttractionStatus(attractionId, newStatus);
      await loadAttractions(); // Перезагружаем данные
    } catch (err) {
      setError(err.message);
    }
  };

  const getStatusText = (status) => {
    switch(status) {
      case 'active': return 'Работает';
      case 'maintenance': return 'Обслуживание';
      case 'closed': return 'Закрыто';
      default: return 'Неизвестно';
    }
  };

  const getStatusIcon = (status) => {
    switch(status) {
      case 'active': return '✅';
      case 'maintenance': return '🔧';
      case 'closed': return '❌';
      default: return '❓';
    }
  };

  const getTypeColor = (type) => {
    switch(type) {
      case 'water_slide': return 'red';
      case 'pool': return 'blue';
      case 'spa': return 'green';
      case 'kids_zone': return 'orange';
      default: return 'purple';
    }
  };

  const filteredAttractions = selectedStatus === 'all' 
    ? attractions 
    : attractions.filter(attraction => attraction.status === selectedStatus);

  if (loading) {
    return (
      <div className="attractions-container">
        <div className="loading">Загрузка аттракционов...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="attractions-container">
        <div className="error">Ошибка: {error}</div>
      </div>
    );
  }

  return (
    <div className="attractions-container">
      <div className="attractions-header">
        <h2 className="attractions-title">Управление аттракционами</h2>
        <p className="attractions-subtitle">
          Мониторинг состояния и управление всеми аттракционами аквапарка
        </p>
        
        <div className="filter-controls">
          <select 
            value={selectedStatus} 
            onChange={(e) => setSelectedStatus(e.target.value)}
            className="status-filter"
          >
            <option value="all">Все аттракционы</option>
            <option value="active">Работающие</option>
            <option value="maintenance">На обслуживании</option>
            <option value="closed">Закрытые</option>
          </select>
        </div>
      </div>

      <div className="attractions-grid">
        {filteredAttractions.map(attraction => (
          <div key={attraction.id} className="attraction-card">
            <div className="attraction-header">
              <div className={`attraction-icon attraction-icon-${getTypeColor(attraction.type)}`}>
                🌊
              </div>
              <div className={`status-badge status-${attraction.status}`}>
                {getStatusIcon(attraction.status)}
                {getStatusText(attraction.status)}
              </div>
            </div>

            <h3 className="attraction-name">{attraction.name}</h3>
            <p className="attraction-description">{attraction.description}</p>

            <div className="attraction-stats">
              <div className="stat">
                <div className="stat-value">{attraction.currentVisitors}</div>
                <div className="stat-label">Посетителей</div>
              </div>
              <div className="stat">
                <div className="stat-value">{attraction.capacity}</div>
                <div className="stat-label">Вместимость</div>
              </div>
              <div className="stat">
                <div className="stat-value">{attraction.waitTime} мин</div>
                <div className="stat-label">Ожидание</div>
              </div>
            </div>

            <div className="attraction-actions">
              <button 
                className={`action-button ${attraction.status === 'active' ? 'action-primary' : 'action-secondary'}`}
                onClick={() => {
                  const newStatus = attraction.status === 'active' ? 'maintenance' : 'active';
                  handleStatusChange(attraction.id, newStatus);
                }}
                disabled={attraction.status === 'closed'}
              >
                {attraction.status === 'active' ? 'На обслуживание' : 
                 attraction.status === 'maintenance' ? 'Запустить' : 'Закрыто'}
              </button>
              
              {attraction.status === 'active' && (
                <button 
                  className="action-button action-danger"
                  onClick={() => handleStatusChange(attraction.id, 'closed')}
                >
                  Закрыть
                </button>
              )}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default Attractions;