namespace portfolio_backend.Dto;

public class UserResponse
{
    public User user { get; set; }
}

public class User
{
    public ContributionsCollection contributionsCollection {get; set;}
}

public class ContributionsCollection
{
    public ContributionsCalendar contributionsCalendar {get; set;}
}

public class ContributionsCalendar
{
    public List<Week> weeks {get; set;}
}

public class Week
{
    public List<ContributionDay> contributionsDays {get; set;}
}

public class ContributionDay
{
    public DateTime date {get; set;}
    public int contributionCount {get; set;}
}

