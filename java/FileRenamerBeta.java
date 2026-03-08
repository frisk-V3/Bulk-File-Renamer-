import java.io.IOException;
import java.nio.file.*;
import java.util.Scanner;
import java.util.concurrent.Executors;
import java.util.regex.Pattern;

public class FileRenamerBeta {
    
    // 実験的機能: Virtual Threadsを使用して非同期で一括リネーム
    public static void main(String[] args) {
        try (var executor = Executors.newVirtualThreadPerTaskExecutor();
             Scanner scanner = new Scanner(System.in)) {

            System.out.println("--- File Renamer Beta (Virtual Threads Mode) ---");
            System.out.print("対象ディレクトリパス: ");
            Path dir = Paths.get(scanner.nextLine());

            System.out.print("置換したい文字列（正規表現可）: ");
            String target = scanner.nextLine();

            System.out.print("置換後の文字列: ");
            String replacement = scanner.nextLine();

            // フォルダ内のファイルを一気にスキャン
            try (var stream = Files.list(dir)) {
                stream.forEach(path -> executor.submit(() -> {
                    processFile(path, target, replacement);
                }));
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private static void processFile(Path path, String target, String replacement) {
        String fileName = path.getFileName().toString();
        
        // モダンなSwitch構文 (Pattern Matching)
        String fileType = switch (fileName.substring(fileName.lastIndexOf(".") + 1).toLowerCase()) {
            case "jpg", "jpeg", "png" -> "IMAGE";
            case "mp4", "mov" -> "VIDEO";
            case "txt", "pdf" -> "DOCUMENT";
            default -> "OTHER";
        };

        // 正規表現による置換
        String newName = fileName.replaceAll(target, replacement);

        if (!fileName.equals(newName)) {
            try {
                Files.move(path, path.resolveSibling(newName), StandardCopyOption.REPLACE_EXISTING);
                System.out.printf("[%s] Renamed: %s -> %s%n", fileType, fileName, newName);
            } catch (IOException e) {
                System.err.println("エラー: " + fileName);
            }
        }
    }
}
