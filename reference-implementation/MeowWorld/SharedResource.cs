namespace MeowWorld;

/// <summary>
/// 共有リソースの型マーカー。実装は持たない。
///
/// ❗ このクラスはプロジェクト直下（namespace MeowWorld）に置くこと。
/// Resources フォルダー内（namespace MeowWorld.Resources）に置くと、
/// ResourcesPath = "Resources" と二重になり
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
