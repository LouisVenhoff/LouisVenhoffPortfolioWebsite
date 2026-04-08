import useEnv from "./useEnv";

export type Contribution = {
    time: Date;
    count: number;
}


export default function useContribution(){

    const loadContributionList = async ():Promise<Contribution[]> => {
        
        const {serverUrl} = useEnv();

        const result = await fetch(`${serverUrl}/api/Contributions`);
        
        const contributions:Contribution[] = await result.json();

        return contributions;
    }


    return { loadContributionList }


}