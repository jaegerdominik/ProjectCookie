import './App.css';
import { useNavigate } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import cookieImage from './images/cookie.svg';

function App() {
    const [username, setUsername] = useState('');
    const navigate = useNavigate();

    const handleJoin = () => {
        if (username) {
            navigate('/game', { state: { username } });
        } else {
            alert('Please enter a username');
        }
    };

    useEffect(() => {
        const createCookie = () => {
            const cookie = document.createElement('div');
            cookie.className = 'falling-cookie';
            cookie.style.left = `${Math.random() * 100}vw`;
            cookie.style.animationDuration = `${2 + Math.random() * 3}s`;
            document.body.appendChild(cookie);

            setTimeout(() => {
                cookie.remove();
            }, 5000);
        };

        const intervalId = setInterval(createCookie, 300);

        return () => clearInterval(intervalId);
    }, []);

    return (
        <div className="container">
            <div className="cookie-animation-container">
                {/* Cookies werden hier animiert */}
            </div>
            <img src={cookieImage} alt="Cookie Logo" className="cookie-logo" />
            <h1 className="title">Welcome to Cookie Clicker</h1>
            <input
                type="text"
                placeholder="Enter your username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                maxLength="20" // Limitiert die Länge auf 20 Zeichen
                className="input"
            />
            <button onClick={handleJoin} className="button">
                Join
            </button>
        </div>
    );
}

export default App;
