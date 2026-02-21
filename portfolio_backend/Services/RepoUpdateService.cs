using portfolio_backend.Models;
using portfolio_backend.Data;
using portfolio_backend.Lib;
using portfolio_backend.Classes;
using portfolio_backend.Interfaces;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Threading.Tasks;
using GitRepo = LibGit2Sharp.Repository;
using Git = LibGit2Sharp;
using portfolio_backend.Exceptions;
using Microsoft.OpenApi.Writers;
using System.Reflection.Metadata;
using System.Text.Json;


namespace portfolio_backend.Services{

    public class RepoUpdateService
    {

        private IServiceScopeFactory _scopeFactory;

        private ISecretProvider _secretProvider;

        private GithubApi _githubApi;

        private LibGit2SharpWrapper gitWrapper;

        public RepoUpdateService(IServiceScopeFactory scopeFactory, SecretProvider secretProvider, GithubApi githubApi)
        {
            this._scopeFactory = scopeFactory;

            this._secretProvider = secretProvider;

            this._githubApi = githubApi;

            string? githubToken = this._secretProvider.GetGithubPat();
            this.gitWrapper = new LibGit2SharpWrapper(githubToken);
        }

        public void StartUpdate()
        {
            Task.Run(async () =>
            {

                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var items = await dbContext.Repositorys.ToListAsync();

                List<Repository> repoList = await this._githubApi.FetchRepositorys();
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
            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

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
                            ProcessAssets(document, repo);

                            dbContext.Docs.Update(document);
                            await dbContext.SaveChangesAsync();
                            break;
                        }
                    }

                    if (!documentFound)
                    {
                        string markdownPath = $"/var/portfolio/{repo.Id}/.portfolio/portfolio.md";

                        if (!File.Exists(markdownPath)) continue;

                        Doc newDoc = new Doc("DocumentName", markdownPath, repo.Id);

                        ProcessAssets(newDoc, repo);

                        dbContext.Docs.Add(newDoc);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }

        private static void ProcessAssets(Doc doc, Repository repo)
        {
            ProcessThumbnail(doc, repo);
            ProcessConfig(doc, repo);
        }


        private static void ProcessThumbnail(Doc doc, Repository repo)
        {
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };

            var files = Directory.GetFiles($"/var/portfolio/{repo.Id}/.portfolio/", "thumbnail.*");

            string? path = files.FirstOrDefault(f => allowedImageExtensions.Contains(Path.GetExtension(f).ToLower()));

            if (path == null) return;

            doc.ThumbnailPath = path;
        }

        private static void ProcessConfig(Doc doc, Repository repo)
        {
            var file = Directory.GetFiles($"/var/portfolio/{repo.Id}/.portfolio/", "config.json");

            if (file.Length == 0) return;

            string config;

            using StreamReader sr = new StreamReader(file[0]);
            config = sr.ReadToEnd();

            JsonConfig configObject = JsonSerializer.Deserialize<JsonConfig>(config);
            string[] persistedTags = JsonSerializer.Deserialize<string[]>(doc.Tags);

            if (configObject == null) return;

            doc.DocumentName = configObject.DocumentName;
            doc.Description = configObject.Description;

            //Console.WriteLine(persistedTags);

            // foreach (string tag in configObject.Tags)
            // {

            //     if(Array.Find(persistedTags, (string currentTag) => currentTag == tag)){
            //         continue;
            //     };

            //     Console.WriteLine(doc.Tags);


            //     doc.AddTag(tag);
            // }
        }

    }


}