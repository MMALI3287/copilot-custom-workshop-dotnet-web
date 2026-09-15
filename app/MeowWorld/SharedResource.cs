namespace MeowWorld;

/// <summary>
/// 共有リソースの型マーカー。実装は持たない。
///
/// ❗ 重要なのはファイルの置き場所ではなく、宣言する namespace である。
/// C# は namespace をフォルダーから自動決定しないが、IDE のテンプレートは
/// フォルダー名から補完するため、Resources/ に作ると
/// `namespace MeowWorld.Resources;` になりやすい。
/// その宣言のまま ResourcesPath = "Resources" を使うと二重パスになり、
/// "MeowWorld.Resources.Resources.SharedResource.ja.resources" を探して
/// MissingManifestResourceException になる。
///
/// 正しい解決経路:
///   型          : MeowWorld.SharedResource
///   ResourcesPath: "Resources"
///   実ファイル   : Resources/SharedResource.ja.resx
///   埋め込み名   : MeowWorld.Resources.SharedResource.ja.resources
/// </summary>
public class SharedResource
{
}
