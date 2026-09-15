// @ts-check
const base = require('@playwright/test');

/**
 * 外部ホストへのリクエストを遮断したテスト用 page。
 *
 * webfont は Google Fonts から読み込まれるが、CI やオフライン環境では
 * この要求が滞留し `load` イベントが発火しないままタイムアウトする。
 * アプリはフォールバック書体で正しく動作するため、テストでは遮断する。
 * こうすることでテストは速く、外部ネットワークから独立する。
 */
const test = base.test.extend({
  page: async ({ page }, use) => {
    await page.route(/^https?:\/\/(fonts\.googleapis\.com|fonts\.gstatic\.com)/, (route) =>
      route.abort());
    await use(page);
  },
});

module.exports = { test, expect: base.expect };
