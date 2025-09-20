import React from 'react';
import './Header.css';

function Header({ onMenuClick }) {
  return (
    <header className="header-container">
      <div className="header-left">
        <button className="menu-button" onClick={onMenuClick}>
          ☰
        </button>
        <h1 className="header-title">Аквапарк "Морские Глубины"</h1>
      </div>
      <div className="header-right">
        <button className="notification-button">
          🔔
          <span className="notification-badge">3</span>
        </button>
        <button className="user-button">
          👤 Администратор
        </button>
      </div>
    </header>
  );
}

export default Header;