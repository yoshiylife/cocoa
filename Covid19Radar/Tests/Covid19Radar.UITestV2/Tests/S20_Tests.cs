﻿/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace CovidRadar.UITestV2
{
    /// <summary>
    /// S-20　利用規約の確認シナリオ.
    /// </summary>
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    [Category("ja-JP")]
    [Category("en-US")]
    [Category("zh-CN")]
    [Category("ko-KR")]
    public class S20_Tests : BaseTestFixture
    {
        /// <summary>
        /// コンストラクタ.
        /// </summary>
        /// <param name="platform">動作OS.</param>
        public S20_Tests(Platform platform)
            : base(platform)
        {
        }

        /// <inheritdoc/>
        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            AppManager.StartApp();
        }

        /// <summary>
        /// 利用規約の確認
        /// 本テストは端末の設定言語に応じて、対応ケースが変わる
        /// 日本語:Case1に相当
        /// 英語:Case2に相当
        /// 中国語:Case3に相当
        /// 韓国語:Case4に相当.
        /// </summary>
        [Test]
        public void Case01_Test()
        {
            /* S1
             * 【iOS】
             *  OSの設定 > 一般 > 言語と地域
             * 「編集」ボタンを押下し、「使用する言語の優先順序」内の項目を[前提]の言語以外を削除する
             */

            // 端末言語取得
            var cultureText = AppManager.GetCurrentCultureBackDoor();
            if (cultureText != "ja-JP")
            {
                //AppManager.StartApp();
                TutorialPageFlow tutorialPageFlow = new TutorialPageFlow();
                tutorialPageFlow.Tutorial();
            }

            AppManager.DismissSpringboardAlerts();

            HomePage homePage = new HomePage();
            homePage.AssertHomePage();

            // S2 ホーム画面で、ハンバーガーメニューを押下
            MenuPage menuPage = homePage.OpenMenuPage();
            menuPage.AssertMenuPage();

            // S3 ハンバーガーメニューで、「設定」ボタンを押下
            SettingsPage settingsPage = menuPage.OpenSettingsPage();
            settingsPage.AssertSettingsPage();

            // S3 設定ページで、「利用規約」ボタンを押下
            TermsofservicePage termsofservicePage = settingsPage.OpenTermsofservicePage();
            termsofservicePage.AssertTermsofservicePage();

            // 比較単語を取得
            string comparisonText = (string)AppManager.Comparison(cultureText, "termofusehtml");

            // タイトル取得
            var message = AppManager.GetTitleText();

            // タイトルのテキストと期待する文言を比較
            Assert.AreEqual(message, comparisonText);
        }
    }
}
