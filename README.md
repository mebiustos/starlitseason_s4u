# StarlitSeason S4U Auto Control Macro

This project provides a KeyToKey macro for automating various actions in the Starlit Season S4U mode.

## 概要
このプロジェクトは、Starlit SeasonのS4Uモードでのアクションを自動化するためのKeyToKeyマクロです。

カメラワークと演出の自動化を目的としています。

S4Uでの手動操作を簡易的に記録する記録モードと、指定した操作をミリ秒単位で再現する再生モードを提供します。
また、４K環境限定になりますが、カスタムスロット設定を保存・復元する機能も提供します。

![概要-img1](https://imgur.com/nhePSDP.jpg)

## 利用前の下準備1

1. KeyToKeyをインストールして起動します。

2. 「プロファイルの操作」の「新規作成」を選択し、「アイドルマスター スターリットシーズン」という名前のプロファイルを作成します。

   ![2-img1](https://imgur.com/TJAKFsI.jpg)
   
   ![2-img2](https://imgur.com/nv3Gu4V.jpg)
   
   ![2-img3](https://imgur.com/VRn5LUg.jpg)

3. 「プロファイルの操作」の「設定編集」をクリック。

   ![3-img1](https://imgur.com/MAZFCjg.jpg)

4. 画面左メニューから、1つ目の歯車マーク（アプリケーションの設定）をクリックし、「仮想XInputコントローラー」の「使用する仮想コントローラーの数」を「1」に設定。

   ![4-img1](https://imgur.com/OP6zMA5.jpg)

5. 画面左メニューから、2つめの歯車マーク（環境設定）をクリックし、「特定アプリでのみ実行を許可」の「以下のアプリで実行を許可する」にてプロセス名＋完全一致にてプロセス名「StarlitSeason-Win64-Shipping」を登録する。

   ![5-img1](https://imgur.com/yjf2EdE.jpg)

6. Ctrl+Sを押して、ここまでの設定を一度保存しておきましょう。

7. 画面左メニューから、マクロの作成／設定を選択。

   ![7-img1](https://imgur.com/OgwfaCv.jpg)

8. タブ「C#スクリプト（マクロ）」にて以下の通り、extensionsフォルダとその下にs4uフォルダを作成します。
```plaintext
/script
  └── /アイドルマスター スターリットシーズン（現在選択中）
      ├── /extensions
      │   └── /s4u
      └── _extension.csx
```

9. s4uフォルダを右クリックし表示されたメニューから「エクスプローラーで開く」を選択し、s4uフォルダをエクスプローラーで表示します。

10. s4uフォルダにダウンロードしたC#マクロファイルを配備します。
```plaintext
/script
  └── /アイドルマスター スターリットシーズン （現在選択中)
      ├── /extensions
      │   └── /s4u
      │       ├S4U.csx
      │       ├S4UCam.csx
      │       ├S4UCore.csx
      │       ├S4UEffect.csx
      │       ├S4UUtil.csx
      │       ├演出スロットツール.csx
      │       └記録.csx
      └── _extension.csx
```
11. _extension.csxをクリックし、先頭に以下の２行を追加します。
```
#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\記録.csx"
#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\演出スロットツール.csx"
```

12. 画面左メニューから、割り当て設定（キーボード／マウス）を選択、マウスサイド２に「S4U記録」のマクロを割り当て。

    ![スクリーンショット](画像のURL)

開発者MEMO: https://imgur.com/a/swFka9L
