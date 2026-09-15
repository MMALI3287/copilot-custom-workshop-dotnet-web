// @ts-check
const { defineConfig, devices } = require('@playwright/test');

/**
 * アプリは Playwright が起動する。
 * 事前に `dotnet run` しておく必要はない。
 */
module.exports = defineConfig({
  testDir: './tests',
  fullyParallel: false,        // CRUD がひとつの DB を共有するため直列に流す
  workers: 1,
  reporter: [['list'], ['html', { open: 'never' }]],
  timeout: 30_000,

  use: {
    baseURL: process.env.BASE_URL || 'http://127.0.0.1:5244',
    locale: 'en-US',           // 既定が日本語であることを検証するため、あえて英語ロケール
    trace: 'retain-on-failure',
  },

  projects: [
    {
      name: 'chromium',
      use: {
        ...devices['Desktop Chrome'],
        // 通常は `npx playwright install chromium` で入るブラウザーを使う。
        // CI やコンテナなど、すでに Chromium がある環境では
        // CHROMIUM_PATH でその実行ファイルを指定できる。
        launchOptions: process.env.CHROMIUM_PATH
          ? { executablePath: process.env.CHROMIUM_PATH }
          : {},
      },
    },
  ],

  webServer: process.env.BASE_URL ? undefined : {
    command: 'dotnet run --project ../MeowWorld --no-launch-profile --urls http://127.0.0.1:5244',
    url: 'http://127.0.0.1:5244',
    reuseExistingServer: !process.env.CI,
    timeout: 120_000,
  },
});
