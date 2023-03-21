namespace SpaceGame.Lib.Test;

public class TestLogFileHandler
{
    [Fact]
    public void SuccessfulLoggingFile()
    {
        var filePath = "../testLogFileHandler.txt";
        var types = new List<Type> { typeof(string), typeof(ArgumentException) };
        var logFileHandler = new LogFileHandler(filePath, types);

        File.WriteAllText(filePath, string.Empty);

        logFileHandler.Handle();

        var logLines = File.ReadAllText(filePath);
        var expected = "System.String ~ System.ArgumentException" + Environment.NewLine;

        Assert.Equal(expected, logLines);
    }
}
