namespace portfolio_backend.Dto;

public class ContributionDay
{
    public string date { get; set; }
    public int contributionCount { get; set; }
}

public class Week
{
    public List<ContributionDay> contributionDays { get; set; }
}

public class ContributionCalendar
{
    public List<Week> weeks { get; set; }
}

public class ContributionsCollection
{
    public ContributionCalendar contributionCalendar { get; set; }
}

public class User
{
    public ContributionsCollection contributionsCollection { get; set; }
}

public class UserResponse
{
    public User user { get; set; }
}
