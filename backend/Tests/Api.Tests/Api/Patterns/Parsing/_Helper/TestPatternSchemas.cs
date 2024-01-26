namespace Api.Tests.Api.Patterns.Parsing._Helper;

public static class TestPatternSchemas
{
    public static string FcJson() => File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Api/Patterns/Parsing/_Helper", "test.fcjson"));
}