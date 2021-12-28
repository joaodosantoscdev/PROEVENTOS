using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class ProEventosContext : DbContext
    {
        public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<SpeakerEvent> SpeakerEvents { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpeakerEvent>()
                .HasKey(pe => new { pe.EventId, pe.SpeakerId });

            modelBuilder.Entity<Event>()
                .HasMany(e => e.SocialMedias)
                .WithOne(sm => sm.Event)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Speaker>()
                .HasMany(s => s.SocialMedias)
                .WithOne(sm => sm.Speaker)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
