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

        public async void UpdateDatabase(List<Week> data)
        {
            List<Contribution> contributionsSummary = [];

            foreach (Week week in data)
            {
                foreach (ContributionDay day in week.contributionDays)
                {
                    this.PersistContribution(new Contribution(day.date, day.contributionCount));
                }
            }
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
                this.UpdateDatabase(response.Data.user.contributionsCollection.contributionCalendar.weeks);
            }
            catch (Exception ex)
            {
                throw new GithubGraphQLException(ex.Message);
            }
        }

        private async void PersistContribution(Contribution con)
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
            else if (foundContributions[0].Count != count)
            {
                foundContributions[0].Count = count;
            }
            else
            {
                return;
            }

            await dbContext.SaveChangesAsync();
            
        }
    }


}