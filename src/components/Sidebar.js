import React from 'react';
import { NavLink } from 'react-router-dom';
import './Sidebar.css';

function Sidebar({ isOpen, onToggle }) {
  return (
    <aside className={`sidebar-container ${isOpen ? 'sidebar-open' : 'sidebar-closed'}`}>
      <div className="sidebar-logo">
        <h2 className="logo-text">🌊 Аквапарк</h2>
      </div>
      
      <nav className="sidebar-nav">
        <div className="nav-section">
          <h3 className="section-title">Основное</h3>
          <NavLink to="/" end className="nav-item">
            <div className="nav-icon">🏠</div>
            <span className="nav-text">Главная</span>
          </NavLink>
          <NavLink to="/attractions" className="nav-item">
            <div className="nav-icon">🌊</div>
            <span className="nav-text">Аттракционы</span>
          </NavLink>
          <NavLink to="/tickets" className="nav-item">
            <div className="nav-icon">🎫</div>
            <span className="nav-text">Билеты</span>
          </NavLink>
        </div>
        
        <div className="nav-section">
          <h3 className="section-title">Аналитика</h3>
          <NavLink to="/analytics" className="nav-item">
            <div className="nav-icon">📊</div>
            <span className="nav-text">Статистика</span>
          </NavLink>
          <NavLink to="/visitors" className="nav-item">
            <div className="nav-icon">👥</div>
            <span className="nav-text">Посетители</span>
          </NavLink>
        </div>
        
        <div className="nav-section">
          <h3 className="section-title">Настройки</h3>
          <NavLink to="/settings" className="nav-item">
            <div className="nav-icon">⚙️</div>
            <span className="nav-text">Настройки</span>
          </NavLink>
        </div>
      </nav>
    </aside>
  );
}

export default Sidebar;