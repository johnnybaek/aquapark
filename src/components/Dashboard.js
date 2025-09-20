import React, { useState, useEffect } from 'react';
import AquaparkService from '../bll/AquaparkService.js';
import './Dashboard.css';

function Dashboard() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      const [dashboardData, recentActivity] = await Promise.all([
        new AquaparkService().getDashboardData(),
        new AquaparkService().getRecentActivity()
      ]);
      
      setData({
        ...dashboardData,
        recentActivity
      });
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="dashboard-container">
        <div className="loading">Загрузка данных...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="dashboard-container">
        <div className="error">Ошибка: {error}</div>
      </div>
    );
  }

  const { stats, recentActivity } = data;

  return (
    <div className="dashboard-container">
      <div className="welcome-section">
        <h2 className="welcome-title">Добро пожаловать в систему управления!</h2>
        <p className="welcome-subtitle">
          Управляйте аквапарком "Морские Глубины" с помощью современной панели администратора
        </p>
      </div>

      <div className="stats-grid">
        <div className="stat-card">
          <div className="stat-header">
            <div className="stat-icon stat-icon-green">
              👥
            </div>
          </div>
          <div className="stat-value">{stats.summary.totalVisitorsToday}</div>
          <div className="stat-label">Посетителей сегодня</div>
          <div className="stat-change stat-change-positive">
            📈 В парке: {stats.summary.visitorsInPark}
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-header">
            <div className="stat-icon stat-icon-red">
              🎫
            </div>
          </div>
          <div className="stat-value">{stats.tickets.total}</div>
          <div className="stat-label">Типов билетов</div>
          <div className="stat-change stat-change-positive">
            📈 Доход: {stats.summary.totalRevenue} ₽
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-header">
            <div className="stat-icon stat-icon-blue">
              🌊
            </div>
          </div>
          <div className="stat-value">{stats.summary.activeAttractions}</div>
          <div className="stat-label">Активных аттракционов</div>
          <div className="stat-change stat-change-positive">
            📈 Загрузка: {stats.summary.averageLoad}%
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-header">
            <div className="stat-icon stat-icon-orange">
              ⏰
            </div>
          </div>
          <div className="stat-value">{Math.round(stats.visitors.averageVisitDuration / 60)}ч</div>
          <div className="stat-label">Среднее время посещения</div>
          <div className="stat-change stat-change-positive">
            📈 Средний возраст: {stats.visitors.averageAge} лет
          </div>
        </div>
      </div>

      <div className="recent-activity">
        <h3 className="section-title">Последняя активность</h3>
        <div className="activity-list">
          {recentActivity && recentActivity.length > 0 ? (
            recentActivity.slice(0, 5).map((activity, index) => (
              <div key={index} className="activity-item">
                <div className="activity-icon activity-icon-green">
                  {activity.icon}
                </div>
                <div className="activity-content">
                  <div className="activity-title">{activity.message}</div>
                  <div className="activity-time">
                    {new Date(activity.time).toLocaleString('ru-RU')}
                  </div>
                </div>
              </div>
            ))
          ) : (
            <div className="no-activity">Нет недавней активности</div>
          )}
        </div>
      </div>
    </div>
  );
}

export default Dashboard;