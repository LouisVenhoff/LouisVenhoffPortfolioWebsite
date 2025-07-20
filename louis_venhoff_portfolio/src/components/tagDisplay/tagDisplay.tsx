import React, { JSX, useEffect, useRef, useState} from "react";
import * as motion from "motion/react-client";
import { useAnimation } from "motion/react";
import { Badge } from "@chakra-ui/react";
import Doc from "../../classes/doc";
import "../../styles/components/tagDisplayStyle.css";

type TagDisplayProps = {
    currentDoc: Doc;
}


const TagDisplay:React.FC<TagDisplayProps> = ({currentDoc}) => {

    
    const [collapsed, setCollapsed] = useState<boolean>(true);
    
    const tagsDiv:Ref<HTMLElement> = useRef<HTMLElement>(null);

    useEffect(() => {
        tagsDiv.current.addEventListener("mouseenter", () => {
            decollapse();
        });

        tagsDiv.current.addEventListener("mouseleave", () => {
            collapse();
        });
    }, []);

    
    const renderTagsCollapsedState = ():JSX.Element[] => {
        if(!currentDoc) return [];

        let badges:JSX.Element[] = [];

        const visibleTagCount = currentDoc.tags.length < 5 ? currentDoc.tags.length : 5;
        
        for(let i = 0; i < visibleTagCount; i++){
            badges.push(<Badge key={i} maxW="sm" backgroundColor="#000000" color="teal">{currentDoc.tags[i]}</Badge>);
        }

        let hiddenTagCount: number = currentDoc.tags.length - visibleTagCount;

        if(hiddenTagCount > 0){
            badges.push(<p className="tag-display--hidden-tag-count">{`+${hiddenTagCount}`}</p>);
        }

        return badges;

    }

    const collapse = () => {
        setCollapsed(true);
    }

    const decollapse = () => {
        setCollapsed(false);
    }

    const renderAllTags = ():JSX.Element => {
        if(!currentDoc) return <></>;

        let badges:JSX.Element[] = [];
        let secondRowBadges:JSX.Element[] = [];

        const alwaysVisibleTagCount = currentDoc.tags.length < 5 ? currentDoc.tags.length : 5;

        for(let i = 0; i < currentDoc.tags.length; i++){
            
            if(i <= alwaysVisibleTagCount){
                badges.push(<Badge key={i} maxW="sm" backgroundColor="#000000" color="teal">{currentDoc.tags[i]}</Badge>);
            }
            else{
                secondRowBadges.push(<Badge key={i} maxW="sm" backgroundColor="#000000" color="teal">{currentDoc.tags[i]}</Badge>);
            }
        }
        
        return(<motion.div animate={{ height: 50, animationDuration: 100 }} className="tag-display--full-container">
                    <div className="tag-display"> 
                        {badges}
                    </div>
                    <div className="tag-display">
                        {secondRowBadges}
                    </div>
                </motion.div>);
    }

    return(
        <div className="flex flex-row">
            <div ref={tagsDiv}>
                <div className="tag-display">
                    {collapsed ? renderTagsCollapsedState() : renderAllTags()}
                </div>
            </div>
        </div>
    );

}

export default TagDisplay;