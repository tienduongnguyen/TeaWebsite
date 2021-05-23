using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace taka.Models.Enitities
{
    public partial class TakaDBContext : DbContext
    {
        public TakaDBContext()
            : base("name=TakaDBContext")
        {
        }

        public virtual DbSet<ADDRESS> ADDRESSes { get; set; }
        public virtual DbSet<CART> CARTs { get; set; }
        public virtual DbSet<CATEGORY> CATEGORies { get; set; }
        public virtual DbSet<IMAGE> IMAGEs { get; set; }
        public virtual DbSet<ORDER> ORDERs { get; set; }
        public virtual DbSet<ORDER_DETAIL> ORDER_DETAIL { get; set; }
        public virtual DbSet<RATE> RATEs { get; set; }
        public virtual DbSet<TEA> TEAs { get; set; }
        public virtual DbSet<USER> USERs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ADDRESS>()
                .Property(e => e.PHONE)
                .IsUnicode(false);

            modelBuilder.Entity<ADDRESS>()
                .HasMany(e => e.ORDERs)
                .WithOptional(e => e.ADDRESS)
                .HasForeignKey(e => e.ID_ADDRESS);

            modelBuilder.Entity<CATEGORY>()
                .HasMany(e => e.TEAs)
                .WithOptional(e => e.CATEGORY)
                .HasForeignKey(e => e.ID_CATEGORY);

            modelBuilder.Entity<IMAGE>()
                .Property(e => e.URL)
                .IsUnicode(false);

            modelBuilder.Entity<ORDER>()
                .Property(e => e.TOTAL_PRICE)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ORDER>()
                .HasMany(e => e.ORDER_DETAIL)
                .WithOptional(e => e.ORDER)
                .HasForeignKey(e => e.ID_ORDER);

            modelBuilder.Entity<TEA>()
                .Property(e => e.PRICE)
                .HasPrecision(18, 0);

            modelBuilder.Entity<TEA>()
                .HasMany(e => e.CARTs)
                .WithOptional(e => e.TEA)
                .HasForeignKey(e => e.ID_TEA);

            modelBuilder.Entity<TEA>()
                .HasMany(e => e.IMAGEs)
                .WithOptional(e => e.TEA)
                .HasForeignKey(e => e.ID_TEA);

            modelBuilder.Entity<TEA>()
                .HasMany(e => e.ORDER_DETAIL)
                .WithOptional(e => e.TEA)
                .HasForeignKey(e => e.ID_TEA);

            modelBuilder.Entity<TEA>()
                .HasMany(e => e.RATEs)
                .WithOptional(e => e.TEA)
                .HasForeignKey(e => e.ID_TEA);

            modelBuilder.Entity<USER>()
                .Property(e => e.PHONE)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .HasMany(e => e.ADDRESSes)
                .WithOptional(e => e.USER)
                .HasForeignKey(e => e.ID_USER);

            modelBuilder.Entity<USER>()
                .HasMany(e => e.CARTs)
                .WithOptional(e => e.USER)
                .HasForeignKey(e => e.ID_USER);

            modelBuilder.Entity<USER>()
                .HasMany(e => e.ORDERs)
                .WithOptional(e => e.USER)
                .HasForeignKey(e => e.ID_USER);

            modelBuilder.Entity<USER>()
                .HasMany(e => e.RATEs)
                .WithOptional(e => e.USER)
                .HasForeignKey(e => e.ID_USER);
        }
    }
}
