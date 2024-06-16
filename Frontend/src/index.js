import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import App from './App';
import GamePage from './GamePage';
import CreditsPage from './CreditsPage';
import reportWebVitals from './reportWebVitals';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <React.StrictMode>
        <Router>
            <Routes>
                <Route path="/" element={<App />} />
                <Route path="/game" element={<GamePage />} />
                <Route path="/credits" element={<CreditsPage />} />
            </Routes>
        </Router>
    </React.StrictMode>
);

reportWebVitals();
