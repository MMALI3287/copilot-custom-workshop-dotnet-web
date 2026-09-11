using MeowWorld.Models;
using Microsoft.EntityFrameworkCore;

namespace MeowWorld.Data;

/// <summary>
/// アプリケーションデータベースコンテキスト
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>猫テーブル</summary>
    public DbSet<Cat> Cats => Set<Cat>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // シードデータ（教材 Step 5 と同一）
        modelBuilder.Entity<Cat>().HasData(
            new Cat { Id = 1, Name = "みけ", Age = 3, Breed = "三毛猫", Description = "おとなしい性格", CreatedAt = new DateTime(2024, 1, 1) },
            new Cat { Id = 2, Name = "くろ", Age = 5, Breed = "黒猫", Description = "甘えん坊", CreatedAt = new DateTime(2024, 1, 1) },
            new Cat { Id = 3, Name = "しろ", Age = 2, Breed = "白猫", Description = null, CreatedAt = new DateTime(2024, 1, 1) },
            new Cat { Id = 4, Name = "チャチャ", Age = 1, Breed = "茶トラ", Description = "元気いっぱい", CreatedAt = new DateTime(2024, 1, 1) },
            new Cat { Id = 5, Name = "ソラ", Age = 4, Breed = "ロシアンブルー", Description = "静かな環境が好き", CreatedAt = new DateTime(2024, 1, 1) }
        );
    }
}
