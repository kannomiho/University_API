using Microsoft.EntityFrameworkCore;
using University_API.Models;

namespace University_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<University> Universities { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<University>().HasData(
                new University()
                {
                    Id = 1,
                    Name = "Big U",
                    Country = "SG",
                    Webpages = "https://bigu.edu.sg",
                    IsBookmark = false,
                    Created=DateTime.Now

                },
                 new University()
                 {
                            Id = 2,
                            Name = "Super U",
                            Country = "SG",
                            Webpages = "https://superu.edu.sg",
                            IsBookmark = false,
                            Created = DateTime.Now

                 }
                );
        }

    }
}
