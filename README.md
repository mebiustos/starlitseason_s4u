# StarlitSeason S4U Auto Control Macro

This project provides a KeyToKey macro for automating various actions in the Starlit Season S4U mode.

## ドキュメント項目
- [なにができるのか](#なにができるのか)
- [必要環境](#必要環境)
- [利用前の準備](#利用前の準備)
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
- [演出スロットの保存と復元](#演出スロットの保存と復元)
  - [演出スロットの保存](#演出スロットの保存)
  - [演出スロットの復元](#演出スロットの復元)
## なにができるのか
このプロジェクトは、Starlit SeasonのS4Uモードでのアクションを自動化するためのKeyToKeyマクロです。

カメラワークと演出の自動化を目的としています。

S4Uでの手動操作を簡易的に記録する記録モードと、指定した操作をミリ秒単位で再現する再生モードを提供します。
また、４K環境限定になりますが、カスタムスロット設定を保存・復元する機能も提供します。

※作者以外の環境で正しく動くかどうかは保証できません。

![概要-img1](https://imgur.com/nhePSDP.jpg)

## 必要環境
- スターリットシーズンがインストールされているPC
- カメラの自動コントロールを利用したいならPCに物理的なコントローラーが接続されていること（作者はDualShock4を利用）
- 演出スロットの保存・復元機能を利用したいならスターリットシーズンを4K解像度で実行

## 利用前の準備
1. KeyToKeyをインストールして起動します。

   KeyToKey公式: https://keytokey-dev.net/
   
   KeyToKey GitHub: https://github.com/x0oey6B8/KeyToKey-Web/

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

9. s4uフォルダにダウンロードしたC#マクロファイルを配備します。ダウンロードは[GitHub:starlitseason_s4u](https://github.com/mebiustos/starlitseason_s4u)のCode>DownloadZipから。
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

11. 画面左メニューから、割り当て設定（キーボード／マウス）を選択、画面に表示された「マウスサイド２」をクリックし、表示されたメニューから「マクロを割り当て（押したら）」を選択。マクロ選択画面になるので「S4U記録」を選択して決定。

    マクロを割り当て（押したら）を選択
    ![11-img1](https://imgur.com/r5wbRdJ.jpg)

    正常に割り当てが完了するとこのような設定になっているはずです
    ![11-img2](https://imgur.com/mfABEwz.jpg)

12. 画面左メニューの一番下のディスクマークをクリックし、設定を保存してKeyToKeyに反映させましょう。

    ![12-img1](https://imgur.com/xQzRfBV.jpg)

13. （オプション・強く推奨）KeyToKeyのメインウィンドウから「コンソール＞表示」を選択しコンソールウィンドウを表示し、さらに「コンソール＞常に最前面に表示」を選択してコンソールが常に最前面に表示されるように設定してください。

    ただしコンソールウィンドウが画面の中央に被らないよう注意してください。右下あたりに小さく表示しておくとよいです。邪魔な場合は最小化（非表示ではない）してください。

    ![13-img1](https://imgur.com/W6aWniS.jpg)

    KeyToKeyコンソールウィンドウ イメージ画像
    ![13-img2](https://imgur.com/W3NzOly.jpg)

## マクロの作成と実行
このパートでは実際に曲「READY!」のマクロファイルを作成し、実際にマクロを実行してみます。

### マクロの作成
1. 曲用マクロファイルを作成。
   
   画面左メニューから、「マクロの作成／設定」を選択して「C#スクリプト（マクロ）タブ」を表示し、extensionsフォルダを右クリックして「新しいスクリプトを作成」を選択し、「READY.csx」を作成します。

   ![mc1-img1](https://imgur.com/xhNLcCL.jpg)

2.作成したREADY.csxを開き、 以下の通り記述しCtrl+Sを押して保存。
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
    
    // ユニット編成人数設定（正しく設定しないとカメラのターゲット切り替えが正しく動作しません）
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
    // 演出スロットの復元コード記述場所
#endif
}
```
   
3. _extension.csxに曲マクロを追加

   _extension.csxをクリックし、先頭に以下の１行を追加し、Ctrl+Sを押して保存します。
   
```
#load "Scripts\アイドルマスター スターリットシーズン\extensions\READY.csx"
```

4. 画面左メニューから、割り当て設定（キーボード／マウス）を選択、画面に表示された「マウスサイド１」をクリックし、表示されたメニューから「マクロを割り当て（押したら）」を選択。マクロ選択画面になるので「READY」を選択して決定。
5. 画面左メニューの一番下のディスクマークをクリックし、設定を保存してKeyToKeyに反映させましょう。

    ![作成5-img1](https://imgur.com/xQzRfBV.jpg)
6. これで曲マクロを実行する準備が整いました。

### マクロの実行

1. KeyToKeyのコンソールウィンドウが画面の中央に被っていないことを確認してください。画面の中央はローディング終了判定の為にカラーコードを監視しています。コンソールにはさまざまな情報が表示されるため、非表示とせずに画面右下あたりに小さく表示させて置くことを強く推奨します。
2. スターリットシーズンを起動し、Starge For You!のライブをSTARTしてください。
3. START実行から「Now Loading...」が終了するまでの間に、曲マクロ（READY）を割り当てたマウスのサイド１ボタンを押します。コンソールには以下のログが表示されます。

   ![実行3-img1](https://imgur.com/NaCfwoo.jpg)
   
4. これで、「Now Loading...」が終了し（画面中央のカラーコードが黒#000000以外になったら）、マクロ内部でタイマーが開始され指定された経過時間に指定されたコマンドが実行されていきます。
5. START実行前にマウスサイド１を押してしまうと、マウスポインタも動かない状態になり何もできなくなります。その場合はALT+TABで別のアプリケーションをアクティブにすれば「利用前の準備 手順5」の設定により実行中のマクロは強制的に終了します。

### マクロの修正
1. 曲マクロを編集します。
2. 曲マクロをCtrl+Sで保存します。
3. 画面左メニューの一番下のディスクマークをクリックし、設定を保存してKeyToKeyに反映させましょう。

    ![修正3-img1](https://imgur.com/xQzRfBV.jpg)

### 簡易記録機能

1. KeyToKeyのコンソールウィンドウが画面の中央に被っていないことを確認してください。画面の中央はローディング終了判定の為にカラーコードを監視しています。コンソールにはさまざまな情報が表示されるため、非表示とせずに画面右下あたりに小さく表示させて置くことを強く推奨します。
2. スターリットシーズンを起動し、Starge For You!のライブをSTARTしてください。
3. START実行から「Now Loading...」が終了するまでの間に、記録マクロが割り当てられているマウスのサイド２ボタンを押します。
4. START実行前にマウスサイド２を押してしまうと、マウスポインタも動かない状態になり何もできなくなります。その場合はALT+TABで別のアプリケーションをアクティブにすれば「利用前の下準備 手順5」の設定により実行中のマクロは強制的に終了します。
5. ライブ中に押されたキーボードに対応するC#コードがKeyToKeyのコンソールに表示されます。
6. 記録が終わったらEscapeキーを押すと記録マクロは終了します。その際、記録された全てのC#コードはクリップボードにコピーされます。
   ![記録6-img1](https://imgur.com/3U8cGD4.jpg)
   上記画像の例だと以下の2行がクリップボードにコピーされています。
   ```
   s4u.ETap(4350, Keys.NumPad2);
   s4u.ETap(6318, Keys.NumPad3);
   ``` 
7. これを適宜曲マクロに貼り付けて利用できます。
8. カメラの切り替えタイミングなどは適当にPキーなどを押してタイミングだけ参考にして、コードを自前で記述してください。
9. ちなみにこの記録機能は曲マクロにも実装されていますが、曲マクロは最後のコマンドが実行されたら即終了してしまうので適宜良いように使いわけてください。

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

### カメラは予約で実行されます
あまり気にする必要はないですが、カメラコマンドは必ず「予約」で実行されます。

カメラを予約実行したくない場合は、めんどくさいですが以下の手順で一応実現可能かと思います。
```
// 予約モード解除
s4u.CamReserve(1111, false);
// カメラモード（NormalかFollow）選択
s4u.CamMode(2222, CameraMode.Normal);
// カメラ選択はキーボード直接制御(W,A,S,D)
s4u.ETap(3333, Keys.W);
// カメラタイプはキーボード直接制御(↑,↓,←,→)
s4u.ETap(4444, Keys.Left);
// ターゲット指定
s4u.camTarget(5555, CameraTarget.No2);

// 注意点として以下のようにCamコマンドを利用すると強制的に予約モードに移行してしまいます。
s4u.Cam(9999, CameraTarget.No1, CameraType.F_アップ_足から頭へ);
// よって、これ以降に再度予約ではないカメラを利用したい場合は改めてCamReserveコマンドの実行が必要となります。
```

### カメラターゲット切り替え処理は最適化されます
```
s4u.Cam(1111, CameraTarget.No1, CameraType.F_アップ_足から頭へ);
s4u.Cam(2222, CameraTarget.No2, CameraType.F_スライド_左からアップして右へアウト);
s4u.Cam(3333, CameraTarget.No2, CameraType.F_ロング_正面);
```
このようにターゲットをNo1＞No2＞No2と遷移させた場合、内部的には最適化されてNo1からNo2への変更処理は1回だけ実行されます。3333実行時のNo2ターゲット指定処理は内部的に省略されます。

### カメラターゲット切り替えログ
カメラターゲット切り替えの際にコンソールに出力されるログはキーボードを操作しているような出力になりますが、実際はコントローラーのRスィテック入力によりターゲット切り替えを行っています。

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
本コマンド以降の全てのコマンドの指定経過時間を指定ミリ秒だけずらします。
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
カメラの基本コマンド。ライブ開始から13秒後にターゲットNo1に対して、カメラ「F_アップ_足から頭へ」を実行します。

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
ターゲット切り替え処理の実施リード時間を指定します。本コマンドを実行した以降は、s4u.Cam(13000, CameraTarget.No2, CameraType.F_アップ_足から頭へ)のようなコマンドは、ターゲット切り替え処理のみ指定経過時間より指定されたミリ秒だけ早く実行するようになります。
つまり、上記例だと12900にターゲットを切り替える動作がに実行され、13000に実際のカメラ動作（F_アップ_足から頭へ）が実行されます。

```
s4u.camDisable();
s4u.camEnable();
```
camDisableからcamEnableで挟まれた間にあるカメラ関連コマンドを全て無視します。

## 演出スロットの保存と復元
この機能は作者の動作環境に最適化されているため、そのままでは正しく動作しない可能性があります。

その際は[対処法](#演出スロットの保存と復元がうまく動作しない場合の対処法)を参考に調整してみてください。

また、4K解像度での動作が必須条件となります。
### 演出スロットの保存
1. 画面左メニューから、割り当て設定（キーボード／マウス）を選択、画面に表示された「F9」をクリックし、表示されたメニューから「マクロを割り当て（押したら）」を選択。マクロ選択画面になるので「S4U演出スロット設定コード生成」を選択して決定。

   マクロを割り当て（押したら）を選択
   ![演出保存1-img1](https://imgur.com/XyyF0HH.jpg)

   正常に割り当てが完了するとこのような設定になっているはずです
   ![演出保存1-img2](https://imgur.com/7TPd2cq.jpg)
2. 画面左メニューの一番下のディスクマークをクリックし、設定を保存してKeyToKeyに反映させましょう。

   ![演出保存2-img1](https://imgur.com/xQzRfBV.jpg)
3. STAGE FOR YOU!のメイン画面でF9を押すと自動で演出カスタム画面に移動し演出スロット設定のスキャン処理が始まります。

   STAGE FOR YOU!のメイン画面
   ![演出保存3-img1](https://imgur.com/bwNulmT.jpg)
4. キーボードやコントローラーに触れずに、演出スロット設定のスキャン処理が終わるまで待ちます。

   ![演出保存4-img1](https://imgur.com/Vhde4fP.jpg)
5. スキャン処理が終了すると自動でSTAGE FOR YOU!のメイン画面に戻ってきます。これで処理は終了です。クリップボードに演出スロットの復元コードがコピーされました。
  
   もし、いつまでもスキャン処理が終わらない場合はループしてます。ループしてしまった場合はALT+TABで別のアプリケーションをアクティブにして実行中のマクロを強制停止させましょう。正しく動作させるには各種WAIT値を調整する必要があります。詳しくは[対処法](#演出スロットの保存と復元がうまく動作しない場合の対処法)を参照してください。

6. 曲マクロファイルを開き、#elseと#endifの間にクリップボードにコピーされた演出スロットの復元コードを貼り付け、保存（Ctrl+S）してください。

   ![演出保存6-img1](https://imgur.com/d0RQ1lN.jpg)
### 演出スロットの復元
1. 曲マクロファイルの先頭付近に記述された「#define ライブ」という記述を見つけ、その行の先頭に//を追加して保存（Ctrl+S）します。これにより、ライブ演出マクロ部分が無効化され、かわりに演出スロット復元コード部が有効化されます。

   ![演出復元1-img1](https://imgur.com/zgpfohE.jpg)
2. 画面左メニューの一番下のディスクマークをクリックし、設定を保存してKeyToKeyに反映させましょう。

   ![演出復元2-img1](https://imgur.com/xQzRfBV.jpg)
3. STAGE FOR YOU!のメイン画面で曲マクロを割り当てたマウスのサイド１ボタンを押すと自動で演出カスタム画面に移動し演出スロットの復元が始まります。

   STAGE FOR YOU!のメイン画面
   ![演出復元3-img1](https://imgur.com/bwNulmT.jpg)
5. 復元処理が終了すると自動でSTAGE FOR YOU!のメイン画面に戻ってきます。
7. 復元処理が終了したので、曲マクロファイルを元に戻します。手順1で追加した//を削除し、保存（Ctrl+S）します。
8. 画面左メニューの一番下のディスクマークをクリックし、設定を保存してKeyToKeyに反映させましょう。

   ![演出復元2-img1](https://imgur.com/xQzRfBV.jpg)
### 演出スロットの保存と復元がうまく動作しない場合の対処法
以下の対処で動くようになると思います。

1. 演出スロットツール.csxを開き「var effect = s4u.GetEffect();」の下に以下に示すコードを追加します。
```
// カーソル自動操作（キー入力後）後のWAIT値(デフォルト50)
effect.SetWaitTimeAfterUp(100);

// アイテム選択へ移動後のWAIT値(デフォルト100)
effect.SetWaitTimeAfterEnteringItemList(200);
```

2. 曲マクロファイルを開き、復元コード部の「var effect = s4u.GetEffect();」の下に以下に示すコードを追加します。
```
// カーソル自動操作（キー入力後）後のWAIT値(デフォルト50)
effect.SetWaitTimeAfterUp(100);

// アイテム選択へ移動後のWAIT値(デフォルト100)
effect.SetWaitTimeAfterEnteringItemList(200);
```

## 開発者MEMO
imgur管理URL:

https://imgur.com/a/swFka9L

https://imgur.com/a/CparnCC
