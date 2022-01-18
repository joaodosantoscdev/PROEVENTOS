using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class ProEventosContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>,
                                                       UserRole, IdentityUserLogin<int>,
                                                       IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<SpeakerEvent> SpeakerEvents { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole => 
                {
                    userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                    userRole.HasOne(ur => ur.User)
                            .WithMany(r => r.UserRoles)    
                            .HasForeignKey(ur => ur.UserId)
                            .IsRequired();

                    userRole.HasOne(ur => ur.Role)
                            .WithMany(r => r.UserRoles)
                            .HasForeignKey(ur => ur.RoleId)
                            .IsRequired();
                }
            );

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
