using Microsoft.EntityFrameworkCore;
using ONEE_Stage.Models.Entities;

namespace ONEE_Stage.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<AIScore> AIScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure multi-document relationships on Application
            modelBuilder.Entity<Application>()
                .HasOne(a => a.CvDocument)
                .WithMany()
                .HasForeignKey("CvDocumentId")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.CoverLetterDocument)
                .WithMany()
                .HasForeignKey("CoverLetterDocumentId")
                .OnDelete(DeleteBehavior.Restrict);

            // Fix DocumentComment relationship and prevent cyclical cascade paths
            modelBuilder.Entity<DocumentComment>(entity =>
            {
                entity.HasOne(dc => dc.AuthorUser)
                      .WithMany()
                      .HasForeignKey(dc => dc.AuthorUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}