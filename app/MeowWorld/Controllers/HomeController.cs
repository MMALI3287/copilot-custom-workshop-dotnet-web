using System.Diagnostics;
using MeowWorld.Data;
using MeowWorld.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeowWorld.Controllers;

/// <summary>
/// トップページと共通ページを担当するコントローラー
/// </summary>
public class HomeController(AppDbContext context, ILogger<HomeController> logger) : Controller
{
    /// <summary>トップページ。統計とプレビュー用に猫の一覧を渡す</summary>
    public async Task<IActionResult> Index()
    {
        var cats = await context.Cats
            .OrderBy(c => c.Id)
            .AsNoTracking()
            .ToListAsync();

        return View(cats);
    }

    /// <summary>このサイトについて</summary>
    public IActionResult Privacy() => View();

    /// <summary>エラーページ</summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        logger.LogWarning("エラーページを表示しました。RequestId={RequestId}", requestId);
        return View(new ErrorViewModel { RequestId = requestId });
    }
}
