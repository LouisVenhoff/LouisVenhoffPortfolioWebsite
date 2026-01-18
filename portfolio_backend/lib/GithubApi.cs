using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Expressions;
using Newtonsoft.Json.Linq;
using NuGet.Packaging;
using portfolio_backend.Exceptions;
using portfolio_backend.Models;
using System.Net.Http.Headers;
using portfolio_backend.Interfaces;
using portfolio_backend.Services;

namespace portfolio_backend.Services{

    public class GithubApi {

        private ISecretProvider secretProvider;

        public GithubApi(SecretProvider secretProvider)
        {
            this.secretProvider = secretProvider;
        }

        private HttpClient client = new HttpClient();

        public async Task<List<Repository>> FetchRepositorys(){
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ReadAccessToken());
            client.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse("LouisVenhoff"));
            HttpResponseMessage response = await client.GetAsync("https://api.github.com/user/repos?perPage=100&page=1");
            Console.WriteLine(response);
            List<Repository> repositories = [];

            String rawJson = await response.Content.ReadAsStringAsync();

            foreach(JObject obj in JArray.Parse(rawJson)){
                if(obj["name"] == null) continue;

                repositories.Add(new Repository(obj["name"]!.ToString(), obj["clone_url"]!.ToString()));
            };

            return repositories;
        }

        private String ReadAccessToken(){
            string? accessToken = this.secretProvider.GetGithubPat() ?? throw new GithubAuthException("GITHUB_ACCESS_TOKEN not found!");
            return accessToken;
        }


    }

}