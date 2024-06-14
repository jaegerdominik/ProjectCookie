import './App.css';
import { useNavigate } from 'react-router-dom';
import React, { useState } from 'react';

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

    return (
        <div className="container">
            <h1 className="title">Welcome</h1>
            <input
                type="text"
                placeholder="Enter your username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                className="input"
            />
            <button onClick={handleJoin} className="button">
                Join
            </button>
        </div>
    );
}

export default App;
