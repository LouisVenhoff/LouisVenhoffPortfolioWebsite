using System.Net.Http.Headers;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Globalization;
using portfolio_backend.Interfaces;
using portfolio_backend.Dto;
using portfolio_backend.Exceptions;
using portfolio_backend.Models;
using portfolio_backend.Data;


namespace portfolio_backend.Services
{

    class ContributionsService : BackgroundService
    {


        private GraphQLHttpClient client;

        private IServiceScopeFactory scopeFactory;

        public ContributionsService(IServiceScopeFactory scopeFactory, SecretProvider secretProvider)
        {

            this.client = new GraphQLHttpClient("https://api.github.com/graphql", new NewtonsoftJsonSerializer());
            this.client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretProvider.GetGithubPat());
            this.scopeFactory = scopeFactory;

        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Fetching Data");
            this.FetchData();
        }

        private async Task FetchData()
        {
            try
            {
                var request = new GraphQLRequest
                {
                    Query = @"{
                        user(login: ""LouisVenhoff"") {
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
                    }"
                };

                var response = await this.client.SendQueryAsync<UserResponse>(request);

                // Console.WriteLine(response);
                Console.WriteLine(response.Data.user.contributionsCollection.contributionCalendar.weeks[1].contributionDays[0].contributionCount);

                // string rawJson = Newtonsoft.Json.JsonConvert.SerializeObject(response.Data, Newtonsoft.Json.Formatting.Indented);
                //Console.WriteLine(rawJson);

                this.updateDatabase(response.Data.user.contributionsCollection.contributionCalendar.weeks);
            }
            catch (Exception ex)
            {
                throw new GithubGraphQLException(ex.Message);
            }
        }

        public async void updateDatabase(List<Week> data)
        {
            List<Contribution> contributionsSummary = [];

            foreach (Week week in data)
            {
                foreach (ContributionDay day in week.contributionDays)
                {
                    Contribution test = new Contribution(day.date, day.contributionCount);
                    this.persistContribution(test);
                }
            }
        }

        private async void persistContribution(Contribution con)
        {
            DateTime time = con.time;
            int count = con.Count;

            using var scope = scopeFactory.CreateScope();
            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            List<Contribution> foundContributions = await dbContext.Contributions.Where(c => c.time == time).ToListAsync();

            if (foundContributions.Count == 0)
            {
                dbContext.Contributions.Add(con);
            }

            await dbContext.SaveChangesAsync();
            
        }
    }


}