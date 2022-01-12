zxing.dllについて

zxing.dllは、以下の手順で作成しました。


(1)入手元
https://github.com/micjahn/ZXing.Net
同ページの Download ZIP ボタンを押下して
ZXing.Net-master.zip
を取得する。

(2)解凍
zipを解凍する

(3)開く
展開後、zxing.vs2015.slnを開く
セキュリティ警告のダイアログが表示される場合があるが、開く
サポートされていない旨のダイアログが何回か表示されるが、開く
ソース管理のダイアログが表示される場合があるが、一時的に管理なしで作業するを選択して開く

注意：ビルドは、VC2010にて実施してあります。

(4)ビルド
ソリューション全体のうち、
zxing.net4.0を選択して右クリック、ビルドを選択
Build\Debug\net4.0/Build\Release\net4.0の下にdllが作成される


2018/03/09 萩原
