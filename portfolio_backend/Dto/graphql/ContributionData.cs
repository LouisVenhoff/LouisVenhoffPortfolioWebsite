namespace portfolio_backend.Dto;

public class UserResponse
{
    public User user { get; set; }
}

public class User
{
    public ContributionsCollection contributionsCollection;
}

public class ContributionsCollection
{
    public ContributionsCalendar contributionsCalendar;
}

public class ContributionsCalendar
{
    public List<Week> weeks;
}

public class Week
{
    public List<ContributionDay> contributionsDays;
}

public class ContributionDay
{
    public DateTime date;
    public int contributionCount;
}

