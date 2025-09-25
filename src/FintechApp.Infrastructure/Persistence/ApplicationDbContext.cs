using FintechApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserWallet> UserWallets { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<Currency> Currencies { get; set; } = null!;

    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<RolePermission> RolePermissions { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    
    public DbSet<TransactionEntry> TransactionEntries { get; set; } = null!;

    public DbSet<ApiPermission> ApiPermissions { get; set; } = null!;
    public DbSet<Notification> notifications { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map tên bảng chính xác với PostgreSQL
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<UserWallet>().ToTable("UserWallets");
        modelBuilder.Entity<Transaction>().ToTable("Transactions");
        modelBuilder.Entity<Currency>().ToTable("Currencies");

        modelBuilder.Entity<Role>().ToTable("Roles");
        modelBuilder.Entity<Permission>().ToTable("Permissions");
        modelBuilder.Entity<UserRole>().ToTable("UserRoles");
        modelBuilder.Entity<RolePermission>().ToTable("RolePermissions");
        modelBuilder.Entity<RefreshToken>().ToTable("RefreshTokens");

        modelBuilder.Entity<TransactionEntry>().ToTable("TransactionEntries");
        modelBuilder.Entity<ApiPermission>().ToTable("ApiPermissions");

        // PKs
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
        modelBuilder.Entity<UserWallet>().HasKey(w => w.WalletId);
        modelBuilder.Entity<Transaction>().HasKey(t => t.TransactionId);
        modelBuilder.Entity<Currency>().HasKey(c => c.CurrencyId);
        modelBuilder.Entity<RefreshToken>().HasKey(rt => rt.RefreshTokenId);

        // Composite keys
        modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
        modelBuilder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });

        // Relationships
        modelBuilder.Entity<UserWallet>()
            .HasOne(w => w.User)
            .WithMany(u => u.Wallets)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.FromWallet)
            .WithMany(w => w.FromTransactions)
            .HasForeignKey(t => t.FromWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ToWallet)
            .WithMany(w => w.ToTransactions)
            .HasForeignKey(t => t.ToWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserWallet>()
            .HasOne(w => w.Currency)
            .WithMany(c => c.Wallets)
            .HasForeignKey(w => w.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);
    }

}
