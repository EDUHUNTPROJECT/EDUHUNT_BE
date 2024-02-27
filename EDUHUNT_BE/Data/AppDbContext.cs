using EDUHUNT_BE.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EDUHUNT_BE.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ScholarshipInfo> ScholarshipInfos { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<QA> QAs { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<CV> CVs { get; set; }
        public DbSet<RoadMap> RoadMaps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoadMap>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.ContentURL).IsRequired(false);

                entity.HasOne<ApplicationUser>()
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ScholarshipInfo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Budget).HasMaxLength(255).IsRequired(false);
                entity.Property(e => e.Title).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Location).HasMaxLength(255).IsRequired(false);
                entity.Property(e => e.SchoolName).HasMaxLength(255).IsRequired(false);
                entity.Property(e => e.CategoryId);
                entity.Property(e => e.AuthorId);
                entity.Property(e => e.IsInSite);
                entity.Property(e => e.Url).HasMaxLength(255).IsRequired(false);
                entity.Property(e => e.CreatedAt).IsRequired();

                // Additional configurations can be added here based on your requirements
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Sender).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Receiver).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.SentAt).IsRequired();

                // Additional configurations for Message entity can be added here if needed
            });

            modelBuilder.Entity<QA>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AskerId).IsRequired();
                entity.Property(e => e.AnswerId).IsRequired();
                entity.Property(e => e.Question).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Answer).HasMaxLength(255).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();

                // Additional configurations for QA entity can be added here if needed
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(e => e.Id); // Assuming Id is the primary key
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.ContentURL).IsRequired(false); 
                entity.Property(e => e.FirstName).IsRequired(false); 
                entity.Property(e => e.LastName).IsRequired(false); 
                entity.Property(e => e.UserName).IsRequired(false); 
                entity.Property(e => e.ContactNumber).IsRequired(false); 
                entity.Property(e => e.Address).IsRequired(false);
                entity.Property(e => e.Description).IsRequired(false);
                entity.Property(e=> e.UrlAvatar).IsRequired(false);
                // Additional configurations for the Profile entity can be added here if needed
            });

            modelBuilder.Entity<CV>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.UrlCV).IsRequired();
                // Additional configurations for the Profile entity can be added here if needed
            });

            // Additional configurations for other entities can be added here if needed
        }
    }
}
