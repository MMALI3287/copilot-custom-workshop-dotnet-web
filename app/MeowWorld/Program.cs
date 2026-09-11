using System.Globalization;
using MeowWorld;
using MeowWorld.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// モデルバインディングのメッセージ解決に使う。app.Build() 後に代入する。
IServiceProvider? localizationServices = null;

// ---- ローカライズ（日英切り替え）----------------------------------------
// リソースは Resources/SharedResource.{culture}.resx に配置する
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews(options =>
    {
        // 値型（int など）が空で送られたときの既定メッセージは
        // DataAnnotations ではなくモデルバインダーが生成するため、
        // ここで個別にローカライズする。
        // 未設定だと英語の "The {0} field is required." が混ざる。
        var provider = options.ModelBindingMessageProvider;

        provider.SetValueIsInvalidAccessor(
            value => Localize("Validation_ValueIsInvalid", value));
        provider.SetValueMustNotBeNullAccessor(
            value => Localize("Validation_ValueMustNotBeNull", value));
        provider.SetMissingBindRequiredValueAccessor(
            field => Localize("Validation_MissingRequiredValue", field));
        provider.SetAttemptedValueIsInvalidAccessor(
            (value, field) => Localize("Validation_AttemptedValueIsInvalid", value, field));
        provider.SetNonPropertyAttemptedValueIsInvalidAccessor(
            value => Localize("Validation_ValueIsInvalid", value));
        provider.SetUnknownValueIsInvalidAccessor(
            field => Localize("Validation_ValueIsInvalid", field));
        provider.SetNonPropertyUnknownValueIsInvalidAccessor(
            () => Localize("Validation_ValueIsInvalid", string.Empty));
        provider.SetValueMustBeANumberAccessor(
            field => Localize("Validation_ValueMustBeANumber", field));
        provider.SetNonPropertyValueMustBeANumberAccessor(
            () => Localize("Validation_ValueMustBeANumber", string.Empty));
    })
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        // DataAnnotations のエラーメッセージも同じ共有リソースから解決する
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });

// 既定は日本語。英語はトグルで切り替える
var supportedCultures = new[] { new CultureInfo("ja"), new CultureInfo("en") };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("ja");
    options.SupportedCultures = supportedCultures;    // 日付・数値の書式
    options.SupportedUICultures = supportedCultures;  // 文字列リソース

    // ブラウザーの Accept-Language は参照しない。
    // 「サイトは日本語。切り替えはトグルで明示的に行う」という方針のため、
    // 英語ブラウザーの利用者にも既定では日本語を表示する。
    //
    // ブラウザーの言語設定を尊重したい場合は次の 1 行を削除する
    // （既定のプロバイダー順は QueryString > Cookie > Accept-Language）。
    var acceptLanguage = options.RequestCultureProviders
        .FirstOrDefault(p => p is AcceptLanguageHeaderRequestCultureProvider);
    if (acceptLanguage is not null)
    {
        options.RequestCultureProviders.Remove(acceptLanguage);
    }
});

// ---- データベース --------------------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
localizationServices = app.Services;

// ---- ミドルウェア --------------------------------------------------------
// ❗ 順序が重要: カルチャを必要とするミドルウェアより前に置くこと
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 起動時にマイグレーションを適用し、DB が無ければ自動生成する
// 注: 学習用の簡便な方法。実運用では配備手順として明示的に実行すること
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();

// モデルバインディングのメッセージを、その時点のリクエストのカルチャで解決する。
// 起動時に固定してしまうと言語切り替えに追従しないため、呼び出しごとに解決する。
string Localize(string key, params object[] args)
{
    var factory = localizationServices!.GetRequiredService<IStringLocalizerFactory>();
    var localizer = factory.Create(typeof(SharedResource));
    return localizer[key, args];
}

/// <summary>統合テストからエントリポイントを参照できるようにする</summary>
public partial class Program { }
