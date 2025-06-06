using System.Runtime.CompilerServices;
using System.Text.Json;
using LibGit2Sharp;

namespace portfolio_backend.Models{

    public class Doc{

        public Doc(){}

        public Doc(string DocumentName, string MarkdownPath, int RepositoryId)
        {
            this.DocumentName = DocumentName;
            this.MarkdownPath = MarkdownPath;
            this.RepositoryId = RepositoryId;
            this.ThumbnailPath = null;
        }

        public Doc(string DocumentName, string MarkdownPath, string ThumbnailPath, int RepositoryId)
        {
            this.DocumentName = DocumentName;
            this.MarkdownPath = MarkdownPath;
            this.ThumbnailPath = ThumbnailPath;
            this.RepositoryId = RepositoryId;

        }

        public void AddTag(string tag)
        {
            List<string> temp = JsonSerializer.Deserialize<List<string>>(this.Tags)! ?? throw new Exception("Document tags corrupt!");

            temp.Add(tag);

            this.Tags = JsonSerializer.Serialize(temp);
        }

        [Key]
        public int Id {get; set;}

        [Required]
        public string MarkdownPath {get; set;}
        
        public string? ThumbnailPath { get; set; }

        public string DocumentName { get; set; }

        public string? Description { get; set; }

        public string Tags { get; set; } = JsonSerializer.Serialize(new List<string>());

        public int? RepositoryId { get; set; }

        public Repository? Repository {get; set;} = null!;

    }


}