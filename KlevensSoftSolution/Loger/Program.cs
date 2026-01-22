using System.Globalization;
using System.Text.RegularExpressions;

namespace LogStandardizer
{
    public class Program
    {
        // Регулярное выражение для Формата 1: 10.03.2025 15:14:49.523 INFORMATION Сообщение
        private static readonly Regex RegexFormat1 = new Regex(
            @"^(?<date>\d{2}\.\d{2}\.\d{4})\s+(?<time>\d{2}:\d{2}:\d{2}\.\d{3})\s+(?<level>\w+)\s+(?<msg>.*)$");

        // Регулярное выражение для Формата 2: 2025-03-10 15:14:51.5882| INFO|11|Method| Сообщение
        private static readonly Regex RegexFormat2 = new Regex(
            @"^(?<date>\d{4}-\d{2}-\d{2})\s+(?<time>\d{2}:\d{2}:\d{2}\.\d+)\|?\s*(?<level>\w+)\s*\|\d+\|(?<method>[^|]+)\|\s*(?<msg>.*)$");

        static void Main(string[] args)
        {

            string inputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.log");
            //shall find this outs in ...KlevensSoftSolution\Loger\bin\Debug\net10.0
            string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.log");
            string problemsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "problems.txt");

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Файл {inputPath} не найден.");
                return;
            }

            using (var reader = new StreamReader(inputPath))
            using (var writer = new StreamWriter(outputPath))
            using (var problemWriter = new StreamWriter(problemsPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    if (TryParseLog(line, out string standardizedLine))
                    {
                        writer.WriteLine(standardizedLine);
                    }
                    else
                    {
                        problemWriter.WriteLine(line);
                    }
                }
            }

            Console.WriteLine("Обработка завершена.");
        }

        public static bool TryParseLog(string line, out string result)
        {
            result = null;
            string date, time, level, method, message;

            var match1 = RegexFormat1.Match(line);
            if (match1.Success)
            {
                date = ConvertDate(match1.Groups["date"].Value, "dd.MM.yyyy");
                time = match1.Groups["time"].Value;
                level = NormalizeLevel(match1.Groups["level"].Value);
                method = "DEFAULT";
                message = match1.Groups["msg"].Value;
            }
            else
            {
                var match2 = RegexFormat2.Match(line);
                if (match2.Success)
                {
                    date = ConvertDate(match2.Groups["date"].Value, "yyyy-MM-dd");
                    time = match2.Groups["time"].Value;
                    level = NormalizeLevel(match2.Groups["level"].Value);
                    method = match2.Groups["method"].Value.Trim();
                    message = match2.Groups["msg"].Value.Trim();
                }
                else
                {
                    return false;
                }
            }

            result = $"{date}\t{time}\t{level}\t{method}\t{message}";
            return true;
        }

        static string ConvertDate(string dateStr, string inputFormat)
        {
            if (DateTime.TryParseExact(dateStr, inputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                return dt.ToString("dd-MM-yyyy");
            }
            return dateStr;
        }

        static string NormalizeLevel(string level)
        {
            return level.ToUpper() switch
            {
                "INFORMATION" => "INFO",
                "INFO" => "INFO",
                "WARNING" => "WARN",
                "WARN" => "WARN",
                "ERROR" => "ERROR",
                "DEBUG" => "DEBUG",
                _ => level
            };
        }
    }
}
