# StarlitSeason S4U Auto Control Macro

This project provides a KeyToKey macro for automating various actions in the Starlit Season S4U mode.

## ドキュメント項目
- [なにができるのか](#なにができるのか)
- [利用前の下準備](#利用前の下準備)
- [マクロの作成と実行](#マクロの作成と実行)
- [マクロの作成](#マクロの作成)
- [マクロの実行](#マクロの実行)

## なにができるのか
このプロジェクトは、Starlit SeasonのS4Uモードでのアクションを自動化するためのKeyToKeyマクロです。

カメラワークと演出の自動化を目的としています。

S4Uでの手動操作を簡易的に記録する記録モードと、指定した操作をミリ秒単位で再現する再生モードを提供します。
また、４K環境限定になりますが、カスタムスロット設定を保存・復元する機能も提供します。

![概要-img1](https://imgur.com/nhePSDP.jpg)

## 利用前の下準備
このパートでは曲マクロの作成及び実行する為に必要な設定などを行います。

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

6. 画面左メニューから、マクロの作成／設定を選択。

   ![6-img1](https://imgur.com/OgwfaCv.jpg)

7. タブ「C#スクリプト（マクロ）」にて以下の通り、extensionsフォルダとその下にs4uフォルダを作成します。
```plaintext
/script
  └── /アイドルマスター スターリットシーズン（現在選択中）
      ├── /extensions
      │   └── /s4u
      └── _extension.csx
```

8. s4uフォルダを右クリックし表示されたメニューから「エクスプローラーで開く」を選択し、s4uフォルダをエクスプローラーで表示します。

9. s4uフォルダにダウンロードしたC#マクロファイルを配備します。
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
10. _extension.csxをクリックし、先頭に以下の２行を追加し、Ctrl+Sを押して保存します。
```
#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\記録.csx"
#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\演出スロットツール.csx"
```

11. 画面左メニューから、割り当て設定（キーボード／マウス）を選択、マウスサイド２に「S4U記録」のマクロを割り当て。

    ![スクリーンショット](画像のURL)

12. 画面左メニューの一番下のディスクマークをクリックし、ここまでの設定を保存しましょう。

    ![12-img1](https://imgur.com/xQzRfBV.jpg)

13. KeyToKeyのメインウィンドウから「コンソール＞表示」を選択しコンソールウィンドウを表示し、さらに「コンソール＞常に最前面に表示」を選択してコンソールが常に最前面に表示されるように設定してください。

    邪魔な場合は最小化してください。（非表示ではない）

## マクロの作成と実行
このパートでは実際に曲「READY!」のマクロファイルを作成し、実際にマクロを実行します。

### マクロの作成
1. 曲用マクロファイルを作成。
   
   画面左メニューから、「マクロの作成／設定」を選択して「C#スクリプト（マクロ）タブ」を表示し、extensionsフォルダを右クリックして「新しいスクリプトを作成」を選択し、「READY.csx」を作成します。

   ![mc1-img1](https://imgur.com/xhNLcCL.jpg)

2.作成したREADY.csxを開き、 以下の通り作成し保存。
```
#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4U.csx"
#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4UCam.csx"
#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4UCore.csx"
#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4UEffect.csx"

#define ライブ

[Action]
void READY()
{
#if ライブ

    var s4u = new S4U(@this, "マクロサンプル曲 READY");
    
    // ユニット編成人数設定
    s4u.SetIdolUnitSize(5);

    // ライブ開始から13秒後にカメラを切り替え
    s4u.Cam(13000, CameraTarget.No1, CameraType.F_アップ_足から頭へ);
    // ライブ開始から15秒後にカメラをオートに
    s4u.CamAuto(15000);
    // ライブ開始から18秒後に演出スロット1実行
    s4u.ETap(18000, Keys.NumPad1);

    // 実行 - ライブ開始（ローディング終了）を待ち合わせ
    s4u.Execute(); 

#else
#endif
}
```
   
3. _extension.csxに曲マクロを追加

   _extension.csxをクリックし、先頭に以下の１行を追加し、Ctrl+Sを押して保存します。
   
```
#load "Scripts\アイドルマスター スターリットシーズン\extensions\READY.csx"
```

4. 画面左メニューから、割り当て設定（キーボード／マウス）を選択、マウスサイド１に「READY」のマクロを割り当て。
5. これで曲マクロを実行する準備が整いました。

### マクロの実行



開発者MEMO: https://imgur.com/a/swFka9L
