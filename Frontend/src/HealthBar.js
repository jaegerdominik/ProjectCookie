import React from 'react';
import './HealthBar.css';

function HealthBar({ lives }) {
    return (
        <div className="healthbar-wrapper">
            <div className="health-info">{lives}/10</div> {/* Anzeige der Leben */}
            <div className="healthbar-container">
                {Array.from({ length: 10 }, (_, i) => (
                    <div
                        key={i}
                        className={`healthbar-life ${i < lives ? 'active' : 'lost'}`}
                    ></div>
                ))}
            </div>
        </div>
    );
}

export default HealthBar;