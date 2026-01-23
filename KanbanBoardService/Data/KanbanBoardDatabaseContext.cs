using Microsoft.EntityFrameworkCore;
using KanbanBoardService.Models;

namespace KanbanBoardService.Data
{
    /// <summary>
    /// EF Core DbContext for the Kanban board service.
    /// Exposes DbSet properties for the domain entities and configures relationships.
    /// </summary>
    public class KanbanBoardDatabaseContext : DbContext
    {
        public KanbanBoardDatabaseContext(DbContextOptions<KanbanBoardDatabaseContext> options) : base(options)
        {
        }

        /// <summary>
        /// Boards table
        /// </summary>
        public DbSet<Board> Boards { get; set; }

        /// <summary>
        /// Phases table
        /// </summary>
        public DbSet<Phase> Phases { get; set; }

        /// <summary>
        /// PhaseTransitions table
        /// </summary>
        public DbSet<PhaseTransitions> PhaseTransitions { get; set; }

        /// <summary>
        /// Tasks table.
        /// </summary>
        public DbSet<Models.Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure table names (attributes on models already set, but keep fluent mapping for clarity)
            modelBuilder.Entity<Models.Task>().ToTable("Tasks");
            modelBuilder.Entity<Board>().ToTable("Boards");
            modelBuilder.Entity<Phase>().ToTable("Phases");
            modelBuilder.Entity<PhaseTransitions>().ToTable("PhaseTransitions");

            // Board -> Phases (one-to-many)
            modelBuilder.Entity<Phase>()
                .HasOne(p => p.Board)
                .WithMany(b => b.Phases)
                .HasForeignKey(p => p.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            // PhaseTransitions: FromPhase -> Phase (one-to-many)
            modelBuilder.Entity<PhaseTransitions>()
                .HasOne(pt => pt.FromPhase)
                .WithMany(p => p.PhaseMovement)
                .HasForeignKey(pt => pt.FromPhaseId)
                .OnDelete(DeleteBehavior.Cascade);

            // PhaseTransitions: ToPhase -> Phase (many-to-one without collection)
            modelBuilder.Entity<PhaseTransitions>()
                .HasOne(pt => pt.ToPhase)
                .WithMany()
                .HasForeignKey(pt => pt.ToPhaseId)
                .OnDelete(DeleteBehavior.Cascade);

            // PhaseTransitions: Task -> Phase (many-to-one)
            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.Phase)
                .WithMany(pt=>pt.Tasks)
                .HasForeignKey(pt => pt.PhaseId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}