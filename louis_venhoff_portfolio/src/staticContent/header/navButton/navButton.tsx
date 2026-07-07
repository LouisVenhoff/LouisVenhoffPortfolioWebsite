import { Link } from "react-router-dom";
import "../../../styles/staticContent/header.css";

type NavButtonProps = {
    title: String;
    target: string;
};

const NavButton:React.FC<NavButtonProps> = ({title, target}) => {

    return(
            <div className="header-nav-button--background">
                <Link to={`/${target}`}>{title}</Link>
            </div>
    );
};

export default NavButton;