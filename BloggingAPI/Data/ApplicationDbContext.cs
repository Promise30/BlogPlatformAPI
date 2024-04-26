using BloggingAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace BloggingAPI.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Post> BlogPosts { get; set; }
    public DbSet<Comment> PostComments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>()
                   .HasOne(c => c.Post)
                   .WithMany(c => c.Comment)
                   .HasForeignKey(c => c.PostId);
        modelBuilder.Entity<Post>()
                    .HasMany(p => p.Comment)
                    .WithOne(p => p.Post)
                    .HasForeignKey(p => p.PostId)
                    .IsRequired();
    }
}

