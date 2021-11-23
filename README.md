
# 七四式電子観測儀

現在鋭意開発中の艦これ補助ブラウザです。  


## 実装されている機能

![](https://github.com/andanteyk/ElectronicObserver/wiki/media/mainimage3.png)

各機能はそれぞれウィンドウとして独立しており、自由にドッキング・タブ化するなどしてレイアウト可能です。  
以下では概略を紹介します。**詳しくは[Wikiを参照](https://github.com/andanteyk/ElectronicObserver/wiki)してください。**  

* 内蔵ブラウザ(スクリーンショット, ズーム, ミュートなど)
* 艦隊(状態(遠征中, 未補給など), 制空戦力, 索敵能力)
    * 個艦(Lv, HP, コンディション, 補給, 装備スロット)
    * 艦隊一覧(全艦隊の状態を一目で確認できます)
    * グループ(フィルタリングで艦娘情報を表示)
* 入渠(入渠艦, 残り時間)
* 工廠(建造中の艦名, 残り時間)
* 司令部(提督情報, 資源情報)
* 羅針盤(次の進路, 敵編成・獲得資源等のイベント予測)
* 戦闘(戦闘予測・結果表示)
* 情報(中破絵未回収艦一覧, 海域ゲージ残量など)
* 任務(達成回数/最大値表示)
* 図鑑(艦船/装備図鑑)
* 装備一覧
* 通知(遠征・入渠完了, 大破進撃警告など)
* レコード(開発・建造・ドロップ艦の記録など)
* ウィンドウキャプチャ(他プログラムのウィンドウを取り込む)

なお、全ての機能において艦これ本体の送受信する情報に干渉する操作は行っていません。


## ダウンロード

[リリースページ](https://github.com/andanteyk/ElectronicObserver/releases) もしくは [配布ブログ](http://electronicobserver.blog.fc2.com/) を参照してください。

[更新内容・履歴はこちらで確認できます。](https://github.com/andanteyk/ElectronicObserver/wiki/ChangeLog)  


## 開発者の皆様へ

[開発のための情報はこちらに掲載しています。](https://github.com/andanteyk/ElectronicObserver/wiki/ForDev)  

[Other/Information/](https://github.com/andanteyk/ElectronicObserver/tree/develop/ElectronicObserver/Other/Information) に艦これのAPIや仕様についての情報を掲載しています。  
ご自由にお持ちください。但し内容は保証しません。  

[ライセンスは MIT License です。](https://github.com/andanteyk/ElectronicObserver/blob/master/LICENSE)  


## 使用しているライブラリ

* [DynaJson](https://github.com/fujieda/DynaJson) (JSON データの読み書き) - [MIT License](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/DynaJson.txt)
* [DockPanel Suite](http://dockpanelsuite.com/) (ウィンドウレイアウト) - [MIT License](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/DockPanelSuite.txt)
* [Nekoxy](https://github.com/veigr/Nekoxy) (通信キャプチャ) - [MIT License](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/Nekoxy.txt)
    * [TrotiNet](http://trotinet.sourceforge.net/) - [GNU Lesser General Public License v3.0](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/LGPL.txt)
        * [log4net](https://logging.apache.org/log4net/) - [Apache License version 2.0](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/Apache.txt)


## 連絡先など

* 配布サイト:[ブルネイ工廠電気実験部](http://electronicobserver.blog.fc2.com/) (バグ報告・要望等はこちらにお願いします)
* 開発:[Andante](https://twitter.com/andanteyk)


## このForkリポジトリでの更新履歴
* 20211124
	* 本家v4.7.1に追従(連合vs連合の閉幕雷撃時の問題修正)
* 20211016
	* 航空基地隊：航空隊同士の中隊入れ替え対応
	* 艦隊分析：分析対象になった艦娘の合計経験値表示追加
	* 任務：新規任務の進捗対応
* 20211001
	* 対空砲火詳細：Atlanta砲＋GFCSx2の対空カットイン対応
	* 任務：いくつかの任務の進捗対応
* 20210928
	* 艦隊の簡易火力計算に魚魚水とド水魚を追加(表示されるものは発動優先度順に従う)
	* 空母の夜戦攻撃計算を修正
	* 装甲艇(AB艇)、武装大発、2号戦車アフリカ仕様の遠征資源ボーナスを追加
	* 新規任務の進捗対応
* 20210919
	* 本家v4.7.0に追従(友軍航空支援対応)
* 20210822
	* 編成プリセット：7隻編成に対応
* 20210810
	* 任務：機種転換系の任務において、秘書艦および転換元兵装を装備しているかどうかを見ずに破棄カウントしていたのを修正
	* 任務：一部の任務の進捗表示対応
	* ツール：艦隊分析のCSVファイル読み込み時、キャンセルした場合に読み込んでいたデータがクリアされないようにした
	* 艦隊：艦Lv表記部分のマウスオーバー時に表示される内容へ現在改装可能かどうかを追加
* 20210724
    * 基地航空隊：マウスオーバー時のポップアップ表示に整備レベルを追加 
    * ツール：艦隊分析にCSV読み込み機能を追加、レーダーチャートで単一項目のみ表示した場合に値のラベルを表示するようにした
    * メニュー：編成プリセット再有効化(ちゃんと機能してました、ごめんなさい)
* 20210718
    * ツール：艦隊分析を追加
    * 任務：一部の任務のカテゴリが不明になっているのを修正
    * 任務：一部の任務の進捗表示対応(クォータリー、イヤーリーの編成縛り演習任務など)
* 20210713
    * メニュー：戦果ウィンドウを新規追加
    * メニュー：機能していない編成プリセットのウィンドウを開けないようにした
* 20210703
    * 艦船図鑑：艦型追加、表示上の不自然な点を修正(明石、宗谷など)
    * 装備一覧：ダイアログを表示した状態で装備を変更しても連動しないので、更新するボタンを用意
        * 実はファイル(F)の配下にはあるのだが、トップに欲しかった
    * 戦闘：新カットイン(魚見電2hitなど)の表示対応
    * 戦闘：潜水艦隊攻撃の表示対応
* 20210630
    * 装備一覧：フィルタを実装
