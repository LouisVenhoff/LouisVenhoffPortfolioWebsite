using Microsoft.EntityFrameworkCore;
using portfolio_backend.Models;

namespace portfolio_backend.Data{

    public class ApplicationDbContext : DbContext{

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options){

        }

        public DbSet<Repository> Repositorys {get; set;}

        public DbSet<Doc> Docs {get; set;}

    }

}