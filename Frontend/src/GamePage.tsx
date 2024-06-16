﻿import { useEffect, useState, useRef } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import './GamePage.css';
import cookieImage from './images/cookie.svg';
import figureImage from './images/figure.png';
import HealthBar from './HealthBar';

interface LocationState {
    username: string;
}

interface Highscore {
    username: string;
    score: number;
    time: string;
}

function GamePage() {
    const location = useLocation();
    const navigate = useNavigate();
    const { username } = (location.state as LocationState) || { username: 'Guest' };

    const [figures, setFigures] = useState<{ id: number; x: number; y: number }[]>([]);
    const [lives, setLives] = useState<number>(10);
    const [score, setScore] = useState<number>(0); // Score-Tracker
    const [time, setTime] = useState<number>(0); // Zeit-Tracker in Millisekunden
    const [isGameOver, setIsGameOver] = useState<boolean>(false);
    const [isGameStarted, setIsGameStarted] = useState<boolean>(false);
    const [highscores, setHighscores] = useState<Highscore[]>([
        { username: 'FirstUsername', score: 124, time: '00:03,00' },
        { username: 'SecondUsername', score: 14, time: '01:03,00' },
    ]); // Beispiel-Highscores
    const spawnInterval = useRef<number>(2000); // Start-Intervall: 2000ms
    const intervalId = useRef<NodeJS.Timeout | null>(null);
    const timerId = useRef<number | null>(null);

    const spawnFigure = () => {
        const centerX = window.innerWidth / 2;
        const centerY = window.innerHeight / 2;
        const radius = 600; // Abstand vom Cookie (doppelt so weit)

        const spawnPoints = Array.from({ length: 18 }, (_, i) => {
            const angle = (i * 2 * Math.PI) / 18;
            const x = centerX + radius * Math.cos(angle);
            const y = centerY + radius * Math.sin(angle);
            return { x, y };
        });

        const randomIndex = Math.floor(Math.random() * spawnPoints.length);
        const spawnPoint = spawnPoints[randomIndex];

        setFigures((figures) => [
            ...figures,
            { id: Date.now(), x: spawnPoint.x, y: spawnPoint.y }
        ]);

        // Verkürze das Intervall und setze es neu
        spawnInterval.current = Math.max(spawnInterval.current * 0.98, 500); // Minimum Intervall: 500ms
        if (intervalId.current) clearInterval(intervalId.current);
        intervalId.current = setInterval(spawnFigure, spawnInterval.current);
    };

    useEffect(() => {
        if (!isGameStarted || isGameOver) return;

        intervalId.current = setInterval(spawnFigure, spawnInterval.current);

        return () => {
            if (intervalId.current) clearInterval(intervalId.current);
        };
    }, [isGameStarted, isGameOver]);

    useEffect(() => {
        if (!isGameStarted || isGameOver) {
            if (timerId.current) cancelAnimationFrame(timerId.current);
            return;
        }

        const startTime = Date.now() - time; // Behalte die aktuelle Zeit bei

        const updateTimer = () => {
            setTime(Date.now() - startTime);
            timerId.current = requestAnimationFrame(updateTimer);
        };

        timerId.current = requestAnimationFrame(updateTimer);

        return () => {
            if (timerId.current) cancelAnimationFrame(timerId.current);
        };
    }, [isGameStarted, isGameOver, time]);

    const handleFigureClick = (id: number) => {
        setFigures((figures) => figures.filter((figure) => figure.id !== id));
        setScore((prevScore) => prevScore + 1); // Erhöhe den Score bei Klick
    };

    const handleAnimationEnd = (id: number) => {
        setFigures((figures) => figures.filter((figure) => figure.id !== id));
        setLives((prevLives) => {
            const newLives = Math.max(prevLives - 1, 0);
            if (newLives === 0) {
                setIsGameOver(true);
                if (intervalId.current) clearInterval(intervalId.current);
                if (timerId.current) cancelAnimationFrame(timerId.current);
            }
            return newLives;
        });
    };

    const formatTime = (milliseconds: number) => {
        const totalSeconds = Math.floor(milliseconds / 1000);
        const minutes = String(Math.floor(totalSeconds / 60)).padStart(2, '0');
        const seconds = String(totalSeconds % 60).padStart(2, '0');
        const centiseconds = String(Math.floor((milliseconds % 1000) / 10)).padStart(2, '0');
        return `${minutes}:${seconds},${centiseconds}`;
    };

    const handleRestart = () => {
        setFigures([]);
        setLives(10);
        setScore(0);
        setTime(0);
        setIsGameOver(false);
        setIsGameStarted(true);
        spawnInterval.current = 2000;

        if (intervalId.current) clearInterval(intervalId.current);
        intervalId.current = setInterval(spawnFigure, spawnInterval.current);

        const startTime = Date.now();
        const updateTimer = () => {
            setTime(Date.now() - startTime);
            timerId.current = requestAnimationFrame(updateTimer);
        };
        timerId.current = requestAnimationFrame(updateTimer);
    };

    const handleStart = () => {
        setIsGameStarted(true);
        handleRestart();

        // Entfernen Sie alle fallenden Cookies
        const fallingCookies = document.querySelectorAll('.falling-cookie');
        fallingCookies.forEach(cookie => cookie.remove());
    };

    const handleBackToMenu = () => {
        navigate('/');
    };

    const formatHighscoreEntry = (username: string, score: number, time: string) => {
        const maxLength = 50; // Maximale Länge der Punkte-Linie
        const scoreStr = String(score);
        const timeStr = `${time} (${scoreStr})`;
        const dotsLength = maxLength - username.length - timeStr.length;
        const dots = '.'.repeat(dotsLength > 0 ? dotsLength : 0);

        return (
            <div className="highscore-entry">
                <span className="highscore-username">{username}</span>
                <span className="highscore-dots">{dots}</span>
                <span className="highscore-time">{timeStr}</span>
            </div>
        );
    };

    return (
        <div className="container">
            <div className="info">
                <div className="username">Hello, {username}</div>
                <div className="score">Score: {score}</div>
                <div className="time">Zeit: {formatTime(time)}</div> {/* Anzeige der Zeit */}
            </div>
            <div className="highscore">
                <div className="highscore-title">Highscore:</div>
                <ul className="highscore-list">
                    {highscores.map((entry, index) => (
                        <li key={index}>
                            {formatHighscoreEntry(entry.username, entry.score, entry.time)}
                        </li>
                    ))}
                </ul>
            </div>
            <img src={cookieImage} alt="Cookie" className="cookie-image" />
            {!isGameOver && figures.map((figure) => {
                const deltaX = window.innerWidth / 2 - figure.x;
                const deltaY = window.innerHeight / 2 - figure.y;
                return (
                    <img
                        key={figure.id}
                        src={figureImage}
                        alt="Figure"
                        className="figure"
                        onClick={() => handleFigureClick(figure.id)}
                        onAnimationEnd={() => handleAnimationEnd(figure.id)}
                        style={{
                            left: `${figure.x}px`,
                            top: `${figure.y}px`,
                            animation: `moveToCookie 5s linear forwards`,
                            transform: `translate(${deltaX}px, ${deltaY}px)`,
                        }}
                    />
                );
            })}
            <HealthBar lives={lives} />
            {(!isGameStarted || isGameOver) && (
                <div className="game-over-container">
                    {!isGameStarted ? (
                        <>
                            <div className="game-over">
                                Klicke auf die Figuren, die deinen Cookie wegessen wollen.
                                Wenn 10 Figuren den Cookie erreichen, ist das Spiel vorbei.
                            </div>
                            <button className="restart-button" onClick={handleStart}>Start</button>
                        </>
                    ) : (
                        <>
                            <div className="game-over">Game Over</div>
                            <button className="restart-button" onClick={handleRestart}>Restart</button>
                            <button className="menu-button" onClick={handleBackToMenu}>Zurück zum Hauptmenü</button>
                        </>
                    )}
                </div>
            )}
            <div className="copyright">
                &copy; Martin Haring, Dominik Jäger, Raphael Klein
            </div>
        </div>
    );
}

export default GamePage;
