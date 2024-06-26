﻿import { useEffect, useState, useRef } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import './GamePage.css';
import cookieImage from './images/cookie.svg';
import figureImage from './images/figure.png';
import HealthBar from './HealthBar';

interface LocationState {
    username: string;
}

interface IScore {
    id: number;
    points: number;
    timestamp: string;
    fK_User: number;
}

interface IUser {
    id: number;
    username: string;
}

function GamePage() {
    const location = useLocation();
    const navigate = useNavigate();
    const { username } = (location.state as LocationState) || { username: 'Guest' };

    const [figures, setFigures] = useState<{ id: number; x: number; y: number }[]>([]);
    const [lives, setLives] = useState<number>(10);
    const [score, setScore] = useState<number>(0);
    const [time, setTime] = useState<number>(0);
    const [isGameOver, setIsGameOver] = useState<boolean>(false);
    const [areScoresSent, setAreScoresSent] = useState<boolean>(false);
    const [isGameStarted, setIsGameStarted] = useState<boolean>(false);
    const [currentScores, setCurrentScores] = useState<IScore[]>([]);
    const [currentUsers, setCurrentUsers] = useState<IUser[]>([]);
    const [error, setError] = useState<string | null>(null);
    const spawnInterval = useRef<number>(2000);
    const intervalId = useRef<NodeJS.Timeout | null>(null);
    const timerId = useRef<number | null>(null);

    const spawnFigure = () => {
        const centerX = window.innerWidth / 2;
        const centerY = window.innerHeight / 2;
        const radius = 600;

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

        spawnInterval.current = Math.max(spawnInterval.current * 0.98, 500);
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

        const startTime = Date.now() - time;

        const updateTimer = () => {
            setTime(Date.now() - startTime);
            timerId.current = requestAnimationFrame(updateTimer);
        };

        timerId.current = requestAnimationFrame(updateTimer);

        return () => {
            if (timerId.current) cancelAnimationFrame(timerId.current);
        };
    }, [isGameStarted, isGameOver, time]);

    const fetchScores = () => {
        fetch(`https://localhost:7031/api/cookie/scores`)
            .then(response => response.json())
            .then((fetchedScores: IScore[]) => {
                const sortedScores = fetchedScores.sort((a, b) => b.points - a.points).slice(0, 10);
                setCurrentScores(sortedScores);
            })
            .catch(error => {
                setError("There was an error fetching the scores!");
                console.error(error);
            });
    };

    useEffect(() => {
        fetchScores();
    }, []);

    useEffect(() => {
        fetch(`https://localhost:7031/api/cookie/users`)
            .then(response => response.json())
            .then((fetchedUsers: IUser[]) => setCurrentUsers(fetchedUsers))
            .catch(error => {
                setError("There was an error fetching the users!");
                console.error(error);
            });
    }, []);

    useEffect(() => {
        if (isGameOver && !areScoresSent) {
            fetch(`https://localhost:7031/api/cookie/publish/${score}|${formatTime(time)}|${username}`, {
                method: 'PUT',
            }).catch(e => console.log(e));

            setAreScoresSent(true);
        }
    }, [isGameOver, areScoresSent, score, time, username]);

    if (error) return <div>{error}</div>;
    if (!currentScores.length) return <div>Loading scores...</div>;
    if (!currentUsers.length) return <div>Loading users...</div>;

    const handleFigureClick = (id: number) => {
        setFigures((figures) => figures.filter((figure) => figure.id !== id));
        setScore((prevScore) => prevScore + 1);
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
        setAreScoresSent(false);
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

        fetchScores(); // Update the scoreboard when restarting
    };

    const handleStart = () => {
        setIsGameStarted(true);
        handleRestart();

        const fallingCookies = document.querySelectorAll('.falling-cookie');
        fallingCookies.forEach(cookie => cookie.remove());
    };

    const handleBackToMenu = () => {
        navigate('/');
    };

    const formatHighscoreEntry = (score: IScore) => {
        const username = getNameById(currentScores, currentUsers, score.id) || 'Unknown';
        const timeStr = `${formatTime(parseTime(score.timestamp))}`;
        return (
            <>
                <span className="highscore-username">{username}</span>
                <span className="highscore-score">{score.points}</span>
                <span className="highscore-time">{timeStr}</span>
            </>
        );
    };

    function getNameById(listA: IScore[], listB: IUser[], id: number): string | undefined {
        const itemA = listA.find(item => item.id === id);

        if (itemA) {
            const itemB = listB.find(item => item.id === itemA.fK_User);
            return itemB ? itemB.username : undefined;
        }

        return undefined;
    }

    const parseTime = (timeStr: string): number => {
        const [minutes, seconds, centiseconds] = timeStr.split(/[:.,]/).map(Number);
        return (minutes * 60 * 1000) + (seconds * 1000) + (centiseconds * 10);
    };

    return (
        <div className="container">
            <div className="info">
                <div className="username">Hello, {username}</div>
                <div className="score">Score: {score}</div>
                <div className="time">Zeit: {formatTime(time)}</div>
            </div>
            <div className="highscore">
                <div className="highscore-title">Highscore:</div>
                <ul className="highscore-list">
                    <li className="highscore-entry">
                        <span className="highscore-username">Username</span>
                        <span className="highscore-score">Score</span>
                        <span className="highscore-time">Time</span>
                    </li>
                    {currentScores.map((entry, index) => (
                        <li key={index} className="highscore-entry">
                            {formatHighscoreEntry(entry)}
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
