import React, { JSX } from "react";
import "../../styles/components/contentHeader.css";
import { IconButton } from "@chakra-ui/react";
import { HiChevronLeft } from "react-icons/hi";

type ContentHeaderProps = {
    children: React.ReactNode;
    showBackButton?: boolean
}


const ContentHeader:React.FC<ContentHeaderProps> = ({children, showBackButton}: ContentHeaderProps) => {
    
    const redirectToProjectPage = () => {
        window.location.href = "/projects"
    }

    const renderBackButton = ():JSX.Element | null => {
        
        const backButton: JSX.Element = <IconButton size="xl" backgroundColor="#242424" onClick={redirectToProjectPage}><HiChevronLeft /></IconButton>
        
        return showBackButton ?  backButton : null;
    }
    
    return(
        <div className="content-header--main">
            <div className="flex items-start">
                {renderBackButton()}
                {children}
            </div>
            
        </div>
    );
}

export default ContentHeader;