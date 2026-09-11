using MeowWorld.Data;
using MeowWorld.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace MeowWorld.Controllers;

/// <summary>
/// 猫情報の CRUD を担当するコントローラー
/// </summary>
public class CatsController(
    AppDbContext context,
    IStringLocalizer<SharedResource> localizer,
    ILogger<CatsController> logger) : Controller
{
    /// <summary>猫の一覧を表示する</summary>
    public async Task<IActionResult> Index()
    {
        var cats = await context.Cats
            .OrderBy(c => c.Id)
            .AsNoTracking()
            .ToListAsync();

        return View(cats);
    }

    /// <summary>猫の詳細を表示する</summary>
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var cat = await context.Cats
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return cat is null ? NotFound() : View(cat);
    }

    /// <summary>登録フォームを表示する</summary>
    public IActionResult Create() => View();

    /// <summary>猫を新規登録する</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Age,Breed,Description,IsFavorite")] Cat cat)
    {
        if (!ModelState.IsValid)
        {
            return View(cat);
        }

        try
        {
            cat.CreatedAt = DateTime.Now;
            context.Add(cat);
            await context.SaveChangesAsync();

            TempData["Flash"] = localizer["Msg_CreateSuccess"].Value;
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "猫の登録に失敗しました。Name={Name}", cat.Name);
            ModelState.AddModelError(string.Empty, localizer["Msg_NotFound"]);
            return View(cat);
        }
    }

    /// <summary>編集フォームを表示する</summary>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var cat = await context.Cats.FindAsync(id);
        return cat is null ? NotFound() : View(cat);
    }

    /// <summary>猫の情報を更新する</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Breed,Description,IsFavorite,CreatedAt")] Cat cat)
    {
        if (id != cat.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(cat);
        }

        try
        {
            context.Update(cat);
            await context.SaveChangesAsync();

            TempData["Flash"] = localizer["Msg_UpdateSuccess"].Value;
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!await CatExistsAsync(cat.Id))
            {
                return NotFound();
            }

            logger.LogError(ex, "猫の更新で同時実行の競合が発生しました。Id={Id}", cat.Id);
            throw;
        }
    }

    /// <summary>削除確認画面を表示する</summary>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var cat = await context.Cats
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return cat is null ? NotFound() : View(cat);
    }

    /// <summary>猫を削除する</summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cat = await context.Cats.FindAsync(id);
        if (cat is null)
        {
            return NotFound();
        }

        context.Cats.Remove(cat);
        await context.SaveChangesAsync();

        TempData["Flash"] = localizer["Msg_DeleteSuccess"].Value;
        return RedirectToAction(nameof(Index));
    }

    /// <summary>お気に入り状態を切り替える（Step 9 で追加した機能）</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleFavorite(int id)
    {
        var cat = await context.Cats.FindAsync(id);
        if (cat is null)
        {
            return NotFound();
        }

        cat.IsFavorite = !cat.IsFavorite;
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private Task<bool> CatExistsAsync(int id) => context.Cats.AnyAsync(c => c.Id == id);
}
