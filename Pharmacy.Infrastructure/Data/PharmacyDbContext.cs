using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.ClaimSubmit;
using Pharmacy.Domain.Entities.Finance;
using Pharmacy.Domain.Entities.Members;
using Pharmacy.Domain.Entities.NDCMaintenance;
using Pharmacy.Domain.Entities.Operations;
using Pharmacy.Domain.Entities.PriorAuthorization;
using Pharmacy.Domain.Entities.Providers;
using Pharmacy.Domain.Entities.RxAdmin;
using Pharmacy.Domain.Entities.RxClaims;
using Pharmacy.Domain.Entities.RxDUR;

namespace Pharmacy.Infrastructure.Data
{
    public class PharmacyDbContext : DbContext
    {
        public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Member> Members { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<NdcProduct> NdcProducts { get; set; }
        public DbSet<RxClaim> RxClaims { get; set; }
        public DbSet<PriorAuthorizationRecord> PriorAuthorizations { get; set; }
        public DbSet<FinanceTransaction> FinanceTransactions { get; set; }
        public DbSet<OperationRecord> OperationRecords { get; set; }
        public DbSet<RxAdmin> RxAdmins { get; set; }
        public DbSet<RxDURRecord> RxDURRecords { get; set; }
        public DbSet<ClaimSubmitRecord> ClaimSubmitRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Member entity
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MemberNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Gender).HasMaxLength(10);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
            });

            // Configure Provider entity
            modelBuilder.Entity<Provider>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProviderNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.NPI).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Specialty).HasMaxLength(100);
            });

            // Configure NdcProduct entity
            modelBuilder.Entity<NdcProduct>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NDC).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Manufacturer).HasMaxLength(100);
                entity.Property(e => e.DosageForm).HasMaxLength(50);
                entity.Property(e => e.Strength).HasMaxLength(50);
                entity.Property(e => e.PackageSize).HasMaxLength(50);
            });

            // Configure RxClaim entity
            modelBuilder.Entity<RxClaim>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ClaimNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.AmountBilled).HasColumnType("decimal(18,2)");
                entity.Property(e => e.AmountPaid).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).HasMaxLength(50);

                // Configure relationships
                entity.HasOne(e => e.Member)
                    .WithMany(m => m.RxClaims)
                    .HasForeignKey(e => e.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Provider)
                    .WithMany(p => p.RxClaims)
                    .HasForeignKey(e => e.ProviderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.NdcProduct)
                    .WithMany(n => n.RxClaims)
                    .HasForeignKey(e => e.NdcProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure PriorAuthorizationRecord entity
            modelBuilder.Entity<PriorAuthorizationRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AuthorizationNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).HasMaxLength(50);

                // Configure relationships
                entity.HasOne(e => e.Member)
                    .WithMany(m => m.PriorAuthorizations)
                    .HasForeignKey(e => e.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Provider)
                    .WithMany(p => p.PriorAuthorizations)
                    .HasForeignKey(e => e.ProviderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.NdcProduct)
                    .WithMany(n => n.PriorAuthorizations)
                    .HasForeignKey(e => e.NdcProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure FinanceTransaction entity
            modelBuilder.Entity<FinanceTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TransactionNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TransactionType).HasMaxLength(50);
                entity.Property(e => e.Notes).HasMaxLength(500);

                entity.HasOne(e => e.RxClaim)
                    .WithMany(r => r.FinanceTransactions)
                    .HasForeignKey(e => e.RxClaimId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure OperationRecord entity
            modelBuilder.Entity<OperationRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Reference).HasMaxLength(100);
                entity.Property(e => e.OperationType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Details).HasMaxLength(1000);

                entity.HasOne(e => e.RxClaim)
                    .WithMany()
                    .HasForeignKey(e => e.RxClaimId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure RxAdmin entity
            modelBuilder.Entity<RxAdmin>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Role).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(100);
            });

            // Configure RxDURRecord entity
            modelBuilder.Entity<RxDURRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AlertCode).HasMaxLength(20);
                entity.Property(e => e.AlertDescription).HasMaxLength(500);
                entity.Property(e => e.Outcome).HasMaxLength(100);

                entity.HasOne(e => e.RxClaim)
                    .WithMany(r => r.RxDURs)
                    .HasForeignKey(e => e.RxClaimId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ClaimSubmitRecord entity
            modelBuilder.Entity<ClaimSubmitRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SubmittedBy).HasMaxLength(100);
                entity.Property(e => e.SubmissionStatus).HasMaxLength(50);

                entity.HasOne(e => e.RxClaim)
                    .WithMany(r => r.ClaimSubmissions)
                    .HasForeignKey(e => e.RxClaimId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Add indexes for better performance
            modelBuilder.Entity<Member>()
                .HasIndex(e => e.MemberNumber)
                .IsUnique();

            modelBuilder.Entity<Provider>()
                .HasIndex(e => e.ProviderNumber)
                .IsUnique();

            modelBuilder.Entity<Provider>()
                .HasIndex(e => e.NPI);

            modelBuilder.Entity<NdcProduct>()
                .HasIndex(e => e.NDC)
                .IsUnique();

            modelBuilder.Entity<RxClaim>()
                .HasIndex(e => e.ClaimNumber)
                .IsUnique();

            modelBuilder.Entity<PriorAuthorizationRecord>()
                .HasIndex(e => e.AuthorizationNumber)
                .IsUnique();

            modelBuilder.Entity<FinanceTransaction>()
                .HasIndex(e => e.TransactionNumber)
                .IsUnique();
        }
    }
}