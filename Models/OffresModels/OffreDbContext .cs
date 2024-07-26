using Microsoft.EntityFrameworkCore;
using Recrutement_plateforme.Models.OffresModels;

public class OffreDbContext : DbContext
{
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Recruteur> Recruteurs { get; set; }
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<CandidateJob> CandidateJobs { get; set; }
    public DbSet<CompleteProfile> Profile { get; set; }
    public OffreDbContext(DbContextOptions<OffreDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureCandidateJobEntity(modelBuilder);

        // Add other configurations as needed
        modelBuilder.Entity<Recruteur>()
             .HasMany(r => r.Jobs)
             .WithOne(j => j.Recruteur)
             .HasForeignKey(j => j.RecruteurId)
             .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureCandidateJobEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CandidateJob>()
            .HasKey(aj => new { aj.CandidateId, aj.JobId });

        // Configure relationships
        modelBuilder.Entity<CandidateJob>()
            .HasOne(cj => cj.Candidate)
            .WithMany(c => c.Applications)
            .HasForeignKey(cj => cj.CandidateId)
            .OnDelete(DeleteBehavior.Cascade); // Adjust the delete behavior as needed

        modelBuilder.Entity<CandidateJob>()
            .HasOne(cj => cj.Job)
            .WithMany(j => j.Candidates)
            .HasForeignKey(cj => cj.JobId)
            .OnDelete(DeleteBehavior.Cascade); // Adjust the delete behavior as needed

        modelBuilder.Entity<CompleteProfile>()
    .HasOne(cp => cp.Candidate)
    .WithOne(c => c.CompleteProfile)
    .HasForeignKey<CompleteProfile>(cp => cp.CandidateId)
    .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<Recrutement_plateforme.Models.OffresModels.CandidateJob>? CandidateJob { get; set; }
}
