using ForestAppUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ForestAppUI.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<ArticleComment> ArticleComments { get; set; }
        public DbSet<Category> Categories { get; set; }
       public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");

            builder.Entity<ArticleComment>()
                       .HasOne(ac => ac.User)
                       .WithMany()
                       .HasForeignKey(ac => ac.UserId)
                       .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
