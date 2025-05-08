using Microsoft.EntityFrameworkCore;
using GunStoreIMS.Domain.Models;


namespace GunStoreIMS.Persistence.Data
{
    public class FirearmsInventoryDB : DbContext
    {
        public FirearmsInventoryDB(DbContextOptions<FirearmsInventoryDB> options) : base(options)
        {
        }

        public DbSet<Form4473Record> Form4473Records { get; set; }
        public DbSet<Form4473FirearmLine> Form4473FirearmLines { get; set; }

        // Optional: override OnModelCreating to configure relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Form4473Record>()
                .HasMany(r => r.FirearmLines)
                .WithOne(l => l.Form4473Record)
                .HasForeignKey(l => l.Form4473RecordId)
                .IsRequired()              // a line cannot exist without its 4473
                .OnDelete(DeleteBehavior.Cascade);   // record delete ⇒ child lines delete
        }


    }

}
