using MeowWorld;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;

namespace MeowWorld.Tests;

/// <summary>
/// 日英リソースの整合性テスト。
/// 片方の言語にだけキーを足す事故を防ぐ。
/// </summary>
public class LocalizationTests
{
    private static IStringLocalizer CreateLocalizer()
    {
        var services = new ServiceCollection();
        // ResourceManagerStringLocalizerFactory は ILoggerFactory を要求する
        services.AddLogging();
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IStringLocalizerFactory>();

        return factory.Create(typeof(SharedResource));
    }

    private static IReadOnlyDictionary<string, string> StringsFor(string culture)
    {
        var previous = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
            return CreateLocalizer()
                .GetAllStrings(includeParentCultures: false)
                .ToDictionary(s => s.Name, s => s.Value);
        }
        finally
        {
            CultureInfo.CurrentUICulture = previous;
        }
    }

    [Fact]
    public void 日本語リソースが読み込めること()
    {
        // Arrange & Act
        var ja = StringsFor("ja");

        // Assert
        Assert.NotEmpty(ja);
        Assert.Equal("猫一覧", ja["Nav_CatList"]);
    }

    [Fact]
    public void 英語リソースが読み込めること()
    {
        // Arrange & Act
        var en = StringsFor("en");

        // Assert
        Assert.NotEmpty(en);
        Assert.Equal("Cat List", en["Nav_CatList"]);
    }

    [Fact]
    public void 日英のキー集合が完全に一致すること()
    {
        // Arrange
        var ja = StringsFor("ja").Keys.ToHashSet();
        var en = StringsFor("en").Keys.ToHashSet();

        // Act
        var missingInEn = ja.Except(en).OrderBy(k => k).ToList();
        var missingInJa = en.Except(ja).OrderBy(k => k).ToList();

        // Assert
        Assert.True(
            missingInEn.Count == 0,
            $"英語リソースに不足しているキー: {string.Join(", ", missingInEn)}");
        Assert.True(
            missingInJa.Count == 0,
            $"日本語リソースに不足しているキー: {string.Join(", ", missingInJa)}");
    }

    [Fact]
    public void すべてのキーに空でない値があること()
    {
        foreach (var culture in new[] { "ja", "en" })
        {
            // Arrange & Act
            var strings = StringsFor(culture);

            // Assert
            var empty = strings.Where(kv => string.IsNullOrWhiteSpace(kv.Value))
                               .Select(kv => kv.Key)
                               .ToList();
            Assert.True(empty.Count == 0, $"{culture} に空の値があります: {string.Join(", ", empty)}");
        }
    }

    [Theory]
    [InlineData("Nav_CatList")]
    [InlineData("Col_Name")]
    [InlineData("Action_Details")]
    [InlineData("Validation_Name_Required")]
    public void 主要キーで日英の値が異なること(string key)
    {
        // Arrange
        var ja = StringsFor("ja");
        var en = StringsFor("en");

        // Act & Assert
        // 値が同一なら、英語リソースが未翻訳のまま残っている可能性が高い
        Assert.NotEqual(ja[key], en[key]);
    }
}
