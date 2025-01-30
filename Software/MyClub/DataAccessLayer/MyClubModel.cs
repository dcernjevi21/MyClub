using EntitiesLayer.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer
{
    public partial class MyClubContext : DbContext
    {
        public MyClubContext()
            : base("name=MyClubContext")
        {
        }

        public virtual DbSet<AthleteEvaluation> AthleteEvaluations { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Club> Clubs { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Membership> Memberships { get; set; }
        public virtual DbSet<Method> Methods { get; set; }
        public virtual DbSet<RoleType> RoleTypes { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Training> Trainings { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AthleteEvaluation>()
                .Property(e => e.Mark)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Club>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.Teams)
                .WithRequired(e => e.Club)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Match>()
                .Property(e => e.OpponentTeam)
                .IsUnicode(false);

            modelBuilder.Entity<Match>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<Match>()
                .Property(e => e.Result)
                .IsUnicode(false);

            modelBuilder.Entity<Membership>()
                .Property(e => e.Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Method>()
                .Property(e => e.MethodName)
                .IsUnicode(false);

            modelBuilder.Entity<Method>()
                .HasMany(e => e.Memberships)
                .WithRequired(e => e.Method)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RoleType>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<Status>()
                .Property(e => e.StatusName)
                .IsUnicode(false);

            modelBuilder.Entity<Status>()
                .HasMany(e => e.Attendances)
                .WithRequired(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Status>()
                .HasMany(e => e.Trainings)
                .WithRequired(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Team>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Matches)
                .WithRequired(e => e.Team)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Trainings)
                .WithRequired(e => e.Team)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Training>()
                .HasMany(e => e.Attendances)
                .WithRequired(e => e.Training)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.AthleteEvaluations)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Attendances)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Memberships)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
