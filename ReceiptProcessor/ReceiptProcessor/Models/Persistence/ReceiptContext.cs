using Microsoft.EntityFrameworkCore;

namespace ReceiptProcessor.Models.Persistence
{
    public class ReceiptContext : DbContext
    {
        public ReceiptContext() {
            Database.EnsureCreated();
        }

        public DbSet<Receipt> Receipts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            string directory = Path.GetFullPath(Directory.GetCurrentDirectory());
            string filename = Path.Join(directory, "/data", "Receipts.db");

            if (!File.Exists(filename))
            {
                File.Create(filename);
            }

            builder.UseSqlite($"Data Source={filename}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Receipt>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Item>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<Receipt>()
                .HasMany(r => r.Items)
                .WithOne(i => i.Receipt)
                .HasForeignKey(i => i.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
