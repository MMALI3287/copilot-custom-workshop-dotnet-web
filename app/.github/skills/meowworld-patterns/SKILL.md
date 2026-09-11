---
description: "MeowWorld プロジェクトのアーキテクチャパターン、ファイル配置規約、命名規則、多言語リソースの追加手順、新機能追加手順を提供する。新しいコントローラー・モデル・ビュー・テスト・UI 文字列の追加時に参照する。"
---
# MeowWorld アーキテクチャパターン

## プロジェクト構成

```text
MeowWorld/
├── Controllers/       ← コントローラー（{Entity}Controller.cs）
├── Models/            ← エンティティ・ビューモデル
├── Data/              ← DbContext・マイグレーション
├── Resources/         ← 多言語リソース（.resx）
├── Views/
│   ├── {Controller}/  ← コントローラー対応ビュー
│   └── Shared/        ← レイアウト・パーシャル
├── wwwroot/           ← 静的ファイル
└── SharedResource.cs  ← リソースの型マーカー（プロジェクト直下に置く）
```

## 技術スタック
- .NET 8 / ASP.NET Core MVC
- Entity Framework Core + SQLite
- xUnit + Moq（テスト）
- 手書きの `site.css` によるエディトリアル UI

## 命名規約
- エンティティ: PascalCase 単数形（`Cat`, `Favorite`）
- コントローラー: `{Entity}Controller`
- ビューフォルダ: コントローラー名から `Controller` を除いた名前
- テストクラス: `{Controller名}Tests`
- テストメソッド: 日本語（「猫一覧が正しく取得できること」）
- リソースキー: `{接頭辞}_{意味}`（`Nav_CatList`、`Col_Name`、`Action_Edit`）

## 新機能追加パターン

### 1. 新エンティティの追加
1. `Models/{Entity}.cs` を作成
2. `Data/AppDbContext.cs` に `DbSet<{Entity}>` を追加
3. マイグレーション作成・適用
4. `Controllers/{Entity}Controller.cs` に CRUD 実装
5. `Views/{Entity}/` に Index, Details, Create, Edit, Delete ビュー作成
6. UI 文字列を日英リソースに追加（下記）
7. `MeowWorld.Tests/Controllers/{Entity}ControllerTests.cs` にテスト

### 2. UI 文字列の追加（必須手順）
1. `Resources/SharedResource.ja.resx` にキーと日本語値を追加
2. `Resources/SharedResource.en.resx` に**同じキー**と英語値を追加
3. ビューでは `@L["キー名"]` で参照する
4. 即時切り替え対象なら `data-i18n="キー名"` を付ける
5. `dotnet test` を実行する（日英のキー差分は LocalizationTests が検出する）

### 3. 既存エンティティへのリレーション追加
1. モデルにナビゲーションプロパティ追加
2. `OnModelCreating` でリレーション設定
3. マイグレーション作成・適用
4. 関連コントローラーにアクション追加

## コーディング規約
- file-scoped namespace
- 非同期メソッド（async/await）
- プライマリコンストラクタ（DI 用）
- 日本語 XML ドキュメントコメント
- `required` / nullable 型の適切な使用
- 読み取り専用のクエリには `AsNoTracking()` を付ける

## 多言語対応の落とし穴

| 症状 | 原因 | 対処 |
|------|------|------|
| `MissingManifestResourceException` | マーカー型を `namespace MeowWorld.Resources;` と宣言した（`ResourcesPath` と二重になる） | `namespace MeowWorld;` で宣言する（本リポジトリではプロジェクト直下に配置） |
| ラベルがキー名のまま表示される | そのキーが該当言語の resx に無い | 両方の resx に追加する |
| トグルは効くが再読込で戻る | クッキーが更新されていない | `wwwroot/js/i18n.js` が毎回クッキーを書いているか確認 |
| 英語表示なのに一部が日本語 | モデルバインダー既定メッセージ | `Program.cs` の `ModelBindingMessageProvider` を確認 |

## 翻訳の境界
- **翻訳する**: ナビゲーション、見出し、ボタン、テーブル見出し、検証メッセージ、alt 属性
- **翻訳しない**: 猫の名前、説明、品種などの利用者データ
