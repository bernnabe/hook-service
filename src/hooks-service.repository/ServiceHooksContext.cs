using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ServiceHooks.Domain;


namespace ServiceHooks.Repository
{
    public partial class ServiceHooksContext : DbContext
    {
        public ServiceHooksContext()
        {
        }

        public ServiceHooksContext(DbContextOptions<ServiceHooksContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);

            if (optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=sndboxwin701-PC\\sqlexpress;Database=ServiceHooks;User Id=sa;Password=123456");
            }


        }

        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventDetail> EventDetail { get; set; }
        public virtual DbSet<EventResult> EventResult { get; set; }
        public virtual DbSet<EventResultType> EventResultType { get; set; }
        public virtual DbSet<EventStatus> EventStatus { get; set; }
        public virtual DbSet<EventType> EventType { get; set; }
        public virtual DbSet<Suscription> Suscription { get; set; }
        public virtual DbSet<SuscriptionDetail> SuscriptionDetail { get; set; }
        public virtual DbSet<SuscriptionType> SuscriptionType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("date");

                entity.Property(e => e.UpdatedUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);


                entity.Property(e => e.UpdatedUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.EventStatus)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.EventStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Event_EventStatus");
            });

            modelBuilder.Entity<EventDetail>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventDetail)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventDetail_Event");

                entity.HasOne(d => d.SuscriptionDetail)
                    .WithMany(p => p.EventDetail)
                    .HasForeignKey(d => d.SuscriptionDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventDetail_SuscriptionDetail");
            });

            modelBuilder.Entity<EventResult>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.EventDetail)
                    .WithMany(p => p.EventResult)
                    .HasForeignKey(d => d.EventDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventResult_EventDetail");

                entity.HasOne(d => d.EventResultType)
                    .WithMany(p => p.EventResult)
                    .HasForeignKey(d => d.EventResultTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventResult_EventResultType");
            });

            modelBuilder.Entity<EventResultType>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EventStatus>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Suscription>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.Suscription)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Suscription_Application");
            });

            modelBuilder.Entity<SuscriptionDetail>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WebHookUrl)
                    .IsRequired()
                    .HasMaxLength(2056)
                    .IsUnicode(false);

                entity.HasOne(d => d.Suscription)
                    .WithMany(p => p.SuscriptionDetail)
                    .HasForeignKey(d => d.SuscriptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SuscriptionDetail_Suscription");

                entity.HasOne(d => d.SuscriptionType)
                    .WithMany(p => p.SuscriptionDetail)
                    .HasForeignKey(d => d.SuscriptionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SuscriptionDetail_SuscriptionType");
            });

            modelBuilder.Entity<SuscriptionType>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
