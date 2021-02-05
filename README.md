# HoloViewer

[![GitHub license](https://img.shields.io/github/license/kawa0x0A/HoloViewer)](https://github.com/kawa0x0A/HoloViewer/blob/main/LICENSE)
[![Github All Releases](https://img.shields.io/github/downloads/kawa0x0A/HoloViewer/total.svg)]()

![HoloViewer_Windows](https://user-images.githubusercontent.com/10515785/103134135-53fc3680-46f2-11eb-8d0d-c6e4d9d1dc82.png)

![HoloViewer_Mac](https://user-images.githubusercontent.com/10515785/103134186-ca993400-46f2-11eb-9603-35a61e424921.png)

* ソフトウェア情報
  * 名前
    * ホロビューワー
  * 開発者
    * kawa0x0A
  * 動作に必要な環境
    * Windows (64bit)
      * 『Microsoft Edge Canary チャネル』のインストールが必要です
    * Mac (macOS 10.9以上)
  * 機能
    * ブラウザを複数起動しなくても、Youtubeなどの動画ページを同時に見ることができます
  * 料金
    * 本ソフトウェアは無料で使用できます
    * 寄付歓迎です (寄付ページをブラウザで開く機能も付いています)
    * 寄付に関する情報は下記のQ&Aにあります
  * ライセンス
    * MITライセンス
  * 寄付ページ
    * https://holoviewer-public.web.app/
  * 利用規約
    * https://holoviewer-public.web.app/user-policy.html
  * 特定商取引法に基づく表示
    * https://holoviewer-public.web.app/specific-trade-law.html
  * 寄付手続きに関するプライバシーポリシー
    * https://holoviewer-public.web.app/privacy.html

* ダウンロード
  * Windows版
    * https://github.com/kawa0x0A/HoloViewer/releases/download/1.0.7.0/HoloViewer_Windows_x64.zip
    * Windows版の使用には[Microsoft Edge Canary チャネル](https://www.microsoftedgeinsider.com/ja-jp/download)が必要になります。
  * Mac版
    * https://github.com/kawa0x0A/HoloViewer/releases/download/1.0.7.0/HoloViewer_macOS.pkg
  * 過去のバージョンは[こちら](https://github.com/kawa0x0A/HoloViewer/releases)からダウンロードしてください

* 連絡先
  * メール holoviewer.contact@gmail.com
  * Twitter https://twitter.com/kawa0x0A

* 技術情報
  * 使用言語とフレームワーク
    * C# + .NET 5.0 + Mobile Blazor Bindings
  * 決済
    * Stripe
  * ホスティング
    * Firebase Hosting

* 最新版での既知の不具合
  * [不具合情報一覧](https://docs.google.com/spreadsheets/d/1YBC7Tw2uv1DNkixUyVOATebgyF0IwTOW15bDXMFW1RI/edit?usp=sharing) (Googleスプレッドシートが開きます)

* Q&A
  * ソフトウェアについて
    * Q. Linux版はある?
      * A. Linuxには非対応です
    * キャプチャした画像はどこに保存される? (スクリーンショットの保存場所の設定を変更していない場合)
      * Windows版は、HoloViewer.Windows.exeファイルがあるディレクトリの「Capture」という名前のディレクトリの中にキャプチャした画面を画像ファイルとして保存します。
      * Mac版は、デスクトップの「Capture」ディレクトリを作成し、その中にキャプチャした画面を画像ファイルとして保存します。
    * Captureディレクトリが無いよ?
      * Captureディレクトリはキャプチャボタンを押した際に作成されます。
    * キャプチャした画像の保存形式は何?
      * キャプチャした画像はPNG形式で保存します。
    * Q. 「こんな機能が欲しい」とか思ったらどうすればいい?
    * Q. 不具合を見つけたらどうすればいい?
      * A. ご要望や不具合の報告などは、メールやTwitterで開発者にご連絡ください (できる限り対応しますが、対応できない場合があります)
  * 寄付について
    * Q. どんな方法で寄付ができる?
      * A. クレジットカードを使用して毎月ごとに一定金額の寄付を行うことができます
    * Q. 寄付するのは何円でもいいの?
      * A. 毎月「100円」「500円」「1000円」「5000円」の支払い金額を選択できます
    * Q. 寄付するとカード情報を抜き取ったりしない?
      * A. 寄付の際に入力するカード情報は決済代行サイトを通じて入力するため、開発者がカード情報を手に入れたり、カード情報を見ることはできません
    * Q. 寄付すると何か得する?
    * Q. 寄付しないと機能が制限される?
      * A. 寄付をしても機能が増えたりしませんし、寄付をしなくても機能は一切制限されません
    * Q. 毎月の寄付じゃなくて1回だけの寄付は無いの?
      * A. 1回だけの寄付には未対応なので、お手数ですがお支払い後にサブスクリプションを解約してください
    * Q. 支払いのキャンセルなどの問い合わせがしたい
      * A. メールかTwitterで開発者までお問い合わせください
