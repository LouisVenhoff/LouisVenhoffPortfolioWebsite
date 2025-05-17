import React from "react";
import "../../styles/components/markdownElement.css";
import testMarkdown from "../../assets/test.md?raw";
import MarkdownPreview  from "@uiw/react-markdown-preview";

const MarkdownElement:React.FC = () => {
    return(
        <div className="markdownElement">
            <MarkdownPreview source={testMarkdown}/>
        </div>
    );
}

export default MarkdownElement;