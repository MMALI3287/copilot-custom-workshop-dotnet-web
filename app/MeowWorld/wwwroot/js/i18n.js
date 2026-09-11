/*
 * 表示言語の即時切り替え。
 *
 * 方針:
 *   - サーバーが正（.resx が唯一の情報源）。初回描画は常にサーバー側で正しい言語
 *   - トグル押下時は /Culture/Strings から辞書を取得し、DOM を直接書き換える
 *     ことでリロードなしに切り替える
 *   - 同じリクエストでクッキーも更新されるため、次回以降のサーバー描画も追従する
 *   - JS が無効なら form が通常どおり POST され、サーバー側で切り替わる
 *
 * 対象の印:
 *   data-i18n="Key"        -> textContent を差し替える
 *   data-i18n-alt="Key"    -> alt 属性を差し替える
 *   data-i18n-title="Key"  -> title 属性を差し替える
 *   data-i18n-value="Key"  -> value 属性を差し替える（ボタン用）
 */
(function () {
  'use strict';

  var cache = {};   // culture -> 辞書
  var inflight = null;

  var COOKIE = '.AspNetCore.Culture';

  /*
   * カルチャクッキーをクライアント側で書く。
   *
   * ❗ 辞書がキャッシュに載っていると fetch を行わないため、
   *    サーバー側の Set-Cookie に頼るとクッキーが古いままになる。
   *    その結果、次のページ遷移で前回の言語に戻ってしまう。
   *    そのため切り替えのたびに必ずここで書き込む。
   *
   * 形式は CookieRequestCultureProvider と同じ "c=xx|uic=xx"。
   */
  function writeCultureCookie(culture) {
    var value = encodeURIComponent('c=' + culture + '|uic=' + culture);
    var expires = new Date();
    expires.setFullYear(expires.getFullYear() + 1);
    document.cookie = COOKIE + '=' + value +
      ';expires=' + expires.toUTCString() +
      ';path=/;samesite=lax';
  }

  function fetchStrings(culture) {
    if (cache[culture]) {
      return Promise.resolve(cache[culture]);
    }
    return fetch('/Culture/Strings?culture=' + encodeURIComponent(culture), {
      headers: { 'Accept': 'application/json' },
      credentials: 'same-origin'
    })
      .then(function (res) {
        if (!res.ok) { throw new Error('HTTP ' + res.status); }
        return res.json();
      })
      .then(function (dict) {
        cache[culture] = dict;
        return dict;
      });
  }

  function applyAttr(dict, attr, apply) {
    document.querySelectorAll('[' + attr + ']').forEach(function (el) {
      var key = el.getAttribute(attr);
      if (Object.prototype.hasOwnProperty.call(dict, key)) {
        apply(el, dict[key]);
      }
    });
  }

  function apply(dict, culture) {
    writeCultureCookie(culture);   // 次のリクエスト以降もこの言語で描画させる

    applyAttr(dict, 'data-i18n', function (el, v) { el.textContent = v; });
    applyAttr(dict, 'data-i18n-alt', function (el, v) { el.setAttribute('alt', v); });
    applyAttr(dict, 'data-i18n-title', function (el, v) { el.setAttribute('title', v); });
    applyAttr(dict, 'data-i18n-value', function (el, v) { el.setAttribute('value', v); });
    applyAttr(dict, 'data-i18n-aria-label', function (el, v) { el.setAttribute('aria-label', v); });

    // ページタイトルはサーバー描画時にしか評価されないため、ここで組み直す
    var titleEl = document.querySelector('title[data-i18n-title-suffix]');
    if (titleEl) {
      var key = titleEl.getAttribute('data-i18n-title-key');
      var suffixKey = titleEl.getAttribute('data-i18n-title-suffix');
      var suffix = dict[suffixKey] || '';
      var head = key && dict[key] ? dict[key] : (titleEl.textContent.split('—')[0] || '').trim();
      document.title = suffix ? head + ' — ' + suffix : head;
    }

    var root = document.documentElement;
    root.setAttribute('lang', culture);
    root.setAttribute('data-lang', culture);

    // トグル自身の押下状態を更新する
    document.querySelectorAll('.js-lang-switcher button[name="culture"]').forEach(function (btn) {
      var isCurrent = btn.value === culture;
      btn.setAttribute('aria-pressed', isCurrent ? 'true' : 'false');
      btn.classList.toggle('btn-light', isCurrent);
      btn.classList.toggle('btn-outline-light', !isCurrent);
    });

    // クライアント検証メッセージはサーバー描画時の言語で data-val-* に焼かれている。
    // 言語を切り替えたら、次の送信でサーバーに判定させるため検証を作り直す。
    // （jQuery unobtrusive validation がある場合のみ）
    try {
      if (window.jQuery && window.jQuery.validator && window.jQuery.validator.unobtrusive) {
        window.jQuery('form').each(function () {
          var $f = window.jQuery(this);
          $f.removeData('validator').removeData('unobtrusiveValidation');
          window.jQuery.validator.unobtrusive.parse($f);
        });
      }
    } catch (e) {
      console.warn('検証の再バインドに失敗しました。', e);
    }

    document.dispatchEvent(new CustomEvent('languagechanged', { detail: { culture: culture } }));
  }

  function switchTo(culture, form) {
    if (inflight) { return; }
    inflight = fetchStrings(culture)
      .then(function (dict) { apply(dict, culture); })
      .catch(function (err) {
        // 取得に失敗したらサーバー側の経路にフォールバックする
        console.warn('即時切り替えに失敗したため、ページ遷移で切り替えます。', err);
        form.removeEventListener('submit', onSubmit, true);

        // ❗ form.submit() は押されたボタンの name/value を送らないため、
        //    culture を hidden input として明示的に足す。
        //    これが無いとフォールバック経路で言語が切り替わらない。
        var hidden = document.createElement('input');
        hidden.type = 'hidden';
        hidden.name = 'culture';
        hidden.value = culture;
        form.appendChild(hidden);

        form.submit();
      })
      .finally(function () { inflight = null; });
  }

  function onSubmit(e) {
    var form = e.currentTarget;
    var btn = e.submitter || form.querySelector('button[name="culture"]:focus');
    if (!btn || !btn.value) { return; }        // 判別できなければ通常送信に任せる

    var culture = btn.value;
    if (culture === document.documentElement.getAttribute('data-lang')) {
      e.preventDefault();                       // 同じ言語なら何もしない
      return;
    }

    e.preventDefault();
    switchTo(culture, form);
  }

  document.addEventListener('DOMContentLoaded', function () {
    // fetch が無い古い環境ではサーバー側の経路をそのまま使う
    if (typeof window.fetch !== 'function') { return; }

    document.querySelectorAll('form.js-lang-switcher').forEach(function (form) {
      form.addEventListener('submit', onSubmit, true);
    });
  });
})();
