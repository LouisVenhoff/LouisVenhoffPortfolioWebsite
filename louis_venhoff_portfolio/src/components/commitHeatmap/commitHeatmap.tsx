import React, { useEffect, useState } from "react";
import "../../styles/components/commitHeatmap.css";
import CalendarHeatmap from "react-calendar-heatmap";
import { Card, ProgressCircle } from "@chakra-ui/react";
import { Button } from "@chakra-ui/react";
import useContribution, { Contribution } from "../../hooks/useContribution";

type GuiCalendarDay = {
  date: string,
  count: number
}

const CommitHeatmap: React.FC = () => {

  const { loadContributionList } = useContribution();

  const [contributions, setContributions] = useState<Contribution[]>([]);
  const [firstCalendarDate, setFirstCalendarDate] = useState<Date>(new Date());
  const [lastCalendarDate, setLastCalendarDate] = useState<Date>(new Date());

  useEffect(() => {
    if(contributions.length > 0){
      setFirstCalendarDate(findFirstCalendarDate());
      setLastCalendarDate(findLastCalendarDate());
    }
  }, [contributions]);

  useEffect(() => {
    fetchContributions();
  }, []);

  const fetchContributions = async () => {
    
    const contributions:Contribution[] =  await loadContributionList();

    setContributions(contributions);
  }

  const findFirstCalendarDate = ():Date => {
    if(contributions.length === 0){
      throw "Error: Trying to load calendar without data!"
    }

    return contributions[0].time;
  }

  const findLastCalendarDate = ():Date => {
    if(contributions.length === 0){
      throw "Error: Trying to load calendar without data!";
    }

    return contributions[contributions.length -1].time;
  }

  const redirectToGithub = () => {
    window.open("https://github.com/LouisVenhoff");
  };

  const convertToGuiCalendarDays = ():GuiCalendarDay[] => {

    return contributions.map((c: Contribution) => { return {date: c.time.toString(), count: c.count}});

  }

  const renderHeatmap = () => {
    return false ? (
      <ProgressCircle.Root value={null} size="sm">
        <ProgressCircle.Circle>
          <ProgressCircle.Track />
          <ProgressCircle.Range />
        </ProgressCircle.Circle>
      </ProgressCircle.Root>
    ) : (
      <CalendarHeatmap
        startDate={firstCalendarDate}
        endDate={lastCalendarDate}
        values={convertToGuiCalendarDays()}
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
