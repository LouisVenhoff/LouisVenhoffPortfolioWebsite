using portfolio_backend.Models;
using portfolio_backend.Data;
using portfolio_backend.Lib;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Threading.Tasks;
using GitRepo = LibGit2Sharp.Repository;
using Git = LibGit2Sharp;
using portfolio_backend.Exceptions;
using Microsoft.OpenApi.Writers;
using System.Reflection.Metadata;

namespace portfolio_backend.Services{

    public class RepoUpdateService
    {

        private IServiceScopeFactory _scopeFactory;

        private LibGit2SharpWrapper gitWrapper;

        public RepoUpdateService(IServiceScopeFactory scopeFactory)
        {
            this._scopeFactory = scopeFactory;

            string? githubToken = Environment.GetEnvironmentVariable("GITHUB_ACCESS_TOKEN") ?? throw new GithubAuthException("GITHUB_ACCESS_TOKEN not found!");
            this.gitWrapper = new LibGit2SharpWrapper(githubToken);
        }

        public void StartUpdate()
        {
            Task.Run(async () =>
            {

                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var items = await dbContext.Repositorys.ToListAsync();

                List<Repository> repoList = await GithubApi.FetchRepositorys();
                try
                {
                    foreach (Repository repo in repoList)
                    {
                        Repository? foundRepo = dbContext.Repositorys.SingleOrDefault(r => r.Name == repo.Name);

                        if (foundRepo == null)
                        {
                            dbContext.Add(repo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                await dbContext.SaveChangesAsync();

                await PullRepositorys();
            });
        }

        private async Task PullRepositorys()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var items = await dbContext.Repositorys.ToListAsync();


            try
            {
                if (Directory.Exists("/var/portfolio") == false)
                {
                    Console.WriteLine("Creating folder for repositorys");
                    DirectoryInfo dirInfo = Directory.CreateDirectory("/var/portfolio");
                }

                foreach (Repository repo in items)
                {
                    if (Directory.Exists($"/var/portfolio/{repo.Id}"))
                    {
                        Console.WriteLine($"Pulling: {repo.Name}");
                        this.gitWrapper.Pull($"/var/portfolio/{repo.Id}");
                    }
                    else
                    {
                        if (repo.Name == "aircraft") continue;

                        Console.WriteLine($"Cloning: {repo.Name}");
                        this.gitWrapper.Clone(repo.CloneLink, $"/var/portfolio/{repo.Id}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"There was an Error! {e.Message}");
            }
            finally
            {
                this.SeachForDocs(items);
            }


        }

        private async void SeachForDocs(List<Repository> repositories)
        {

            using var scope = _scopeFactory.CreateScope();
            ApplicationDbContext dbContext  = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); 

            List<Doc> documents = await dbContext.Docs.ToListAsync();

            foreach (Repository repo in repositories)
            {
                if (Directory.Exists($"/var/portfolio/{repo.Id}/.portfolio"))
                {

                    bool documentFound = false;

                    foreach (Doc document in documents)
                    {
                        if (document.RepositoryId == repo.Id)
                        {
                            documentFound = true;
                            break;
                        }
                    }

                    if (!documentFound)
                    {
                        string markdownPath = $"/var/portfolio/{repo.Id}/.portfolio/portfolio.md";

                        if (!File.Exists(markdownPath)) continue;

                        dbContext.Docs.Add(new Doc(markdownPath, repo.Id));
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }

    }


}