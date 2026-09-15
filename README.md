# カスタム GitHub Copilot ワークショップ（ASP.NET Core & SQLite スタック）

🌐 **言語:** **日本語** | [English](README_EN.md)

**GitHub エキスパートサービスチーム**がご用意したカスタム Copilot ワークショップへようこそ！

このワークショップでは、GitHub Copilot の **Agent モード**・**Custom Instructions**・**Custom Agent** を中心に、ASP.NET Core MVC + SQLite の Web アプリケーション「MeowWorld」を構築しながら、Copilot の最新機能を実践的に学びます。

> **Note:** Visual Studio 2026 + .NET 10（LTS）をメイン環境としています。VS Code（C# Dev Kit）でも実施可能です。Visual Studio 2022 をご利用の場合は .NET 8（LTS）での実施も可能です。詳細は [Step 2](docs/2_BeforeGettingStarted/README_JA.md) をご覧ください。

## 目次

| Step | タイトル | 主な学習内容 |
|------|---------|-------------|
| 1 | [Mona の夢を叶えるストーリー](docs/1_Story/README_JA.md) | ワークショップの背景ストーリー |
| 2 | [はじめにお読みください](docs/2_BeforeGettingStarted/README_JA.md) | 環境構築・モード解説・前提条件 |
| 3 | [プロジェクト作成（Agent モード）](docs/3_CreateProject/README_JA.md) | Agent モードで一気にプロジェクト構築 |
| 4 | [Custom Instructions](docs/4_CustomInstructions/README_JA.md) | 3 層の指示設定（リポジトリ / ファイルスコープ / .prompt.md） |
| 5 | [DB レイヤー実装](docs/5_ImplementDBLayer/README_JA.md) | EF Core + SQLite を Agent が自律的に実装 |
| 6 | [MVC 実装 + Vision](docs/6_ImplementMVC/README_JA.md) | .prompt.md 活用 + 画像から UI 生成 |
| 7 | [Token 節約とコンテキスト管理](docs/7_TokenManagement/README_JA.md) | #file・#codebase・#terminalLastCommand の使い分け |
| 8 | [ユニットテスト](docs/8_UnitTesting/README_JA.md) | /tests コマンド + Agent によるテスト自動生成 |
| 9 | [Custom Agent & Skill](docs/9_CustomAgent/README_JA.md) | `.agent.md` + `SKILL.md` で専門エージェント構築 |
| 10 | [まとめとふりかえり](docs/10_LessonsLearned/README_JA.md) | ベストプラクティス・次のステップ |

**付録:**

| 付録 | 内容 |
|------|------|
| [トラブルシューティングガイド](docs/TroubleshootingGuide/README_JA.md) | Copilot・環境まわりのよくある問題と対処 |
| [画像の詳細分析](docs/ImageAnalysis/README.md) | 教材で使用する全画像のテキスト仕様書（英語） |
| [日英切り替え（JA/EN トグル）](docs/Localization/README.md) | MeowWorld に言語切り替えを追加する手順（英語） |
| [文字列カタログ](docs/Localization/string-catalog.md) | UI 文字列のリソースキーと日英対訳表 |
| [リファレンス実装](app/README.md) | 日英トグルを含む MeowWorld の完成版（動作確認済み・英語） |

> **⚠️ ワークショップを手順どおり行う場合:** このフォークの `app/` には完成済みの MeowWorld が
> 入っています。Step 3 は空の `app/` に `mkdir app` して進める手順のため、実施前に `app/` を
> 退避または削除して空にしてください。完成版を読むだけならそのままで構いません。

> **Note:** 本ワークショップの英語版は [README_EN.md](README_EN.md) にあります。各ステップの英訳は同じフォルダーの `README_EN.md` です。
