using System.Reflection;

namespace Arc.Compiler.Tests;

public static class ResourceHandler
{
    public static string LoadResourceAsString(string resourceName)
    {
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Arc.Compiler.Tests.Resources.{resourceName}");
        if (stream == null)
        {
            throw new InvalidOperationException($"Resource '{resourceName}' not found.");
        }
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}