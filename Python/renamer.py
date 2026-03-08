import os

def main():
    print("--- ファイル一括リネームツール ---")

    # 1. フォルダパスの取得
    folder_path = input("フォルダのパスを入力してください: ").strip()

    if not os.path.isdir(folder_path):
        print("エラー: 有効なフォルダではありません。")
        return

    # 2. 置換ルールの入力
    target = input("置換したい文字（例: IMG_）: ")
    replacement = input("置換後の文字（例: 休日_）: ")

    # 3. フォルダ内のファイルをスキャン
    files = os.listdir(folder_path)
    count = 0

    for file_name in files:
        # フルパスを作成
        old_path = os.path.join(folder_path, file_name)

        # ディレクトリではなくファイルのみを対象にする
        if os.path.isfile(old_path):
            if target in file_name:
                # 新しいファイル名を作成
                new_name = file_name.replace(target, replacement)
                new_path = os.path.join(folder_path, new_name)

                # 同名のファイルがなければリネーム実行
                if not os.path.exists(new_path):
                    os.rename(old_path, new_path)
                    print(f"成功: {file_name} -> {new_name}")
                    count += 1
                else:
                    print(f"スキップ（既に存在）: {new_name}")

    print(f"\n完了しました。合計 {count} 個のファイルを変更しました。")

if __name__ == "__main__":
    main()
