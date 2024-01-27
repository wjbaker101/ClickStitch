namespace Api.Tests.Patterns.Parsing._Helper;

public static class TestPatternSchemas
{
    public static string FcJson() => File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Patterns/Parsing/_Helper", "test.fcjson"));
}