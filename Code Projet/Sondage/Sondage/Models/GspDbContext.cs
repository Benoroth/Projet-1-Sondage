namespace Sondage.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GspDbContext : DbContext
    {
        public GspDbContext()
            : base("name=SondageBDDContext")
        {
        }

        public virtual DbSet<TChoix> TChoix { get; set; }
        public virtual DbSet<TSondage> TSondage { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TSondage>()
                .Property(e => e.dateSondage)
                .IsFixedLength();

            modelBuilder.Entity<TSondage>()
                .Property(e => e.lienResult)
                .IsUnicode(false);

            modelBuilder.Entity<TSondage>()
                .Property(e => e.lienPartage)
                .IsUnicode(false);

            modelBuilder.Entity<TSondage>()
                .Property(e => e.lienSuppr)
                .IsUnicode(false);
        }
    }
}
