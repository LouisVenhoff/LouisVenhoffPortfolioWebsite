import React from "react";
import "../../styles/components/markdownElement.css";
import MarkdownPreview  from "@uiw/react-markdown-preview";


type MarkdownElementProps = {
    markdown: string
}


const MarkdownElement:React.FC<MarkdownElementProps> = ({markdown}) => {
    return(
        <div className="markdownElement">
            <MarkdownPreview source={markdown}/>
        </div>
    );
}

export default MarkdownElement;