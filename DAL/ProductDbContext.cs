using BOL;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options):base(options){}

        public DbSet<ProductObj> Products { get; set; }
        public DbSet<ApprovalQueueObj> ApprovalQueues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductObj>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            //modelBuilder.Entity<ProductObj>()
            //    .HasMany(p => p.ApprovalQueueObj)
            //    .WithOne(a => a.Product)
            //    .HasForeignKey(a => a.ProductId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
