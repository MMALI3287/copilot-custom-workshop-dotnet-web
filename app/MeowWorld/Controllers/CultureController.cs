using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace MeowWorld.Controllers;

/// <summary>
/// 表示言語の切り替えを担当するコントローラー
/// </summary>
[Route("[controller]/[action]")]
public class CultureController(IStringLocalizerFactory localizerFactory) : Controller
{
    /// <summary>このアプリがサポートする言語</summary>
    private static readonly string[] Supported = ["ja", "en"];

    /// <summary>
    /// 選択された言語をクッキーに保存し、元のページへ戻る。
    /// JavaScript が無効な環境向けのフォールバック経路。
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Set(string culture, string? returnUrl)
    {
        if (!string.IsNullOrEmpty(culture) && Supported.Contains(culture))
        {
            AppendCultureCookie(culture);
        }

        // ❗ Redirect ではなく LocalRedirect（オープンリダイレクト対策）
        return LocalRedirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
    }

    /// <summary>
    /// 指定言語の UI 文字列を JSON で返す。
    /// クライアント側で即時に表示を切り替えるために使用する。
    /// クッキーも同時に更新するため、次のリクエスト以降はサーバー側も追従する。
    /// </summary>
    [HttpGet]
    public IActionResult Strings(string culture)
    {
        if (string.IsNullOrEmpty(culture) || !Supported.Contains(culture))
        {
            return BadRequest(new { error = "Unsupported culture." });
        }

        var requested = new CultureInfo(culture);
        var previous = CultureInfo.CurrentUICulture;

        try
        {
            // 要求された言語でリソースを読むため、一時的に UI カルチャを差し替える
            CultureInfo.CurrentUICulture = requested;

            var localizer = localizerFactory.Create(typeof(SharedResource));
            var strings = localizer.GetAllStrings(includeParentCultures: true)
                .ToDictionary(s => s.Name, s => s.Value);

            AppendCultureCookie(culture);

            return Json(strings);
        }
        finally
        {
            CultureInfo.CurrentUICulture = previous;
        }
    }

    /// <summary>カルチャクッキーを 1 年間有効で書き込む</summary>
    private void AppendCultureCookie(string culture)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,  // 同意前でも送出する（機能上必須のため）
                HttpOnly = false,    // クライアント側から現在の言語を読むため
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });
    }
}
