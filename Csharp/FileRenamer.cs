using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("--- ファイル一括リネームツール ---");

        // 1. フォルダパスの入力
        Console.Write("フォルダのパスを入力してください: ");
        string? folderPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
        {
            Console.WriteLine("有効なフォルダパスを入力してください。");
            return;
        }

        // 2. 置換ルールの入力
        Console.Write("置換したい文字列（例: IMG_）: ");
        string? target = Console.ReadLine();

        Console.Write("置換後の文字列（例: 旅行_）: ");
        string? replacement = Console.ReadLine();

        try
        {
            // 3. ファイル一覧を取得してループ処理
            string[] files = Directory.GetFiles(folderPath);
            int count = 0;

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);

                // 文字列が含まれている場合のみ処理
                if (!string.IsNullOrEmpty(target) && fileName.Contains(target))
                {
                    string newFileName = fileName.Replace(target, replacement ?? "");
                    string newFilePath = Path.Combine(folderPath, newFileName);

                    // 同名のファイルがないか確認して移動（リネーム）
                    if (!File.Exists(newFilePath))
                    {
                        File.Move(filePath, newFilePath);
                        Console.WriteLine($"成功: {fileName} -> {newFileName}");
                        count++;
                    }
                    else
                    {
                        Console.WriteLine($"スキップ（既に存在）: {newFileName}");
                    }
                }
            }

            Console.WriteLine($"\n完了！ {count} 個のファイルをリネームしました。");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"エラーが発生しました: {ex.Message}");
        }

        Console.WriteLine("キーを押して終了します...");
        Console.ReadKey();
    }
}
