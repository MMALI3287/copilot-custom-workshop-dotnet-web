# MeowWorld プロジェクト - Copilot カスタム指示

## 全般
- 日本語で回答すること

## コメント規約
- コード内のコメントは日本語で記述すること
- XML ドキュメントコメント（`///`）も日本語で記述すること

## C# コーディング規約
- file-scoped namespace（`namespace X;` 形式）を使用すること
- nullable reference types を考慮し、必要に応じて `string?` や `required` キーワードを使用すること
- プライマリコンストラクタが使える場面では積極的に使うこと

## ASP.NET Core / Entity Framework Core
- 非同期メソッド（async/await）を優先すること
- DbContext はコンストラクタインジェクションで受け取ること
- LINQ メソッド構文を優先すること

## 多言語対応（日英切り替え）
- UI 文字列は直接記述せず、`@L["キー名"]` でリソースから取得すること
- リソースキーは `Resources/SharedResource.ja.resx` と `SharedResource.en.resx` の両方に追加すること
- 猫の名前・説明・品種などのデータは翻訳対象外とすること
- 即時切り替えの対象にする要素には `data-i18n="キー名"` を付けること

## プロジェクト構成
- OS: Windows / macOS / Linux
- フレームワーク: .NET 8, ASP.NET Core MVC
- データベース: SQLite（Entity Framework Core 経由）
- テスト: xUnit + Moq
