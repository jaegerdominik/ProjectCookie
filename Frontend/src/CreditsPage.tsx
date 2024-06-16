import { useNavigate } from 'react-router-dom';
import './CreditsPage.css';
import glumandaImage from './images/glumanda.png';
import shiggyImage from './images/schiggy.png';
import bisasamImage from './images/bisasam.png';

function CreditsPage() {
    const navigate = useNavigate();

    const handleBack = () => {
        navigate('/');
    };

    return (
        <div className="credits-container">
            <h1>Credits</h1>
            <p>Developed by:</p>
            <ul>
                <li>
                    <img src={glumandaImage} alt="Glumanda Icon" className="developer-icon" />
                    Martin Haring
                </li>
                <li>
                    <img src={shiggyImage} alt="Shiggy Icon" className="developer-icon" />
                    Dominik Jäger
                </li>
                <li>
                    <img src={bisasamImage} alt="Bisasam Icon" className="developer-icon" />
                    Raphael Klein
                </li>
            </ul>
            <button onClick={handleBack} className="button">
                Back to Main Menu
            </button>
        </div>
    );
}

export default CreditsPage;
