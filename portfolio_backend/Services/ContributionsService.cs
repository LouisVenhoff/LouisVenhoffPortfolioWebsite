using System.Net.Http.Headers;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json.Linq;
using portfolio_backend.Interfaces;
using portfolio_backend.Dto;


namespace portfolio_backend.Services
{

    class ContributionsService : BackgroundService
    {


        private GraphQLHttpClient client;

        public ContributionsService(SecretProvider secretProvider)
        {

            this.client = new GraphQLHttpClient("https://api.github.com/graphql", new NewtonsoftJsonSerializer());
            this.client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretProvider.GetGithubPat());
            
            
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
                var request = new GraphQLRequest{
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

                Console.WriteLine(response);
                Console.WriteLine(response.Data.user.contributionsCollection.contributionsCalendar);

                // string rawJson = Newtonsoft.Json.JsonConvert.SerializeObject(response.Data, Newtonsoft.Json.Formatting.Indented);
                //Console.WriteLine(rawJson);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            
        }
    }


}