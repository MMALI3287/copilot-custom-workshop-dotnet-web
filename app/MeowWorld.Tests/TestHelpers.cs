using MeowWorld;
using MeowWorld.Data;
using MeowWorld.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace MeowWorld.Tests;

/// <summary>
/// テスト共通のヘルパー
/// </summary>
internal static class TestHelpers
{
    /// <summary>
    /// テストごとに独立した SQLite インメモリ DB のコンテキストを作る。
    ///
    /// ❗ EF Core の InMemory プロバイダーではなく SQLite を使う理由:
    ///    InMemory はリレーショナル DB ではないため、`ExecuteUpdateAsync` のような
    ///    SQL を発行する API が動作せず、実運用と挙動が食い違う。
    ///    SQLite インメモリなら本番と同じプロバイダーで検証できる。
    ///
    /// 返されたコンテキストを破棄すると接続も閉じ、DB は消える。
    /// </summary>
    public static AppDbContext CreateContext(string dbName)
    {
        // 共有キャッシュ付きの名前付きインメモリ DB。名前が違えば互いに独立する。
        var connection = new SqliteConnection($"Data Source={dbName};Mode=Memory;Cache=Shared");
        connection.Open();   // 接続を開いている間だけ DB が存在する

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new TestAppDbContext(options, connection);
        context.Database.EnsureCreated();

        // シードデータはテスト側で明示的に用意したいので消しておく
        context.Cats.RemoveRange(context.Cats);
        context.SaveChanges();

        return context;
    }

    /// <summary>キー名をそのまま返すダミーのローカライザー</summary>
    public static IStringLocalizer<SharedResource> CreateLocalizer() => new PassThroughLocalizer();

    /// <summary>検証用の猫を作る</summary>
    public static Cat NewCat(string name = "テスト猫", int age = 1, string breed = "雑種") =>
        new() { Name = name, Age = age, Breed = breed };

    /// <summary>破棄時に SQLite 接続も閉じるコンテキスト</summary>
    private sealed class TestAppDbContext(DbContextOptions<AppDbContext> options, SqliteConnection connection)
        : AppDbContext(options)
    {
        public override void Dispose()
        {
            base.Dispose();
            connection.Dispose();
        }
    }

    private sealed class PassThroughLocalizer : IStringLocalizer<SharedResource>
    {
        public LocalizedString this[string name] => new(name, name, resourceNotFound: false);

        public LocalizedString this[string name, params object[] arguments] =>
            new(name, string.Format(name, arguments), resourceNotFound: false);

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => [];
    }
}
