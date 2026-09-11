# String Catalog

Every user-facing string in the MeowWorld UI, with a resource key and its Japanese and English values.

Japanese is the source language; the workshop's Custom Instructions produce it, and it is what appears in the wireframe (`Designer.png`) and the finished screenshot (`cats-index.png`). English is the added translation.

**How to use this:** copy the Japanese column into `Resources/SharedResource.ja.resx` and the English column into `Resources/SharedResource.en.resx`, keyed by the Key column. Setup instructions are in [README.md](README.md).

---

## Naming convention for keys

| Prefix | Used for | Example |
|--------|----------|---------|
| `App_` | Application-level identity | `App_Title` |
| `Nav_` | Sidebar and navigation links | `Nav_CatList` |
| `Page_` | Page titles and headings | `Page_CatList_Title` |
| `Col_` | Table column headers and field labels | `Col_Name` |
| `Action_` | Buttons and links that do something | `Action_Edit` |
| `Msg_` | Informational, confirmation and empty-state messages | `Msg_NoCats` |
| `Validation_` | DataAnnotations error messages | `Validation_Name_Required` |
| `Logo_` | Image alt text | `Logo_Alt` |

Keys are `PascalCase` after the prefix and describe **what the string is**, never what it says. `Action_Delete` survives a copy change from 削除 to 消去; `Action_Sakujo` would not.

---

## Application identity

| Key | 日本語 | English | Notes |
|-----|--------|---------|-------|
| `App_Name` | MeowWorld | MeowWorld | A brand name. Identical in both; keep the key so it stays centralised |
| `App_Tagline` | 猫管理システム | Cat Management System | |
| `App_Title` | MeowWorld 猫管理システム | MeowWorld Cat Management System | The navbar title. Consider composing from the two keys above rather than duplicating |
| `Logo_Alt` | MeowWorld 猫管理システムのロゴ | MeowWorld Cat Management System logo | Alt text for `logo.png` |

---

## Sidebar navigation

Sourced from `Designer.png`. The finished screenshot drops the last two.

| Key | 日本語 | Romanisation | English | In the screenshot? |
|-----|--------|--------------|---------|--------------------|
| `Nav_Dashboard` | ダッシュボード | dasshubōdo | Dashboard | Yes |
| `Nav_CatList` | 猫一覧 | neko ichiran | Cat List | Yes |
| `Nav_RegisterCat` | 新しい猫を登録 | atarashii neko o tōroku | Register a New Cat | Yes |
| `Nav_Categories` | カテゴリ | kategori | Categories | Yes |
| `Nav_HealthRecords` | 健康記録 | kenkō kiroku | Health Records | Yes |
| `Nav_Adoption` | 里親募集 | satooya boshū | Adoption | Yes |
| `Nav_Schedule` | スケジュール | sukejūru | Schedule | Yes |
| `Nav_Reports` | レポート | repōto | Reports | Yes |
| `Nav_UserManagement` | ユーザー管理 | yūzā kanri | User Management | No, wireframe only |
| `Nav_Settings` | 設定 | settei | Settings | No, wireframe only |

> 里親募集 is literally "foster parent recruitment". "Adoption" is the natural English equivalent in an animal-shelter context and is what English-language rescue sites use. "Foster Care" would be a mistranslation, since it implies temporary placement.

---

## Page titles

| Key | 日本語 | English |
|-----|--------|---------|
| `Page_CatList_Title` | 猫一覧 | Cat List |
| `Page_CatDetails_Title` | 猫の詳細 | Cat Details |
| `Page_CatCreate_Title` | 新しい猫を登録 | Register a New Cat |
| `Page_CatEdit_Title` | 猫の情報を編集 | Edit Cat |
| `Page_CatDelete_Title` | 猫の削除 | Delete Cat |
| `Page_Home_Title` | ホーム | Home |
| `Page_Privacy_Title` | プライバシー | Privacy |

---

## Table columns and field labels

The same keys serve both the table headers and the form labels, via `[Display(Name = "...")]`.

| Key | 日本語 | Romanisation | English | Maps to |
|-----|--------|--------------|---------|---------|
| `Col_Id` | ID | - | ID | `Cat.Id` |
| `Col_Name` | 名前 | namae | Name | `Cat.Name` |
| `Col_Age` | 年齢 | nenrei | Age | `Cat.Age` |
| `Col_Breed` | 品種 | hinshu | Breed | `Cat.Breed` |
| `Col_Description` | 説明 | setsumei | Description | `Cat.Description` |
| `Col_CreatedAt` | 登録日時 | tōroku nichiji | Registered | `Cat.CreatedAt` |
| `Col_Actions` | 操作 | sōsa | Actions | (the button column) |
| `Col_IsFavorite` | お気に入り | okiniiri | Favourite | `Cat.IsFavorite` (added in Step 9) |

> The wireframe writes the first four headers in English (ID, Name, Age, Breed) and only 操作 in Japanese. The finished implementation uses Japanese throughout. The Japanese column above follows the implementation, which is the correct baseline.

---

## Actions

| Key | 日本語 | Romanisation | English | Style in the screenshot |
|-----|--------|--------------|---------|------------------------|
| `Action_Details` | 詳細 | shōsai | Details | `btn-info` (cyan) |
| `Action_Edit` | 編集 | henshū | Edit | `btn-warning` (amber) |
| `Action_Delete` | 削除 | sakujo | Delete | `btn-danger` (red) |
| `Action_CreateCat` | 新しい猫を登録 | atarashii neko o tōroku | Register a New Cat | `btn-primary` (blue) |
| `Action_Save` | 保存 | hozon | Save | |
| `Action_Cancel` | キャンセル | kyanseru | Cancel | |
| `Action_BackToList` | 一覧に戻る | ichiran ni modoru | Back to List | |
| `Action_ConfirmDelete` | 削除する | sakujo suru | Delete | On the delete confirmation page |
| `Action_AddFavorite` | お気に入りに追加 | okiniiri ni tsuika | Add to Favourites | Step 9 |
| `Action_RemoveFavorite` | お気に入りから削除 | okiniiri kara sakujo | Remove from Favourites | Step 9 |

> `Action_CreateCat` and `Nav_RegisterCat` hold the same text. Keep them as separate keys: the sidebar link and the page button are different UI elements and may diverge later. Duplicated *values* are cheap; a shared key you have to split later is not.

---

## Messages

| Key | 日本語 | English |
|-----|--------|---------|
| `Msg_NoCats` | 登録されている猫はいません。 | No cats have been registered. |
| `Msg_DeleteConfirm` | この猫を削除してもよろしいですか？ | Are you sure you want to delete this cat? |
| `Msg_DeleteIrreversible` | この操作は取り消せません。 | This action cannot be undone. |
| `Msg_CreateSuccess` | 猫を登録しました。 | The cat has been registered. |
| `Msg_UpdateSuccess` | 猫の情報を更新しました。 | The cat's information has been updated. |
| `Msg_DeleteSuccess` | 猫を削除しました。 | The cat has been deleted. |
| `Msg_NotFound` | 指定された猫が見つかりません。 | The specified cat was not found. |

> Japanese ends sentences with the full-width `。` and uses the full-width `？`. Do not copy these into the English values; use `.` and `?`. A mixed-punctuation English string is a reliable sign the value was pasted from the wrong column.

---

## Validation messages

Put the **key** in the attribute, not the message. See [README.md](README.md) Step 7.

| Key | 日本語 | English | Attribute |
|-----|--------|---------|-----------|
| `Validation_Name_Required` | 名前は必須です。 | The name is required. | `[Required]` on `Name` |
| `Validation_Name_MaxLength` | 名前は50文字以内で入力してください。 | The name must be 50 characters or fewer. | `[MaxLength(50)]` |
| `Validation_Age_Range` | 年齢は0以上30以下で入力してください。 | The age must be between 0 and 30. | `[Range(0, 30)]` |
| `Validation_Breed_Required` | 品種は必須です。 | The breed is required. | `[Required]` on `Breed` |
| `Validation_Breed_MaxLength` | 品種は50文字以内で入力してください。 | The breed must be 50 characters or fewer. | `[MaxLength(50)]` |
| `Validation_Description_MaxLength` | 説明は500文字以内で入力してください。 | The description must be 500 characters or fewer. | `[MaxLength(500)]` |

---

## Language switcher

| Key | 日本語 | English | Notes |
|-----|--------|---------|-------|
| `Lang_Japanese` | 日本語 | 日本語 | **Never translate.** Always shown in its own language |
| `Lang_English` | English | English | **Never translate** |
| `Lang_SwitcherLabel` | 表示言語 | Display language | The `aria-label` on the button group |

> The two language names being identical across both files is the correct result, not an oversight. A reader looking for Japanese scans for 日本語; rendering it as "Japanese" when the UI happens to be in English makes the control harder to use for exactly the person who needs it.

---

## Not translatable: data

Listed here so nobody adds keys for them by mistake.

| Value | Why not |
|-------|---------|
| Cat names (みけ, くろ, しろ, チャチャ, ソラ) | User data. Romanisation is not translation, and there is no correct English |
| Cat descriptions (おとなしい性格 and so on) | Free-text user data |
| `CreatedAt` values | Data. **Format** it by culture via `SupportedCultures`; do not translate it |

Breeds (三毛猫, 黒猫, 白猫, 茶トラ, ロシアンブルー) are the borderline case. They are a closed vocabulary with real English equivalents, so translating them genuinely helps. If you want it, use a lookup table rather than a `.resx`, since breeds are data that happens to be translatable rather than UI chrome. See "Translating data, not just labels" in [README.md](README.md).

For reference, the mapping if you build that table:

| 日本語 | English | Appears in |
|--------|---------|------------|
| 三毛猫 | Calico | Seed data, wireframe |
| 黒猫 | Black cat | Seed data, wireframe |
| 白猫 | White cat | Seed data |
| 茶トラ | Orange tabby | Seed data |
| ロシアンブルー | Russian Blue | Seed data |
| アメリカンショートヘア | American Shorthair | Wireframe |
| マンチカン | Munchkin | Wireframe |
| スコティッシュフォールド | Scottish Fold | Wireframe |
| ラグドール | Ragdoll | Wireframe |
| ベンガル | Bengal | Wireframe |
| ブリティッシュショートヘア | British Shorthair | Wireframe |
| 雑種 | Mixed breed | Wireframe (as a parenthetical) |

---

## Workshop terminology

Not UI strings. This is for reading the Japanese documentation and the Japanese Copilot prompts.

| 日本語 | English |
|--------|---------|
| ワークショップ | workshop |
| ハンズオン | hands-on |
| 手順 | procedure, steps |
| 受講者 | participant, attendee |
| 講師 | instructor |
| 演習 | exercise |
| 確認ポイント | what to check |
| 学びのポイント | learning point |
| ふりかえり | retrospective |
| 期待される成果物 | expected deliverables |
| 前提条件 | prerequisites |
| 補足 | supplementary note |
| 規約 | convention |
| 命名規約 | naming convention |
| コーディング規約 | coding convention |
| 完了条件 | completion criteria, Definition of Done |
| 手戻り | rework |
| 再現性 | reproducibility |
| 説明責任 | accountability |
| ガードレール | guardrail |
| 一覧 | list |
| 詳細 | details |
| 編集 | edit |
| 削除 | delete |
| 作成 | create |
| 登録 | register |
| 設定 | settings, configuration |
| 検証 | verification |
| 修正 | fix, correction |
| 実装 | implementation |
| 動作確認 | verifying the behaviour |
| 既存 | existing |
| 正常系 / 異常系 | happy path / error case |
| 初期データ | seed data, initial data |
| 接続文字列 | connection string |
| 自律的 | autonomous |
| 忖度 | reading between the lines, deferring to unstated expectations |
