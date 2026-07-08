import ContentHeader from "../../components/contentHeader/contentHeader";
import Timeline, { TimelineItem } from "../../components/timeline/timeline";
import "../../styles/pages/about/about.css";
import portrait from "../../assets/Louis_LED.png";
import { FaBolt, FaCode, FaGraduationCap, FaLaptopCode, FaBookOpen } from "react-icons/fa6";
import { SiTypescript, SiJavascript, SiReact, SiRuby, SiRubyonrails, SiDotnet, SiCplusplus } from "react-icons/si";
import { TbBrandCSharp } from "react-icons/tb";

type TechIcon = {
    icon: React.ReactNode;
    name: string;
};

const techIcons: TechIcon[] = [
    { icon: <SiTypescript color="#3178C6" />, name: "TypeScript" },
    { icon: <SiJavascript color="#F7DF1E" />, name: "JavaScript" },
    { icon: <SiReact color="#61DAFB" />, name: "React" },
    { icon: <TbBrandCSharp color="#9B4F96" />, name: "C#" },
    { icon: <SiRuby color="#CC342D" />, name: "Ruby" },
    { icon: <SiRubyonrails color="#CC0000" />, name: "Ruby on Rails" },
    { icon: <SiDotnet color="#512BD4" />, name: ".NET Core" },
    { icon: <SiCplusplus color="#00599C" />, name: "C++" }
];

const timelineItems: TimelineItem[] = [
    {
        icon: <FaBolt />,
        title: "Elektroniker für Geräte und Systeme",
        subtitle: "heddier electronic GmbH",
        text: "Start meines beruflichen Werdegangs in der Elektronik."
    },
    {
        icon: <FaCode />,
        title: "Praktikum Softwareentwicklung",
        subtitle: "PFREUNDT GmbH",
        text: "Entdeckung meiner Leidenschaft für die Programmierung."
    },
    {
        icon: <FaGraduationCap />,
        title: "Ausbildung zum Fachinformatiker für Anwendungsentwicklung",
        subtitle: "PFREUNDT GmbH",
        text: "Wechsel in die Softwareentwicklung."
    },
    {
        icon: <FaLaptopCode />,
        title: "Softwareentwickler",
        subtitle: "PFREUNDT GmbH",
        date: "seit Januar 2024",
        text: "Fokus auf moderne Webentwicklung."
    },
    {
        icon: <FaBookOpen />,
        title: "Geprüfter IT-Spezialist Softwareentwicklung & Bachelor Professional in Computer Science",
        subtitle: "IHK Münster",
        date: "aktuell, berufsbegleitend",
        text: "Kontinuierliche Weiterbildung neben meiner beruflichen Tätigkeit."
    }
];

const About: React.FC = () => {

    return (
        <div className="about--main">
            <ContentHeader>
                Über mich
            </ContentHeader>
            <div className="about--content">
                <div className="about--hero">
                    <div className="about--portrait">
                        <img src={portrait} className="about-portrait--image" />
                    </div>
                    <div className="about--intro">
                        <h2 className="about-intro--name">Louis Venhoff</h2>
                        <p className="about-intro--tagline">
                            Vom Elektroniker zum Softwareentwickler – mit Leidenschaft für moderne Webtechnologien.
                        </p>
                        <div className="about--tech-icons">
                            {techIcons.map(tech => (
                                <div className="about-tech-icon--item" key={tech.name}>
                                    {tech.icon}
                                    <span className="about-tech-icon--label">{tech.name}</span>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
                <div className="about--sections">
                    <div className="about--section">
                        <h3 className="about-section--title">Mein Werdegang</h3>
                        <Timeline items={timelineItems} />
                    </div>
                    <div className="about--section">
                        <h3 className="about-section--title">Motivation</h3>
                        <p className="about-section--text">
                            Mich treibt der Anspruch an, qualitativ hochwertige Software zu entwickeln und dabei stets dazuzulernen. Ich
                            freue mich über jede Gelegenheit, mein Wissen zu erweitern und an spannenden Projekten mitzuwirken.
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default About;
