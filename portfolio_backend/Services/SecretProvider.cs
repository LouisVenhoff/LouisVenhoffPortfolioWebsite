using System;
using System.IO;
using portfolio_backend.Interfaces;
using portfolio_backend.Exceptions;
using System.Text.RegularExpressions;
//using Microsoft.Extensions.Hosting;


namespace portfolio_backend.Services
{

    public class SecretProvider : ISecretProvider
    {

        private string githubPat = null;

        private readonly IHostEnvironment env;

        public SecretProvider(IHostEnvironment env)
        {
            this.env = env;
        }

        public string GetGithubPat()
        {
            if (githubPat != null) return githubPat;

            if (env.IsDevelopment())
            {
                githubPat = Environment.GetEnvironmentVariable("GITHUB_ACCESS_TOKEN") ?? throw new GithubAuthException("GITHUB_ACCESS_TOKEN not found!");
                return githubPat;
            }

            githubPat = LoadGithubTokenFromSecrets();

            return githubPat;

        }

        private string LoadGithubTokenFromSecrets()
        {
            string secretPath = "/var/run/secrets/github_pat";
            try
            {
                if (File.Exists(secretPath))
                {

                    string pat = File.ReadAllText(secretPath);
                    return Regex.Replace(pat, @"\s+", "");;
                }
                else
                {
                    throw new GithubAuthException("No secret for github personal acces token (github_pat) found!");
                    return null;
                }
            }
            catch
            {
                Console.WriteLine("Error while reading Secrets");
                throw new GithubAuthException("Error while opening secrets file");
            }
            
        }
        


    }

}