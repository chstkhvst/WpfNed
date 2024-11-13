using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WpfNed.EF
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<DealType> DealType { get; set; }
        public virtual DbSet<REObject> Object { get; set; }
        public virtual DbSet<ObjectType> ObjectType { get; set; }
        public virtual DbSet<Owner> Owner { get; set; }
        public virtual DbSet<Reservation> Reservation { get; set; }
        public virtual DbSet<ResStatus> ResStatus { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<ObjectImage> ObjectImage { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DealType>()
                .Property(e => e.DealName)
                .IsUnicode(false);

            modelBuilder.Entity<DealType>()
                .HasMany(e => e.Object)
                .WithRequired(e => e.DealType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<REObject>()
                .Property(e => e.Street)
                .IsUnicode(false);

            modelBuilder.Entity<REObject>()
                .HasMany(e => e.ObjectImage)
                .WithRequired(e => e.Object)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<REObject>()
                .HasMany(e => e.Reservation)
                .WithRequired(e => e.Object)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ObjectType>()
                .Property(e => e.TypeName)
                .IsUnicode(false);

            modelBuilder.Entity<ObjectType>()
                .HasMany(e => e.Object)
                .WithRequired(e => e.ObjectType)
                .HasForeignKey(e => e.TypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Owner>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<Owner>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Owner>()
                .Property(e => e.Passport)
                .IsUnicode(false);

            modelBuilder.Entity<Owner>()
                .HasMany(e => e.Object)
                .WithRequired(e => e.Owner)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reservation>()
                .HasMany(e => e.Contract)
                .WithRequired(e => e.Reservation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ResStatus>()
                .Property(e => e.StatusType)
                .IsUnicode(false);

            modelBuilder.Entity<ResStatus>()
                .HasMany(e => e.Reservation)
                .WithRequired(e => e.ResStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Status>()
                .Property(e => e.StatusName)
                .IsUnicode(false);

            modelBuilder.Entity<Status>()
                .HasMany(e => e.Object)
                .WithRequired(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Passport)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Login)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Contract)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Reservation)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserRole>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<UserRole>()
                .HasMany(e => e.User)
                .WithRequired(e => e.UserRole)
                .HasForeignKey(e => e.RoleId)
                .WillCascadeOnDelete(false);
        }
    }
}
