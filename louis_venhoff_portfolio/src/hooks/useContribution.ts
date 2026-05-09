import { useState } from "react";
import useEnv from "./useEnv";

export type Contribution = {
    time: Date;
    count: number;
}


export default function useContribution(){

    const [loading, setLoading] = useState<boolean>(true);
    
    const loadContributionList = async ():Promise<Contribution[]> => {
        
        setLoading(true);

        const {serverUrl} = useEnv();

        const result = await fetch(`${serverUrl}/api/Contributions`);
        
        const contributions:Contribution[] = await result.json();

        setLoading(false);

        return contributions;
    }


    return { loadContributionList, loading }


}