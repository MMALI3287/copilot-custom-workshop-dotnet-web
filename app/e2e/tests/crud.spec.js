// @ts-check
const { test, expect } = require('./fixtures');

const textOf = (locator) => locator.first().evaluate((el) => el.textContent.trim());
const FORM = '.panel form';

test.describe('猫の CRUD', () => {
  test('一覧にシードデータが並ぶ', async ({ page }) => {
    await page.goto('/Cats');
    expect(await page.locator('tbody tr').count()).toBeGreaterThanOrEqual(1);
  });

  test('登録・編集・削除が一通り動く', async ({ page }) => {
    const unique = `テスト猫${Date.now()}`;

    // 登録
    await page.goto('/Cats/Create');
    await page.fill(`${FORM} #Name`, unique);
    await page.fill(`${FORM} #Age`, '2');
    await page.fill(`${FORM} #Breed`, 'ベンガル');
    await page.click(`${FORM} button[type="submit"]`);
    await expect(page.locator('.flash')).toBeVisible();
    await expect(page.locator('tbody')).toContainText(unique);

    // 編集
    const row = page.locator('tbody tr', { hasText: unique });
    await row.locator('a[data-i18n="Action_Edit"]').click();
    await page.fill(`${FORM} #Name`, `${unique}-edited`);
    await page.click(`${FORM} button[type="submit"]`);
    await expect(page.locator('tbody')).toContainText(`${unique}-edited`);

    // 削除
    const edited = page.locator('tbody tr', { hasText: `${unique}-edited` });
    await edited.locator('a[data-i18n="Action_Delete"]').click();
    // 削除確認画面は .panel で囲んでいないため、ボタンを直接指す
    await page.click('button[data-i18n="Action_ConfirmDelete"]');
    await expect(page.locator('tbody')).not.toContainText(`${unique}-edited`);
  });

  test('編集しても CreatedAt は書き換えられない', async ({ page, request }) => {
    // 詳細画面に出ている登録日時を控える
    await page.goto('/Cats/Details/1?culture=en');
    const before = await textOf(page.locator('.record__row').last().locator('.record__v'));

    // フォームから編集する（CreatedAt は送られない）
    await page.goto('/Cats/Edit/1');
    await page.fill(`${FORM} #Age`, '9');
    await page.click(`${FORM} button[type="submit"]`);

    await page.goto('/Cats/Details/1?culture=en');
    const after = await textOf(page.locator('.record__row').last().locator('.record__v'));
    expect(after, '登録日時はサーバー所有で不変であること').toBe(before);
  });

  test('お気に入りを切り替えられる', async ({ page }) => {
    await page.goto('/Cats');
    const fav = page.locator('.fav').first();
    const before = await fav.getAttribute('data-on');
    await fav.click();
    expect(await page.locator('.fav').first().getAttribute('data-on')).not.toBe(before);
  });

  test('存在しない ID は 404 を返す', async ({ request }) => {
    expect((await request.get('/Cats/Details/999999')).status()).toBe(404);
  });
});

test.describe('言語切り替えの安全性', () => {
  test('外部 URL への returnUrl はトップに落とす', async ({ page }) => {
    await page.goto('/Cats');

    // ❗ クラスを外しても、既に登録済みのリスナーは外れない。
    //    サーバー経路を確実に通すため、トークンを写した新しいフォームを組んで送る。
    await page.evaluate(() => {
      const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
      const f = document.createElement('form');
      f.method = 'post';
      f.action = '/Culture/Set';
      f.innerHTML =
        `<input name="__RequestVerificationToken" value="${token}">` +
        `<input name="culture" value="en">` +
        `<input name="returnUrl" value="https://example.com/evil">`;
      document.body.appendChild(f);
      f.submit();
    });

    // 現在の URL に一致するパターンで待つと即座に解決してしまうため、
    // 「/Cats から離れたこと」を条件にする
    await page.waitForURL((url) => new URL(url).pathname !== '/Cats');
    expect(page.url(), 'オープンリダイレクトにならないこと').not.toContain('example.com');
    expect(new URL(page.url()).pathname, '無効な returnUrl はトップへ落とす').toBe('/');
  });
});
