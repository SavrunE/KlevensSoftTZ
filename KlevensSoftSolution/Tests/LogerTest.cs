using Xunit;
using LogStandardizer;

public class LogerTest
{
    [Theory]
    // Тест Формата 1 (Преобразование INFORMATION -> INFO, добавление DEFAULT)
    [InlineData(
        "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'",
        "10-03-2025\t15:14:49.523\tINFO\tDEFAULT\tВерсия программы: '3.4.0.48729'")]

    // Тест Формата 2 (Парсинг метода и времени с 4 знаками)
    [InlineData(
        "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'",
        "10-03-2025\t15:14:51.5882\tINFO\tMobileComputer.GetDeviceId\tКод устройства: '@MINDEO-M40-D-410244015546'")]

    // Тест уровня WARNING -> WARN
    [InlineData(
        "11.03.2025 10:00:00.000 WARNING Внимание",
        "11-03-2025\t10:00:00.000\tWARN\tDEFAULT\tВнимание")]

    public void TestLogStandardization(string input, string expected)
    {
        // Act
        bool success = Program.TryParseLog(input, out string actual);

        // Assert
        Assert.True(success);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestInvalidLog()
    {
        // Act
        bool success = Program.TryParseLog("Неправильная строка", out string result);

        // Assert
        Assert.False(success);
    }
}