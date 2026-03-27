using System.Text.Json.Serialization;

namespace portfolio_backend.Exceptions;

[Serializable]
class GithubGraphQLException : Exception{

    public GithubGraphQLException(string message) : base(message){}

}