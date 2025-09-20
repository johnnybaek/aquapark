import React, { useState, useEffect } from 'react';
import TicketService from '../bll/TicketService.js';
import './Tickets.css';

function Tickets() {
  const [tickets, setTickets] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedType, setSelectedType] = useState('all');

  useEffect(() => {
    loadTickets();
  }, []);

  const loadTickets = async () => {
    try {
      setLoading(true);
      const ticketService = new TicketService();
      const data = await ticketService.getTicketsData();
      setTickets(data.tickets);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = async (query) => {
    try {
      setLoading(true);
      const ticketService = new TicketService();
      const results = await ticketService.searchTickets(query);
      setTickets(results);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleSellTicket = async (ticketId) => {
    try {
      const visitorData = {
        name: prompt('Введите имя посетителя:'),
        age: parseInt(prompt('Введите возраст:')),
        phone: prompt('Введите телефон (необязательно):') || null,
        email: prompt('Введите email (необязательно):') || null,
        visitorCount: 1
      };

      if (!visitorData.name || !visitorData.age) {
        alert('Имя и возраст обязательны!');
        return;
      }

      const ticketService = new TicketService();
      const result = await ticketService.sellTicket(ticketId, visitorData);
      
      alert(`Билет продан! Общая стоимость: ${result.totalPrice} ₽`);
      await loadTickets(); // Перезагружаем данные
    } catch (err) {
      alert(`Ошибка продажи билета: ${err.message}`);
    }
  };

  const getTypeText = (type) => {
    switch(type) {
      case 'adult': return 'Взрослый';
      case 'child': return 'Детский';
      case 'family': return 'Семейный';
      case 'vip': return 'VIP';
      case 'group': return 'Групповой';
      default: return 'Стандартный';
    }
  };

  const getTypeColor = (type) => {
    switch(type) {
      case 'adult': return 'green';
      case 'child': return 'blue';
      case 'family': return 'purple';
      case 'vip': return 'yellow';
      case 'group': return 'orange';
      default: return 'gray';
    }
  };

  const filteredTickets = tickets.filter(ticket => {
    const matchesSearch = searchTerm === '' || 
      ticket.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      ticket.description.toLowerCase().includes(searchTerm.toLowerCase());
    
    const matchesType = selectedType === 'all' || ticket.type === selectedType;
    
    return matchesSearch && matchesType;
  });

  if (loading) {
    return (
      <div className="tickets-container">
        <div className="loading">Загрузка билетов...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="tickets-container">
        <div className="error">Ошибка: {error}</div>
      </div>
    );
  }

  return (
    <div className="tickets-container">
      <div className="tickets-header">
        <h2 className="tickets-title">Управление билетами</h2>
        <p className="tickets-subtitle">
          Продажа и управление различными типами билетов аквапарка
        </p>
      </div>

      <div className="tickets-controls">
        <div className="search-input">
          <div className="search-icon">🔍</div>
          <input
            type="text"
            placeholder="Поиск билетов..."
            value={searchTerm}
            onChange={(e) => {
              setSearchTerm(e.target.value);
              handleSearch(e.target.value);
            }}
            className="search-field"
          />
        </div>
        
        <select 
          value={selectedType} 
          onChange={(e) => setSelectedType(e.target.value)}
          className="type-filter"
        >
          <option value="all">Все типы</option>
          <option value="adult">Взрослые</option>
          <option value="child">Детские</option>
          <option value="family">Семейные</option>
          <option value="vip">VIP</option>
          <option value="group">Групповые</option>
        </select>
        
        <button className="add-button">
          ➕ Новый билет
        </button>
      </div>

      <div className="tickets-grid">
        {filteredTickets.map(ticket => (
          <div key={ticket.id} className="ticket-card">
            <div className="ticket-header">
              <div className={`ticket-icon ticket-icon-${getTypeColor(ticket.type)}`}>
                🎫
              </div>
              <div className={`ticket-type ticket-type-${ticket.type}`}>
                {getTypeText(ticket.type)}
              </div>
            </div>

            <div className="ticket-info">
              <h3 className="ticket-name">{ticket.name}</h3>
              <p className="ticket-description">{ticket.description}</p>
              <div className="ticket-price">
                {ticket.getFinalPrice ? ticket.getFinalPrice() : ticket.price} ₽
                {ticket.getDiscountPercentage && ticket.getDiscountPercentage() > 0 && (
                  <span className="discount">
                    (скидка {ticket.getDiscountPercentage()}%)
                  </span>
                )}
              </div>
              <div className="ticket-details">
                <div className="detail-item">
                  <strong>Макс. посетителей:</strong> {ticket.maxVisitors}
                </div>
                <div className="detail-item">
                  <strong>Действует до:</strong> {new Date(ticket.validTo).toLocaleDateString('ru-RU')}
                </div>
              </div>
            </div>

            <div className="ticket-actions">
              <button className="action-button action-secondary">
                👁️ Просмотр
              </button>
              <button 
                className="action-button action-primary"
                onClick={() => handleSellTicket(ticket.id)}
              >
                💳 Продать
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default Tickets;