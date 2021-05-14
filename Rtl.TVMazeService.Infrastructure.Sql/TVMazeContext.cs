using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rtl.TVMazeService.Domain.Models;
using Rtl.TVMazeService.Infrastructure.Sql.Configuration;

namespace Rtl.TVMazeService.Infrastructure.Sql
{
    public class TVMazeContext : DbContext
    {
        private readonly SqlServerConfig sqlServerOptions;

        public TVMazeContext() { }

        public TVMazeContext(DbContextOptions<TVMazeContext> options, IOptionsSnapshot<SqlServerConfig> optionsSnapshot)
            : base(options) => this.sqlServerOptions = optionsSnapshot.Value;

        public virtual DbSet<Show> Shows { get; set; }
        public virtual DbSet<CastMember> CastMembers { get; set; }
        public DbSet<ShowCastMember> ShowCastMembers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                _ = optionsBuilder.UseSqlServer(this.sqlServerOptions.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<ShowCastMember>()
                .HasKey(scm => new { scm.ShowId, scm.CastMemberId });
            _ = modelBuilder.Entity<ShowCastMember>()
                .HasOne<Show>(scm => scm.Show)
                .WithMany(s => s.ShowCastMembers)
                .HasForeignKey(scm => scm.ShowId);
            _ = modelBuilder.Entity<ShowCastMember>()
                .HasOne<CastMember>(scm => scm.CastMember)
                .WithMany(cm => cm.ShowCastMembers)
                .HasForeignKey(scm => scm.CastMemberId);

            _ = modelBuilder.Entity<Show>()
                .HasIndex(s => s.MazeId)
                .IsUnique();
            _ = modelBuilder.Entity<CastMember>()
                .HasIndex(cm => cm.MazeId)
                .IsUnique();
            _ = modelBuilder.Entity<CastMember>()
                .HasIndex(cm => cm.BirthDay)
                .IsUnique(false);
        }
    }
}
