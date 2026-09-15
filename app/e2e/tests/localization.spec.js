// @ts-check
const { test, expect } = require('./fixtures');

/**
 * innerText は CSS の text-transform を反映してしまうため、
 * 表示文字ではなく実際の内容を比較する。
 */
const textOf = (locator) => locator.first().evaluate((el) => el.textContent.trim());

test.describe('日英の切り替え', () => {
  test('英語ロケールのブラウザーでも既定は日本語', async ({ page }) => {
    await page.goto('/Cats');
    expect(await textOf(page.locator('th[data-i18n="Col_Name"]'))).toBe('名前');
    await expect(page.locator('html')).toHaveAttribute('lang', 'ja');
  });

  test('トグルはページを再読み込みせずに切り替える', async ({ page }) => {
    let loads = 0;
    page.on('load', () => loads++);

    await page.goto('/Cats');
    const before = loads;

    await page.click('button[value="en"]');
    await page.waitForFunction(() => document.documentElement.dataset.lang === 'en');

    expect(loads, 'ページ遷移が起きていないこと').toBe(before);
    expect(await textOf(page.locator('th[data-i18n="Col_Name"]'))).toBe('Name');
    expect(await textOf(page.locator('.nav__link[data-i18n="Nav_Collection"]'))).toBe('Collection');
    await expect(page.locator('html')).toHaveAttribute('lang', 'en');
  });

  test('ページタイトルも切り替わる', async ({ page }) => {
    await page.goto('/Cats');
    await page.click('button[value="en"]');
    await page.waitForFunction(() => document.documentElement.dataset.lang === 'en');
    await expect(page).toHaveTitle(/MeowWorld/);
    expect(await page.title()).not.toContain('猫管理システム');
  });

  test('切り替えの選択はクッキーで保持される', async ({ page }) => {
    await page.goto('/Cats');
    await page.click('button[value="en"]');
    await page.waitForFunction(() => document.documentElement.dataset.lang === 'en');

    await page.goto('/Cats');   // サーバー描画
    expect(await textOf(page.locator('th[data-i18n="Col_Name"]'))).toBe('Name');
  });

  test('往復して切り替えてもクッキーが古くならない', async ({ page }) => {
    await page.goto('/Cats');
    for (const c of ['en', 'ja', 'en']) {
      await page.click(`button[value="${c}"]`);
      await page.waitForFunction((v) => document.documentElement.dataset.lang === v, c);
    }
    await page.goto('/Cats');
    expect(await textOf(page.locator('th[data-i18n="Col_Name"]'))).toBe('Name');
  });

  test('猫の名前などのデータは翻訳しない', async ({ page }) => {
    await page.goto('/Cats?culture=en');
    const names = await page.locator('tbody tr td:nth-child(3)').allTextContents();
    expect(names.join('')).toMatch(/[ぁ-んァ-ン一-龯]/);
  });

  test('お気に入りボタンに読み上げ用の名前がある', async ({ page }) => {
    await page.goto('/Cats?culture=en');
    const label = page.locator('.fav .visually-hidden').first();
    expect(await textOf(label)).toMatch(/Favourites/);
  });

  test('言語切り替えに aria-label がある', async ({ page }) => {
    await page.goto('/Cats?culture=en');
    await expect(page.locator('form.js-lang-switcher')).toHaveAttribute('aria-label', /Display language/);
  });
});

test.describe('検証メッセージ', () => {
  test('サーバー側の検証も選択中の言語で返る', async ({ page }) => {
    await page.goto('/Cats/Create?culture=en');
    await page.evaluate(() => {
      const f = document.querySelector('.panel form');
      f.setAttribute('novalidate', '');
      f.querySelectorAll('[data-val="true"]').forEach((e) => e.removeAttribute('data-val'));
    });
    await page.fill('.panel form #Name', '');
    await page.click('.panel form button[type="submit"]');
    const errors = (await page.locator('.panel .text-danger').allTextContents()).join(' ');
    expect(errors.toLowerCase()).toContain('required');
  });
});
