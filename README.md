# BricsAutoDrawingDemo

BricsCAD の .NET API（C#）を利用したプラグイン開発のデモプロジェクトです。

## 構成
- `AutoDrawingPlugin`：BricsCAD上で動作するコマンドDLL
- `AutoDrawingDialog`：WPFベースのUIモジュール

## 開発環境
- BricsCAD V24
- .NET Framework 4.8
- Visual Studio 2022

# ブランチ管理

## ブランチ一覧

|ブランチ名|説明|
|:--------|:---|
|main     |リリース用のブランチ|
|develop  |開発時の本線ブランチ|
|feature/XXX|機能実装を行うブランチ。<br>名前は原則 feature/#[対応するGithubIssue番号]-XXXX とすること<br>ただし、対応するGithubIssue番号が存在しない場合は「#[対応するGithubIssue番号]-」を省略可|

## ワークフロー
### developブランチへのfeatureブランチ取り込み

Github上にてプルリクエストを作成し、コードレビューを実施の上、マージを行うこと

プルリクエスト作成時点で、developブランチに他の修正が取り込まれており、featureブランチ作成当時に起点としたコミットIDに差分が出てしまっている場合には下記対応を行うこと

* 競合が未発生、または競合の解決が容易な場合
    * 最新のdevelopブランチをpullした上で、featureブランチを最新のdevelopブランチにリベースすること。リベースを行う際のGitコマンド例は以下の通り
        ```
        $ git checkout develop
        $ git pull
        $ git checkout feature/XXX  // 修正を実装しているfeatureブランチへcheckout
        $ git rebase develop
          ~ 競合が発生した場合には解消を行い、`git add [修正]` および、`git rebase --continue` を実行する ~ 
        $ git log --graph // featureブランチが最新のdevelopブランチから伸びていることを確認する
        $ git push -f
        ```
* 競合の解決が困難な場合
    * 最新のdevelopブランチをpullした上で、新規にfeatureブランチを作成し、既存の修正をチェリーピックすること


# Issue

## ラベル

チケットの起票時には、適切なラベルを設定すること
各ラベルの概要は下記の通り

|ラベル名|概要|
|:-------|:------|
|開発     |開発枠で対応するもの|
|保守     |運用・保守枠で対応するもの|
|不具合   |内部で検出された不具合|


# Pull request

## 作成

最新版developへのリベースを行った上で、developブランチをマージ先としてPRの作成を行うこと  
PRの内容についてはテンプレートに従って記載を行い、またコード差分を確認して不要なコードやファイルがPRに含まれていないことを確認する

## 作成後

Projectページに移動し、対応するGithub IssueをIn ProgressからWaiting for merge へ移動させる  
その後、マージ担当者へ連絡を行う

## マージ

コードレビュー及び動作確認を実施し、問題がないようであればマージを行う。  
その際複数のマージを連続で行うなどして、対象のブランチが最新develop起点となっていない場合、上述した手順でfeatureブランチを最新のdevelopブランチにリベースすること。

また、マージが完了後、Projectページに移動し、対応するGithub IssueをWaiting for mergeからIn reviewへ移動させる  
