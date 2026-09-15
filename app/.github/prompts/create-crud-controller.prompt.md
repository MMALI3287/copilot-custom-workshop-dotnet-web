---
description: "Entity に対する CRUD コントローラーを生成する"
---
# CRUD コントローラー生成

以下の Entity に対して、ASP.NET Core MVC の CRUD コントローラーを作成してください。

## 対象 Entity
{{input}}

## 要件
- AppDbContext を DI で受け取る
- DB アクセスを伴うアクションは async/await で実装する（GET のフォーム表示など I/O の無いものは同期で良い）
- Index（一覧）、Details（詳細）、Create（作成 GET/POST）、Edit（編集 GET/POST）、Delete（削除 GET/POST）を実装する
- POST アクションでは ModelState.IsValid を検証する
- POST アクションには `[ValidateAntiForgeryToken]` を付ける
- 保存の失敗が起こりうるアクション（Create/Edit の POST）は try-catch で囲み、エラー時はログ出力する
- 成功時は TempData でフラッシュメッセージを設定する（メッセージはリソースから取得）
