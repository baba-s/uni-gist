# UniGist

「UniGist」は Unity から Gist に投稿できるエディタ拡張です  

# 開発環境

- Unity 2017.4.1f1

# 導入方法

1. 下記のページにアクセスして「UniGist.unitypackage」をダウンロードします  
https://github.com/baba-s/uni-gist/blob/master/UniGist.unitypackage?raw=true
2. ダウンロードした「UniGist.unitypackage」を Unity プロジェクトにインポートします  

# 使い方


![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20180521/20180521213145.png)

スクリプトやテキストファイルを右クリックして「New gist」を選択すると  

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20180521/20180521213151.png)

設定画面が表示されるため、各項目を入力して「New gist」ボタンを押します  
すると、指定したファイルを Gist に投稿できます  

|項目|内容|
|:--|:--|
|User|GitHub のユーザー名|
|Password|GitHub のパスワード|
|Description|投稿するファイルの説明文（任意）|
|Public|公開するかどうか|
|Filename|投稿するファイルの名前|

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20180521/20180521213203.png)

「UniGistSettings」に項目を設定しておくと、  
ユーザー名やパスワード、公開するかどうかの設定を省略できます  

# 著作権について

UniGist は「UnityExtensions.EditorCoroutine」を使用させていただいております  
https://github.com/garettbass/UnityExtensions.EditorCoroutine