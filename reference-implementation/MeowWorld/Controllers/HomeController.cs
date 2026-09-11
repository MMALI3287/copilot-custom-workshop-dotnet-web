using System.Diagnostics;
using MeowWorld.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeowWorld.Controllers;

/// <summary>
/// トップページと共通ページを担当するコントローラー
/// </summary>
public class HomeController(ILogger<HomeController> logger) : Controller
{
    /// <summary>ダッシュボード（トップページ）</summary>
    public IActionResult Index() => View();

    /// <summary>プライバシーポリシー</summary>
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
