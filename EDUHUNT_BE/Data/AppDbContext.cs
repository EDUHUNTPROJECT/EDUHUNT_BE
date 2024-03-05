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
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<CodeVerify> CodeVerifies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CodeVerify>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.UserId).IsRequired(false);
                entity.Property(e => e.Code).IsRequired();
                entity.Property(e => e.ExpirationTime).IsRequired();
                entity.HasOne<ApplicationUser>()
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.ContentURL).IsRequired();
                entity.Property(e=>e.IsApproved).IsRequired();
                entity.HasOne<ApplicationUser>()
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StudentID).IsRequired();
                entity.Property(e => e.ScholarshipID).IsRequired();
                entity.Property(e => e.StudentCV).IsRequired(false); // Adjust based on your requirements
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.MeetingURL).IsRequired(false); // Adjust based on your requirements

                // Assuming you want to store the dates as UTC
                entity.Property(e => e.StudentAvailableStartDate).IsRequired();
                entity.Property(e => e.StudentAvailableEndDate).IsRequired();
                entity.Property(e => e.ScholarshipProviderAvailableStartDate).IsRequired();
                entity.Property(e => e.ScholarshipProviderAvailableEndDate).IsRequired();

                // Optional: Add relationships if there are any
                // For example, to link to Student and ScholarshipProvider entities
            });
            modelBuilder.Entity<RoadMap>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.ContentURL).IsRequired(false);
                entity.Property(e => e.IsApproved).IsRequired();
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
                entity.Property(e => e.IsInSite);
                entity.Property(e => e.Description).HasColumnType("text").IsRequired(false);
                entity.Property(e => e.Url).HasMaxLength(255).IsRequired(false);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.ImageUrl).IsRequired(false);
                entity.Property(e => e.IsApproved).IsRequired();
                entity.HasOne<ApplicationUser>()
                      .WithMany()
                      .HasForeignKey(e => e.AuthorId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);

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
                entity.Property(e=>e.IsAllow).IsRequired();
                entity.Property(e => e.IsVIP);
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
