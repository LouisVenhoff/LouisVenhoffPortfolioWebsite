import { JSX, useEffect, useRef, useState } from "react";
import "../../styles/components/timeline.css";

export type TimelineItem = {
    icon: JSX.Element;
    title: string;
    subtitle: string;
    date?: string;
    text: string;
};

type TimelineProps = {
    items: TimelineItem[];
};

type TimelineEntryProps = {
    item: TimelineItem;
    showLine: boolean;
    delayMs: number;
};

const TimelineEntry: React.FC<TimelineEntryProps> = ({ item, showLine, delayMs }) => {

    const itemRef = useRef<HTMLDivElement>(null);
    const [visible, setVisible] = useState<boolean>(false);

    useEffect(() => {
        const node = itemRef.current;
        if (!node) return;

        const observer = new IntersectionObserver(([entry]) => {
            setVisible(entry.isIntersecting);
        }, { threshold: 0.3 });

        observer.observe(node);

        return () => observer.disconnect();
    }, []);

    return (
        <div
            className={`timeline--item${visible ? " timeline-item--visible" : ""}`}
            style={{ transitionDelay: `${delayMs}ms` }}
            ref={itemRef}
        >
            <div className="timeline-item--marker">
                <div className="timeline-item--icon">
                    {item.icon}
                </div>
                {showLine && <div className="timeline-item--line" />}
            </div>
            <div className="timeline-item--content">
                <h4 className="timeline-item--title">{item.title}</h4>
                <div className="timeline-item--subtitle">
                    {item.subtitle}{item.date ? ` · ${item.date}` : ""}
                </div>
                <p className="timeline-item--text">{item.text}</p>
            </div>
        </div>
    );
}

const Timeline: React.FC<TimelineProps> = ({ items }) => {

    const buildEntries = ():JSX.Element[] => {
        return items.map((item, index) => (
            <TimelineEntry
                item={item}
                showLine={index < items.length - 1}
                delayMs={index * 60}
                key={item.title}
            />
        ));
    }

    return (
        <div className="timeline--main">
            {buildEntries()}
        </div>
    );
}

export default Timeline;
