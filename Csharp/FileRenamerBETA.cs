using System.Text.RegularExpressions;
using System.Globalization;

// --- Beta Features Enabled: Interpolated Strings, Primary Constructors, LINQ Flow ---

Console.WriteLine("🚀 File Renamer Beta v2.0");

// 1. ユーザー設定の取得（インライン検証）
string dir = ReadInput("📂 対象ディレクトリ: ", s => Directory.Exists(s));
string pattern = ReadInput("🔍 検索パターン(正規表現可): ");
string replacement = ReadInput("✏️ 置換後 (連番は {n}, 日付は {date} ): ");

bool isDryRun = true; // Beta機能: デフォルトはシミュレーションモード
Console.WriteLine("\n⚠️  現在は[プレビューモード]です。実際に変更しますか？ (y/N)");
if (Console.ReadLine()?.ToLower() == "y") isDryRun = false;

// 2. ファイル一括スキャン & 並列処理
var files = Directory.EnumerateFiles(dir).ToList();
int counter = 1;

Console.WriteLine(new string('-', 30));

// Parallel.ForEachによる高速リネーム
Parallel.ForEach(files, (filePath) =>
{
    string oldName = Path.GetFileName(filePath);
    string dateStr = File.GetLastWriteTime(filePath).ToString("yyyyMMdd");

    // 動的トークン置換: {n} を連番に、{date} を日付に
    string newName = Regex.Replace(oldName, pattern, replacement)
                          .Replace("{n}", counter.ToString("D3"))
                          .Replace("{date}", dateStr);

    if (oldName != newName)
    {
        string status = isDryRun ? "[PREVIEW]" : "[DONE]";
        if (!isDryRun) File.Move(filePath, Path.Combine(dir, newName));
        
        Console.WriteLine($"{status} {oldName} ➔ {newName}");
        Interlocked.Increment(ref counter);
    }
});

Console.WriteLine(new string('-', 30));
Console.WriteLine(isDryRun ? "🧪 プレビュー完了。実際には変更されていません。" : "✅ 全処理が完了しました。");

// 補助用メソッド
string ReadInput(string label, Func<string, bool>? validator = null)
{
    while (true)
    {
        Console.Write(label);
        string? input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input) && (validator?.Invoke(input) ?? true)) return input;
        Console.WriteLine("❌ 無効な入力です。");
    }
}
