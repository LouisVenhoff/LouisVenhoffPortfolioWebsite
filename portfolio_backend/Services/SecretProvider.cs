using System;
using System.IO;
using portfolio_backend.Interfaces;
using portfolio_backend.Exceptions;


namespace portfolio_backend.Services
{

    class SecretProvider : ISecretProvider
    {

        private string githubPat = null;

        public string GetGithubPat()
        {
            if (githubPat != null) return githubPat;

            string secretPath = "/run/secrets/github_pat";

            if (File.Exists(secretPath))
            {
                githubPat = File.ReadAllText(secretPath);

                return githubPat;
            }
            else
            {
                throw new GithubAuthException("No secret for github personal acces token (github_pat) found!");
                return null;
            }
        }
        


    }

}