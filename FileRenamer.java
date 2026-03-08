import java.io.IOException;
import java.nio.file.*;
import java.nio.file.attribute.BasicFileAttributes;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.concurrent.atomic.AtomicInteger;

public class FileRenamer {
    public static void main(String[] args) {
        // 対象のディレクトリを指定
        Path dir = Paths.get("C:/Users/YourName/Pictures/Sample"); 

        // リネームの実行
        renameFiles(dir);
    }

    public static void renameFiles(Path dir) {
        try (DirectoryStream<Path> stream = Files.newDirectoryStream(dir)) {
            AtomicInteger counter = new AtomicInteger(1);
            SimpleDateFormat sdf = new SimpleDateFormat("yyyyMMdd");

            for (Path entry : stream) {
                // ディレクトリはスキップ、ファイルのみ対象
                if (Files.isRegularFile(entry)) {
                    // ファイルの作成日時（または更新日時）を取得
                    BasicFileAttributes attr = Files.readAttributes(entry, BasicFileAttributes.class);
                    String dateStr = sdf.format(new Date(attr.lastModifiedTime().toMillis()));

                    // 新しいファイル名を作成 (例: 20231025_001.jpg)
                    String extension = getFileExtension(entry.getFileName().toString());
                    String newName = String.format("%s_%03d%s", dateStr, counter.getAndIncrement(), extension);

                    // リネーム実行
                    Files.move(entry, entry.resolveSibling(newName));
                    System.out.println(entry.getFileName() + " -> " + newName);
                }
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    // 拡張子を取得する補助メソッド
    private static String getFileExtension(String fileName) {
        int lastIndex = fileName.lastIndexOf(".");
        return (lastIndex == -1) ? "" : fileName.substring(lastIndex);
    }
}
