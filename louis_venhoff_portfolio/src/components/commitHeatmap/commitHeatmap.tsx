import React, { useEffect, useState } from "react";
import "../../styles/components/commitHeatmap.css";
import CalendarHeatmap from "react-calendar-heatmap";
import { Card, ProgressCircle } from "@chakra-ui/react";
import { Button } from "@chakra-ui/react";
import { useQuery, gql } from "@apollo/client";
import { ContributionCalendarWeek, ContributionCalendarDay } from "../../gql/graphql";

type GuiCalendarDay = {
  date: string,
  count: number
}


const CONTRIBUTIONS_QUERY = gql`
  {
    user(login: "LouisVenhoff") {
      contributionsCollection {
        contributionCalendar {
          weeks {
            contributionDays {
              date
              contributionCount
            }
          }
        }
      }
    }
  }
`;

const CommitHeatmap: React.FC = () => {
  const { data, loading, error } = useQuery(CONTRIBUTIONS_QUERY);

  const [contributions, setContributions] = useState<GuiCalendarDay[]>([]);
  const [firstCalendarDate, setFirstCalendarDate] = useState<string>("0");
  const [lastCalendarDate, setLastCalendarDate] = useState<string>("0");

  useEffect(() => {
    if(!loading && !error){
      setContributions(generateContributionData(data.user.contributionsCollection.contributionCalendar.weeks));
    }
  },[loading, data, error]);

  useEffect(() => {
    if(contributions.length > 0){
      setFirstCalendarDate(findFirstCalendarDate());
      setLastCalendarDate(findLastCalendarDate());
    }
  }, [contributions]);

  const generateContributionData = (weeks:ContributionCalendarWeek[]):GuiCalendarDay[] => {
    
    let guiContributionDays:GuiCalendarDay[] = [];
    
    for(let i = 0; i < weeks.length; i++){
      
      let contributionDays:ContributionCalendarDay[] = weeks[i].contributionDays;
      
      for(let j = 0; j < contributionDays.length; j++){
        guiContributionDays.push({date: contributionDays[j].date, count: contributionDays[j].contributionCount });
      }
    }

    return guiContributionDays;
  }

  const findFirstCalendarDate = ():string => {
    if(contributions.length === 0){
      throw "Error: Trying to load calendar without data!"
    }

    return contributions[0].date;
  }

  const findLastCalendarDate = ():string => {
    if(contributions.length === 0){
      throw "Error: Trying to load calendar without data!";
    }

    return contributions[contributions.length -1].date;
  }

  const redirectToGithub = () => {
    window.open("https://github.com/LouisVenhoff");
  };

  const renderHeatmap = () => {
    return loading ? (
      <ProgressCircle.Root value={null} size="sm">
        <ProgressCircle.Circle>
          <ProgressCircle.Track />
          <ProgressCircle.Range />
        </ProgressCircle.Circle>
      </ProgressCircle.Root>
    ) : (
      <CalendarHeatmap
        startDate={new Date(firstCalendarDate)}
        endDate={new Date(lastCalendarDate)}
        values={contributions}
        classForValue={(value) => {
          if (!value) {
            return "color-empty";
          }
          return `color-scale-${value.count}`;
        }}
      />
    );
  };

  return (
    <div className="commit-heatmap--container">
      <Card.Root height={230} variant={"elevated"} color={"teal"} backgroundColor={"#202020"} borderColor={"#202020"} boxShadow={"sm"} boxShadowColor={"teal"} >
        <Card.Header fontSize="xl">
          <h2>Github activity: </h2>
        </Card.Header>
        <Card.Body><div className="flex justify-center">{renderHeatmap()}</div></Card.Body>
        <Card.Footer>
          <Button
            onClick={redirectToGithub}
            colorScheme="teal"
            variant={"solid"}
            size="md"
            backgroundColor="teal"
          >
            Zu Github
          </Button>
        </Card.Footer>
      </Card.Root>
    </div>
  );
};

export default CommitHeatmap;
