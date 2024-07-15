# StarlitSeason S4U Auto Control Macro

This project provides a KeyToKey macro for automating various actions in the Starlit Season S4U mode.

## ドキュメント項目
- [なにができるのか](#なにができるのか)
- [必要環境](#必要環境)
- [利用前の下準備](#利用前の下準備)
- [マクロの作成と実行](#マクロの作成と実行)
  - [マクロの作成](#マクロの作成)
  - [マクロの実行](#マクロの実行)
  - [マクロの修正](#マクロの修正)
- [簡易記録機能](#簡易記録機能)
- [制限事項](#制限事項)
- [コマンド](#コマンド)
  - [コマンド一覧（共通）](#コマンド一覧共通)
  - [コマンド一覧（演出）](#コマンド一覧演出)
  - [コマンド一覧（カメラ）](#コマンド一覧カメラ)

## なにができるのか
このプロジェクトは、Starlit SeasonのS4Uモードでのアクションを自動化するためのKeyToKeyマクロです。

カメラワークと演出の自動化を目的としています。

S4Uでの手動操作を簡易的に記録する記録モードと、指定した操作をミリ秒単位で再現する再生モードを提供します。
また、４K環境限定になりますが、カスタムスロット設定を保存・復元する機能も提供します。

でもこのマクロを使っても曲にちゃんと合わせて演出するのはきついです。動画編集のようにはいきません。

また、作者以外の環境で動くかどうかはわかりません。このドキュメントも適宜追記していこうと思っていますが、力尽きても許してほしい。

![概要-img1](https://imgur.com/nhePSDP.jpg)

## 必要環境
- スターリットシーズンがインストールされているPC
- カメラの自動コントロールを利用したいならPCに物理的なコントローラーが接続されていること（作者はDualShock4を利用）
- 演出スロットの保存・復元機能を利用したいならスターリットシーズンを4K解像度で実行

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

9. s4uフォルダにダウンロードしたC#マクロファイルを配備します。ダウンロードは[GitHub:starlitseason_s4u](https://github.com/mebiustos/starlitseason_s4u)のCode>DownloadZipにて。
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

13. （オプション・かなり推奨）KeyToKeyのメインウィンドウから「コンソール＞表示」を選択しコンソールウィンドウを表示し、さらに「コンソール＞常に最前面に表示」を選択してコンソールが常に最前面に表示されるように設定してください。

    ただしコンソールウィンドウが画面の中央に被らないよう注意してください。右下あたりに小さく表示しておくとよいです。邪魔な場合は最小化（非表示ではない）してください。

## マクロの作成と実行
このパートでは実際に曲「READY!」のマクロファイルを作成し、実際にマクロを実行してみます。

### マクロの作成
1. 曲用マクロファイルを作成。
   
   画面左メニューから、「マクロの作成／設定」を選択して「C#スクリプト（マクロ）タブ」を表示し、extensionsフォルダを右クリックして「新しいスクリプトを作成」を選択し、「READY.csx」を作成します。

   ![mc1-img1](https://imgur.com/xhNLcCL.jpg)

2.作成したREADY.csxを開き、 以下の通り記述し保存。
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

1. KeyToKeyのコンソールウィンドウが画面の中央に被っていないことを確認してください。画面の中央はローディング終了判定の為にカラーコードを監視しています。さまざまな情報が表示されるため、非表示とせずに画面右下あたりに小さく表示させて置くことを強く推奨します。
2. スターリットシーズンを起動し、Starge For You!のライブをSTARTしてください。
3. START実行から「Now Loading...」が終了するまでの間に、曲マクロ（READY）を割り当てたマウスのサイド１ボタンを押します。
4. これで、「Now Loading...」が終了し、画面中央のカラーコードが黒#000000以外になった時点からタイマーが開始され指定された経過時間に指定されたコマンドが実行されます。
5. START実行前にマウスサイド１を押してしまうと、マウスポインタも動かない状態になり何もできなくなります。その場合はALT+TABで別のアプリケーションをアクティブにすれば「利用前の下準備 手順5」の設定により曲マクロ処理は強制的に終了します。

### マクロの修正
（執筆中）

### 簡易記録機能

1. KeyToKeyのコンソールウィンドウが画面の中央に被っていないことを確認してください。画面の中央はローディング終了判定の為にカラーコードを監視しています。さまざまな情報が表示されるため、非表示とせずに画面右下あたりに小さく表示させて置くことを強く推奨します。
2. スターリットシーズンを起動し、Starge For You!のライブをSTARTしてください。
3. START実行から「Now Loading...」が終了するまでの間に、記録マクロが割り当てられているマウスのサイド２ボタンを押します。
4. ライブ中に押されたキーボードに対応するC#コードがKeyToKeyのコンソールに表示されます。
5. 記録が終わったらEscapeキーを押すと記録マクロは終了します。その際、記録された全てのC#コードはクリップボードにコピーされます。
6. これを適宜曲マクロのC#コードに貼り付けて、利用できます。
7. カメラの切り替えタイミングなどは適当にPキーなどを押してタイミングだけ参考にして、コードを自前で記述してください。
8. ちなみにこの記録機能は曲マクロにも実装されていますが、曲マクロは最後のコマンドが実行されたら即終了してしまうので適宜良いように使いわけてください。

## 制限事項
### コマンド実行順
コマンドは記述された順番に実行されます。
経過時間でソートされたりはしません。
```
s4u.ETap(18000, Keys.NumPad1);
s4u.ETap(100, Keys.NumPad2);
```
上記のマクロを実行すると18秒後にテンキーの１が押され、その直後にテンキーの2が押されます。
このように矛盾した記述を持つ曲マクロはマクロ開始時にコンソールに警告が出力されます。

### カメラ予約
あまり気にする必要はないですが、カメラコマンドは必ず「予約」で実行されます。

### カメラターゲット処理
```
s4u.Cam(1111, CameraTarget.No1, CameraType.F_アップ_足から頭へ);
s4u.Cam(2222, CameraTarget.No2, CameraType.F_スライド_左からアップして右へアウト);
s4u.Cam(3333, CameraTarget.No2, CameraType.F_ロング_正面);
```
このようにターゲットをNo1＞No2＞No2と遷移させた場合、内部的には最適化されてNo1からNo2への変更処理は1回だけ実行されます。3333実行時にはNo2のターゲット指定は内部的に省略されます。

## コマンド

### コマンド一覧（共通）
```
s4u.SetIdolUnitSize(5);
```
ユニットの編成人数を指定します。
デフォルトは5です。
編成人数を正しく設定しないとカメラのターゲット選択が正しく動作しません。

```
s4u.SetGlobalShift(300);
```
本コマンド以降のコマンドの指定経過時間を指定ミリ秒だけずらします。
この例だと以降のコマンドの指定経過時間は+300ミリ秒されます。

### コマンド一覧（演出）
```
s4u.ETap(18000, Keys.NumPad1);
```
指定経過時間に指定したキーを押下します。
上記の例だとライブ開始から18秒後にテンキーの１を押します。

```
s4u.ETap(18000, Keys.NumPad1, 3, 10);
```
指定経過時間に指定したキーを複数回押下します。
上記の例だとライブ開始から18秒後にテンキーの１を３回10ミリ秒毎押します。

```
s4u.ETap(18000, s4u.KeysArray(Keys.NumPad1, Keys.NumPad2));
```
指定経過時間に指定した複数のキーを押下します。
上記の例だとライブ開始から18秒後にテンキーの１と２を押します。

### コマンド一覧（カメラ）
```
s4u.Cam(13000, CameraTarget.No1, CameraType.F_アップ_足から頭へ);
```
カメラの基本コマンド。みたまんま。ライブ開始から13秒後にターゲットNo1に対して、カメラ「F_アップ_足から頭へ」を実行します。

```
s4u.Cam(13000, CameraTarget.No1, 100, CameraType.F_アップ_足から頭へ);
```
カメラの基本コマンドの亜種。
ターゲット切り替え処理の実施リード時間を指定できます。ターゲット切り替え処理を指定の指定ミリ秒だけ早く実施します。つまり上記例だと100ミリ秒早い12900にターゲット切り替え処理が行われ、13000に実際のカメラ処理が適用されます。

```
s4u.Cam(13000, CameraType.F_アップ_足から頭へ);
```
カメラの基本コマンドの亜種その２。ターゲット指定なしバージョン。

```
s4u.CamTarget(13000, CameraTarget.No1);
```
カメラのターゲット指定だけを先にやっておきたいときに利用。

```
s4u.CamAuto(15000);
```
オートカメラへの戻し。

```
s4u.SetTargetAfterWaitMs(25);
```
スターリットシーズンのMVの仕様上、カメラのターゲット切り替えの後にだけは必ず内部的にWAITを入れる必要があり、その内部的なWAIT時間を指定できます。デフォルトは25ミリ秒です。カメラのターゲット切り替えが不安定な場合はマクロの先頭付近でこの値を大きくすると解決すると思います。もしくは次に示すSetCamGlobalTargetLeadMsコマンドを利用してください。

```
s4u.SetCamGlobalTargetLeadMs(100);
```
ターゲット切り替え処理の実施リード時間を指定します。本コマンド以降、s4u.Cam(13000, CameraTarget.No2, CameraType.F_アップ_足から頭へ)のようなコマンドは、ターゲット切り替え処理のみ指定経過時間より指定されたミリ秒だけ早く実行するようになります。
つまり、上記例だと12900にターゲットを切り替える動作がに実行され、13000に実際のカメラ動作（F_アップ_足から頭へ）が実行されます。

```
s4u.camDisable();
s4u.camEnable();
```
上記camDisableからcamEnableで挟まれた間にあるカメラ操作を全て無視します。

## 開発者MEMO
imgur管理URL: https://imgur.com/a/swFka9L
