using MeowWorld;
using MeowWorld.Data;
using MeowWorld.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace MeowWorld.Tests;

/// <summary>
/// テスト共通のヘルパー
/// </summary>
internal static class TestHelpers
{
    /// <summary>テストごとに一意な InMemory DB のコンテキストを作る</summary>
    public static AppDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new AppDbContext(options);
    }

    /// <summary>キー名をそのまま返すダミーのローカライザー</summary>
    public static IStringLocalizer<SharedResource> CreateLocalizer() => new PassThroughLocalizer();

    /// <summary>検証用の猫を作る</summary>
    public static Cat NewCat(string name = "テスト猫", int age = 1, string breed = "雑種") =>
        new() { Name = name, Age = age, Breed = breed };

    private sealed class PassThroughLocalizer : IStringLocalizer<SharedResource>
    {
        public LocalizedString this[string name] => new(name, name, resourceNotFound: false);

        public LocalizedString this[string name, params object[] arguments] =>
            new(name, string.Format(name, arguments), resourceNotFound: false);

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => [];
    }
}
