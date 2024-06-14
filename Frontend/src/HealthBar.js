import React from 'react';
import './HealthBar.css';

function HealthBar({ lives }) {
    return (
        <div className="healthbar-container">
            {Array.from({ length: 10 }, (_, i) => (
                <div
                    key={i}
                    className={`healthbar-life ${i < lives ? 'active' : 'lost'}`}
                ></div>
            ))}
        </div>
    );
}

export default HealthBar;
