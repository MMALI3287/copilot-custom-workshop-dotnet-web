using System.ComponentModel.DataAnnotations;
using MeowWorld.Controllers;
using MeowWorld.Data;
using MeowWorld.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace MeowWorld.Tests.Controllers;

/// <summary>
/// CatsController のユニットテスト
/// </summary>
public class CatsControllerTests
{
    private static CatsController CreateController(AppDbContext context)
    {
        var controller = new CatsController(
            context,
            TestHelpers.CreateLocalizer(),
            NullLogger<CatsController>.Instance);

        // TempData を使うアクションのために最小限の構成を与える
        controller.TempData = new TempDataDictionary(
            new DefaultHttpContext(),
            new NullTempDataProvider());

        return controller;
    }

    // ---- Index ----------------------------------------------------------

    [Fact]
    public async Task 猫一覧が正しく取得できること()
    {
        // Arrange
        using var context = TestHelpers.CreateContext(nameof(猫一覧が正しく取得できること));
        context.Cats.AddRange(
            TestHelpers.NewCat("みけ", 3, "三毛猫"),
            TestHelpers.NewCat("くろ", 5, "黒猫"));
        await context.SaveChangesAsync();
        var controller = CreateController(context);

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Cat>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task 猫が一件もない場合は空の一覧が返ること()
    {
        // Arrange
        using var context = TestHelpers.CreateContext(nameof(猫が一件もない場合は空の一覧が返ること));
        var controller = CreateController(context);

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Cat>>(viewResult.Model);
        Assert.Empty(model);
    }

    // ---- Details --------------------------------------------------------

    [Fact]
    public async Task 存在しないIDでNotFoundが返ること()
    {
        // Arrange
        using var context = TestHelpers.CreateContext(nameof(存在しないIDでNotFoundが返ること));
        var controller = CreateController(context);

        // Act
        var result = await controller.Details(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task IDがnullの場合にNotFoundが返ること()
    {
        // Arrange
        using var context = TestHelpers.CreateContext(nameof(IDがnullの場合にNotFoundが返ること));
        var controller = CreateController(context);

        // Act
        var result = await controller.Details(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task 詳細で該当する猫が取得できること()
    {
        // Arrange
        using var context = TestHelpers.CreateContext(nameof(詳細で該当する猫が取得できること));
        var cat = TestHelpers.NewCat("ソラ", 4, "ロシアンブルー");
        context.Cats.Add(cat);
        await context.SaveChangesAsync();
        var controller = CreateController(context);

        // Act
        var result = await controller.Details(cat.Id);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Cat>(viewResult.Model);
        Assert.Equal("ソラ", model.Name);
    }

    // ---- Create ---------------------------------------------------------

    [Fact]
    public async Task 正常に作成した場合は一覧へリダイレクトすること()
    {
        // Arrange
        using var context = TestHelpers.CreateContext(nameof(正常に作成した場合は一覧へリダイレクトすること));
        var controller = CreateController(context);
        var cat = TestHelpers.NewCat("ハル", 1, "ベンガル");

        // Act
        var result = await controller.Create(cat);

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(CatsController.Index), redirect.ActionName);
        Assert.Equal(1, await context.Cats.CountAsync());
    }

    [Fact]
    public void Nameが空の場合にRequired検証が失敗すること()
    {
        // Arrange
        // ❗ ModelState を手で汚すのではなく、実際の属性を評価する。
        //    こうしないと [Required] を外してもテストが通ってしまう。
        var cat = new Cat { Name = string.Empty, Age = 1, Breed = "雑種" };
        var context = new ValidationContext(cat);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(cat, context, results, validateAllProperties: true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Cat.Name)));
    }

    [Fact]
    public async Task ModelStateが無効なときは保存されないこと()
    {
        // Arrange
        using var context = TestHelpers.CreateContext(nameof(ModelStateが無効なときは保存されないこと));
        var controller = CreateController(context);
        controller.ModelState.AddModelError(nameof(Cat.Name), "Validation_Name_Required");
        var cat = TestHelpers.NewCat(string.Empty);

        // Act
        var result = await controller.Create(cat);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(controller.ModelState.IsValid);
        Assert.Same(cat, viewResult.Model);
        Assert.Equal(0, await context.Cats.CountAsync());
    }

    // ---- Edit -----------------------------------------------------------

    [Fact]
    public async Task 編集後に変更がDBへ反映されること()
    {
        // Arrange
        using var context = TestHelpers.CreateContext(nameof(編集後に変更がDBへ反映されること));
        var cat = TestHelpers.NewCat("ミルク", 2, "ラグドール");
        context.Cats.Add(cat);
        await context.SaveChangesAsync();
        context.Entry(cat).State = EntityState.Detached;

        var controller = CreateController(context);
        var edited = new Cat
        {
            Id = cat.Id,
            Name = "ミルク（改）",
            Age = 3,
            Breed = "ラグドール",
            CreatedAt = cat.CreatedAt
        };

        // Act
        var result = await controller.Edit(cat.Id, edited);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        var reloaded = await context.Cats.AsNoTracking().FirstAsync(c => c.Id == cat.Id);
        Assert.Equal("ミルク（改）", reloaded.Name);
        Assert.Equal(3, reloaded.Age);
    }

    [Fact]
    public async Task 編集でIDが一致しない場合にNotFoundが返ること()
    {
        // Arrange
        using var context = TestHelpers.CreateContext(nameof(編集でIDが一致しない場合にNotFoundが返ること));
        var controller = CreateController(context);
        var cat = TestHelpers.NewCat();
        cat.Id = 1;

        // Act
        var result = await controller.Edit(999, cat);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    // ---- Delete ---------------------------------------------------------

    [Fact]
    public async Task 削除で対象の猫がDBから消えること()
    {
        // Arrange
        using var context = TestHelpers.CreateContext(nameof(削除で対象の猫がDBから消えること));
        var cat = TestHelpers.NewCat("ナナ", 2, "三毛猫");
        context.Cats.Add(cat);
        await context.SaveChangesAsync();
        var controller = CreateController(context);

        // Act
        var result = await controller.DeleteConfirmed(cat.Id);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(0, await context.Cats.CountAsync());
    }

    [Fact]
    public async Task 存在しないIDの削除でNotFoundが返ること()
    {
        // Arrange
        using var context = TestHelpers.CreateContext(nameof(存在しないIDの削除でNotFoundが返ること));
        var controller = CreateController(context);

        // Act
        var result = await controller.DeleteConfirmed(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    // ---- ToggleFavorite --------------------------------------------------

    [Fact]
    public async Task お気に入りの状態が切り替わること()
    {
        // Arrange
        using var context = TestHelpers.CreateContext(nameof(お気に入りの状態が切り替わること));
        var cat = TestHelpers.NewCat("ココ", 3, "ブリティッシュショートヘア");
        context.Cats.Add(cat);
        await context.SaveChangesAsync();
        var controller = CreateController(context);

        // ❗ ExecuteUpdateAsync は DB を直接更新し、変更追跡を経由しない。
        //    追跡済みエンティティは古い値のままなので、AsNoTracking で読み直す。
        //    Web アプリではリクエストごとに DbContext が作られるため問題にならない。
        async Task<bool> ReadFavoriteAsync() =>
            (await context.Cats.AsNoTracking().FirstAsync(c => c.Id == cat.Id)).IsFavorite;

        // Act
        await controller.ToggleFavorite(cat.Id);

        // Assert
        Assert.True(await ReadFavoriteAsync());

        // Act: もう一度押すと元に戻る
        await controller.ToggleFavorite(cat.Id);

        // Assert
        Assert.False(await ReadFavoriteAsync());
    }

    private sealed class NullTempDataProvider : ITempDataProvider
    {
        public IDictionary<string, object?> LoadTempData(HttpContext context) =>
            new Dictionary<string, object?>();

        public void SaveTempData(HttpContext context, IDictionary<string, object?> values) { }
    }
}
